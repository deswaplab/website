namespace DeswapApp;

using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

public class OpenseaNftFetcher(HttpClient httpClient, ILogger<OpenseaNftFetcher> logger) : INftFetcher
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly ILogger<OpenseaNftFetcher> _logger = logger;

    public async Task<IList<UserOptionNFT>> GetUserOptionTokens(string userAddress, long chainId)
    {
        var curTokens = await FetchAllUserTokens(userAddress, chainId);

        var supportedContracts = TokenPairs.FilterSupportedContracts(chainId);
        var tokens = curTokens
            .Where(item => supportedContracts.Contains(item.Contract.ToLower()))
            .Select(item =>
            {
                return new UserOptionNFT
                {
                    TokenId = long.Parse(item.Identifier),
                    ChainId = chainId,
                    Contract = item.Contract!,
                    ImageData = ParseImageSvg(item.MetadataUrl),
                    MaturityDate = ParseMaturityDate(item.MetadataUrl, "maturityDate"),
                    OptionsKind = ParseOptionsKind(item.MetadataUrl),
                    BaseAssetAmount = ParseBaseAssetAmount(item.MetadataUrl, "baseAssetAmount"),
                    QuoteAssetAmount = ParseQuoteAssetAmount(item.MetadataUrl),
                    Price = ParsePrice(item.MetadataUrl),
                };
            })
            .ToList();
        return tokens;
    }

    private async Task<IList<OpenseaNft>> FetchAllUserTokens(string userAddress, long chainId)
    {
        var curNetwork = SupportedNetworks.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");

        bool hasMore = true;
        string next = "";
        IList<OpenseaNft> curTokens = [];
        while (hasMore)
        {
            string url = $"{curNetwork.OpenseaApiHost}/account/{userAddress}/nfts?limit=200";
            if (next != "")
            {
                url += $"&next={next}";
            }
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "25ad74a6d36042cfa5bb92c9980f9519");
            var tokensResponse = await _httpClient.GetFromJsonAsync<OpenseaTokensResponse>(url);
            curTokens = [.. curTokens, .. tokensResponse!.Nfts];
            if (tokensResponse.Next is not null)
            {
                hasMore = true;
                next = tokensResponse.Next;
            }
            else
            {
                break;
            }
        }
        return curTokens;
    }

    private static string ParseImageSvg(string metadataUrl)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            return payload!.Image;
        }
        return "";
    }

    class MetadataUrlPayload
    {
        [JsonPropertyName("image")]
        public required string Image { get; set; }

        [JsonPropertyName("attributes")]
        public IList<OpenseaTraits>? Attributes { get; set; }
    }

    private static DateTimeOffset ParseMaturityDate(string metadataUrl, string name)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == name && attr.DisplayType == "date")
                {
                    // 此处取到的数据包含小数点，所以要转为decimal
                    var maturityDate = DateTimeOffset.FromUnixTimeSeconds((long)attr.Value.GetDecimal());
                    return maturityDate;
                }
            }
        }
        
        throw new Exception("can find maturity date");
    }

    private static OptionsKind ParseOptionsKind(string metadataUrl)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == "optionsKind")
                {
                    var s = attr.Value.GetString() ?? "";
                    if (s == "call")
                    {
                        return OptionsKind.CALL;
                    }
                    else if (s == "put")
                    {
                        return OptionsKind.PUT;
                    }
                }
            }
        }
        throw new Exception("no options kind found");
    }

    private static decimal ParseBaseAssetAmount(string metadataUrl, string name)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == name && attr.DisplayType == "number")
                {
                    return attr.Value.GetDecimal();
                }
            }
        }
        throw new Exception("no baseAssetAmount found");
    }

    private static decimal ParseQuoteAssetAmount(string metadataUrl)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == "quoteAssetAmount" && attr.DisplayType == "number")
                {
                    return attr.Value.GetDecimal();
                }
            }
        }
        throw new Exception("no quoteAssetAmount found");
    }

    private static decimal ParsePrice(string metadataUrl)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == "price" && attr.DisplayType == "number")
                {
                    return attr.Value.GetDecimal();
                }
            }
        }
        throw new Exception("no price found");
    }

    private static string ParseString(string metadataUrl, string name)
    {
        string head = "data:application/json;base64,";
        if (metadataUrl.StartsWith(head))
        {
            var blob = Convert.FromBase64String(metadataUrl[head.Length..]);
            var json = Encoding.UTF8.GetString(blob);
            var payload = JsonSerializer.Deserialize<MetadataUrlPayload>(json);
            foreach (var attr in payload!.Attributes!)
            {
                if (attr.TraitType == name)
                {
                    return attr.Value.GetString() ?? "";
                }
            }
        }
        throw new Exception("no price found");
    }

    public class OpenseaTokensResponse
    {
        [JsonPropertyName("nfts")]
        public required IList<OpenseaNft> Nfts { get; set; }

        [JsonPropertyName("next")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Next { get; set; }
    }

    public class OpenseaNft
    {
        [JsonPropertyName("identifier")]
        public required string Identifier { get; set; } // "identifier": "3"

        [JsonPropertyName("contract")]
        public required string Contract { get; set; } // lowercase, "contract": "0x4b8d9d541a5b60cc1dd90d2fc870d28c965ea2fb"

        [JsonPropertyName("metadata_url")]
        public required string MetadataUrl { get; set; }
    }

    public class OpenseaTraits
    {
        [JsonPropertyName("trait_type")]
        public required string TraitType { get; set; } // "optionsKind"

        [JsonPropertyName("display_type")]
        public string? DisplayType { get; set; } // "date" or null or "number"...

        [JsonPropertyName("value")]
        public required JsonElement Value { get; set; } // 可能是字符串，也可能是数字，由display_type决定
    }

    // fetch user lottery nfts
    public async Task<IList<UserLotteryNFT>> GetUserLotteryTokens(string userAddress, long chainId)
    {
        var curTokens = await FetchAllUserTokens(userAddress, chainId);
        var supportedContracts = LotteryContracts.Inner
            .Where(p => p.Network.ChainId == chainId)
            .Select(p => p.NftAddress.ToLower())
            .ToList();
        var tokens = curTokens.Where(item => supportedContracts.Contains(item.Contract.ToLower()))
            .Select(item => {
                return new UserLotteryNFT
                {
                    TokenId = long.Parse(item.Identifier),
                    ChainId = chainId,
                    Contract = item.Contract!,
                    Status = ParseString(item.MetadataUrl, "status"), // open|close
                    ImageData = ParseImageSvg(item.MetadataUrl),
                    DrawTime = ParseMaturityDate(item.MetadataUrl, "drawTime"),
                    BaseAssetAmount = ParseBaseAssetAmount(item.MetadataUrl, "baseAssetAmount"),
                };
            })
            .ToList();
        return tokens;
    }

    // fetch user red envelope nfts
    public async Task<IList<UserRedEnvelopeNFT>> GetUserRedEnvelopeTokens(string userAddress, long chainId)
    {
        var curTokens = await FetchAllUserTokens(userAddress, chainId);
        var supportedContracts = RedEnvelopeContracts.Inner
            .Where(p => p.Network.ChainId == chainId)
            .Select(p => p.NftAddress.ToLower())
            .ToList();
        var tokens = curTokens.Where(item => supportedContracts.Contains(item.Contract.ToLower()))
            .Select(item => {
                return new UserRedEnvelopeNFT
                {
                    TokenId = long.Parse(item.Identifier),
                    ChainId = chainId,
                    Contract = item.Contract!,
                    Status = ParseString(item.MetadataUrl, "status"), // open|close
                    ImageData = ParseImageSvg(item.MetadataUrl),
                    BaseAssetAmount = ParseBaseAssetAmount(item.MetadataUrl, "baseAssetAmount"),
                };
            })
            .ToList();
        return tokens;
    }
}
