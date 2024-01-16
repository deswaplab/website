namespace DeswapApp;

using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json.Serialization;

public class ReservoirNftFetcher(HttpClient httpClient) : INftFetcher
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IList<UserToken>> GetUserOptionTokens(string userAddress, long chainId)
    {
        var curNetwork = SupportedNetworks.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        var supportedTokenPairs = TokenPairs.FilterByChainId(chainId);
        if (supportedTokenPairs.Count == 0)
        {
            throw new Exception($"no supported token pairs for chain {chainId}");
        }

        IList<ReservoirTokenResponse> curTokens = [];
        bool hasMore = true;
        string continuation = "";
        while (hasMore)
        {
            string url = $"{curNetwork.ReservoirHost}/users/{userAddress}/tokens/v8?includeAttributes=true&limit=200";
            if (continuation != "")
            {
                url += $"&continuation={continuation}";
            }
            var tokensResponse = await _httpClient.GetFromJsonAsync<ReservoirTokensResponse>(url);
            if (tokensResponse is not null && tokensResponse.Tokens is not null)
            {
                curTokens = [.. curTokens, .. tokensResponse!.Tokens];
            }
            else
            {
                break;
            }
            if (tokensResponse is not null && tokensResponse.Continuation is not null)
            {
                hasMore = true;
                continuation = tokensResponse.Continuation;
            }
            else
            {
                break;
            }
        }

        // 变化
        var supportedContracts = TokenPairs.FilterSupportedContracts(chainId);
        var tokens = curTokens
            .Where(item => item.Token is not null && item.Token.Contract is not null)
            .Where(item => supportedContracts.Contains(item.Token!.Contract!.ToLower()))
            .Select(item =>
            {
                return new UserToken
                {
                    TokenId = long.Parse(item.Token!.TokenId!),
                    ChainId = item.Token.ChainId,
                    Contract = item.Token.Contract!,
                    ImageData = item.Token!.Metadata!.ImageOriginal!,
                    MaturityDate = ParseMaturityDate(item.Token.Attributes),
                    OptionsKind = ParseOptionsKind(item.Token.Attributes),
                    BaseAssetAmount = ParseBaseAssetAmount(item.Token.Attributes),
                    QuoteAssetAmount = ParseQuoteAssetAmount(item.Token.Attributes),
                };
            })
            .ToList();

        return tokens;
    }

    private static DateTimeOffset ParseMaturityDate(IList<ReservoirTokenAttribute> tokenAttributes)
    {
        foreach(var attr in tokenAttributes)
        {
            if (attr.Key == "maturityDate" && attr.Value is not null)
            {
                var maturityDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(attr.Value));
                return maturityDate;
            }
        }
        throw new Exception("can find maturity date");
    }

    private static string ParseOptionsKind(IList<ReservoirTokenAttribute> tokenAttributes)
    {
        foreach(var attr in tokenAttributes)
        {
            if (attr.Key == "optionsKind" && attr.Value is not null)
            {
                return attr.Value;
            }
        }
        throw new Exception("no options kind found");
    }

    private static BigInteger ParseBaseAssetAmount(IList<ReservoirTokenAttribute> tokenAttributes)
    {
        foreach(var attr in tokenAttributes)
        {
            if (attr.Key == "baseAssetAmount" && attr.Value is not null)
            {
                return BigInteger.Parse(attr.Value);
            }
        }
        throw new Exception("no baseAssetAmount found");
    }

    private static BigInteger ParseQuoteAssetAmount(IList<ReservoirTokenAttribute> tokenAttributes)
    {
        foreach(var attr in tokenAttributes)
        {
            if (attr.Key == "quoteAssetAmount" && attr.Value is not null)
            {
                return BigInteger.Parse(attr.Value);
            }
        }
        throw new Exception("no quoteAssetAmount found");
    }

    public class ReservoirTokensResponse
    {
        [JsonPropertyName("tokens")]
        public IList<ReservoirTokenResponse> Tokens { get; set; } = [];

        [JsonPropertyName("continuation")]
        public string? Continuation { get; set; }
    }

    public class ReservoirTokenResponse
    {
        [JsonPropertyName("token")]
        public ReservoirToken? Token { get; set; }

    }
    public class ReservoirToken
    {
        [JsonPropertyName("chainId")]
        public long ChainId { get; set; }

        [JsonPropertyName("contract")]
        public string? Contract { get; set; }

        [JsonPropertyName("tokenId")]
        public string? TokenId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ReservoirTokenMetadata? Metadata { get; set; }

        [JsonPropertyName("attributes")]
        public IList<ReservoirTokenAttribute> Attributes { get; set; } = [];
    }

    public class ReservoirTokenMetadata
    {
        [JsonPropertyName("imageOriginal")]
        public string? ImageOriginal { get; set; }
    }

    public class ReservoirTokenAttribute
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}
