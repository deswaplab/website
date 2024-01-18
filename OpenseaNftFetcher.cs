namespace DeswapApp;

using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

public class OpenseaNftFetcher(HttpClient httpClient, ILogger<OpenseaNftFetcher> logger) : INftFetcher
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly ILogger<OpenseaNftFetcher> _logger = logger;

    public async Task<IList<UserToken>> GetUserOptionTokens(string userAddress, long chainId)
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

        var supportedContracts = TokenPairs.FilterSupportedContracts(chainId);
        var tokens = curTokens
            .Where(item => supportedContracts.Contains(item.Contract.ToLower()))
            .Select(item =>
            {
                return new UserToken
                {
                    TokenId = long.Parse(item.Identifier),
                    ChainId = chainId,
                    Contract = item.Contract!,
                    ImageData = ParseImageSvg(item.MetadataUrl),
                    MaturityDate = ParseMaturityDate(item.Traits),
                    OptionsKind = ParseOptionsKind(item.Traits),
                    BaseAssetAmount = ParseBaseAssetAmount(item.Traits),
                    QuoteAssetAmount = ParseQuoteAssetAmount(item.Traits),
                };
            })
            .ToList();
        return tokens;
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
    }

    private static DateTimeOffset ParseMaturityDate(IList<OpenseaTraits> tokenAttributes)
    {
        foreach (var attr in tokenAttributes)
        {
            if (attr.TraitType == "maturityDate" && attr.DisplayType == "date")
            {
                // 此处取到的数据包含小数点，所以要转为decimal
                var maturityDate = DateTimeOffset.FromUnixTimeSeconds((long)attr.Value.GetDecimal());
                return maturityDate;
            }
        }
        throw new Exception("can find maturity date");
    }

    private static string ParseOptionsKind(IList<OpenseaTraits> tokenAttributes)
    {

        foreach (var attr in tokenAttributes)
        {
            if (attr.TraitType == "optionsKind")
            {
                return attr.Value.GetString() ?? "";
            }
        }
        throw new Exception("no options kind found");
    }

    private static BigInteger ParseBaseAssetAmount(IList<OpenseaTraits> tokenAttributes)
    {
        foreach (var attr in tokenAttributes)
        {
            if (attr.TraitType == "baseAssetAmount" && attr.DisplayType == "number")
            {
                return BigInteger.Parse(attr.Value.GetRawText());
            }
        }
        throw new Exception("no baseAssetAmount found");
    }

    private static BigInteger ParseQuoteAssetAmount(IList<OpenseaTraits> tokenAttributes)
    {
        foreach (var attr in tokenAttributes)
        {
            if (attr.TraitType == "quoteAssetAmount" && attr.DisplayType == "number")
            {
                return BigInteger.Parse(attr.Value.GetRawText());
            }
        }
        throw new Exception("no quoteAssetAmount found");
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

        [JsonPropertyName("traits")]
        public IList<OpenseaTraits> Traits { get; set; } = [];
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
}
