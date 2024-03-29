namespace DeswapApp;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Web3;

// 发查询请求的客户端，不同网络可以使用不同的api，如ethereum走opensea，moonbeam走covan

public interface IApiClient
{
    Task<IList<NFTMetadataBase>> GetUserTokens(string userAddress, long chainId);

    Task RefreshMetadata(string contractAddress, long chainId, long tokenId);
}

public record UserNftBase
{
    public long TokenId { get; set; }

    public long ChainId { get; set; }

    public required string Contract { get; set; }

    public required string ImageData { get; set; }
}

public record UserOptionNFT : UserNftBase
{
   
    public DateTimeOffset MaturityDate { get; set; }

    public OptionsKind OptionsKind { get; set; }

    public decimal BaseAssetAmount { get; set; }

    public decimal QuoteAssetAmount { get; set; }

    public decimal Price { get; set; }

    public bool Listable()
    {
        if (!Burnable() && !Exercisable())
        {
            return true;
        }
        return false;
    }

    public bool Burnable()
    {
        var now = DateTime.Now;
        if (now >= MaturityDate.AddDays(1))
        {
            return true;
        }
        return false;
    }

    public bool Exercisable()
    {
        var now = DateTime.Now;
        // TODO: 方便测试暂且注释，上线时解除
        // if (now >= MaturityDate && now < MaturityDate.AddDays(1))
        // {
        //     return true;
        // }
        if (now < MaturityDate.AddDays(1))
        {
            return true;
        }
        return false;
    }

    public (string, BigInteger) GetPayAsset()
    {
        var tokenPair = new OptionsContracts().Inner.Where(p => p.Network.ChainId == ChainId)
            .Where(item => item.Address.Equals(Contract, StringComparison.CurrentCultureIgnoreCase))
            .First();
        if (OptionsKind == OptionsKind.CALL)
        {
            return (tokenPair.QuoteAsset.Address, Web3.Convert.ToWei(QuoteAssetAmount, tokenPair.QuoteAsset.Decimals));
        }
        else if (OptionsKind == OptionsKind.PUT)
        {
            return (tokenPair.BaseAsset.Address, Web3.Convert.ToWei(BaseAssetAmount, tokenPair.BaseAsset.Decimals));
        }
        throw new Exception("invalid token");
    }

    public static UserOptionNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserOptionNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            MaturityDate = NftMetadataParser.ParseMaturityDate(metadataUrl, "maturityDate"),
            OptionsKind = NftMetadataParser.ParseOptionsKind(metadataUrl),
            BaseAssetAmount = NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount"),
            QuoteAssetAmount = NftMetadataParser.ParseAmount(metadataUrl, "quoteAssetAmount"),
            Price = NftMetadataParser.ParsePrice(metadataUrl),
        };
    }
}

public record UserLotteryNFT : UserNftBase
{
    public required string Status { get; set; }

    public DateTimeOffset DrawTime { get; set; }

    public decimal BaseAssetAmount { get; set; }

    public bool Listable()
    {
        if (Status == "open")
        {
            return true;
        }
        return false;
    }

    public bool Drawable()
    {
        if (DateTime.Now > DrawTime)
        {
            return true;
        }
        return false;
    }

    public static UserLotteryNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserLotteryNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            Status = NftMetadataParser.ParseString(metadataUrl, "status"), // open|close
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            DrawTime = NftMetadataParser.ParseMaturityDate(metadataUrl, "drawTime"),
            BaseAssetAmount = NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount"),
        };
    }
}

public record UserRedEnvelopeNFT : UserNftBase
{
    public required string Status { get; set; }

    public decimal BaseAssetAmount { get; set; }

    public static UserRedEnvelopeNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserRedEnvelopeNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            Status = NftMetadataParser.ParseString(metadataUrl, "status"), // open|close
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            BaseAssetAmount = NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount"),
        };
    }
}

public record UserRouletteNFT : UserNftBase
{
    public decimal BaseAssetAmount { get; set; }

    public DateTimeOffset OpenTime { get; set; }

    public required string Writer { get; set; }

    public bool Openable(string userAddress)
    {
        if (DateTime.Now > OpenTime)
        {
            return true;
        }
        else if (userAddress.Equals(Writer, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static UserRouletteNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserRouletteNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            BaseAssetAmount = NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount"),
            OpenTime = NftMetadataParser.ParseMaturityDate(metadataUrl, "openTime"),
            Writer = NftMetadataParser.ParseString(metadataUrl, "writer"),
        };
    }
}

public record UserBlackJackNFT : UserNftBase
{
    public decimal DealerBalance { get; set; }

    public required string Writer { get; set; }

    public bool CanDeposit(string userAddress)
    {
        if (userAddress.Equals(Writer, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static UserBlackJackNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserBlackJackNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            DealerBalance = NftMetadataParser.ParseAmount(metadataUrl, "dealerBalance"),
            Writer = NftMetadataParser.ParseString(metadataUrl, "writer"),
        };
    }

}

public class NftApi(HttpClient httpClient, ILogger<NftApi> logger)
{
    private readonly IApiClient openseaApiClient = new OpenseaNftApi(httpClient);

    private readonly IApiClient covalentApiClient = new CovalentNftApi(httpClient, logger);

    // contract, metadata_url
    // 通过contract进行过滤，通过metadata_url解析出详情
    // public abstract Task<IList<NFTMetadataBase>> GetUserTokens(string userAddress, long chainId);
    private IApiClient GetApiClient(long chainId)
    {
        if (chainId == 11155111 || chainId == 8001)
        {
            return openseaApiClient;
        }
        return covalentApiClient;
    }

    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        // TODO: 有些网络没有刷新的方法，要注意
        var client = GetApiClient(chainId);
        await client.RefreshMetadata(contractAddress, chainId, tokenId);
    }

    public async Task<IList<NFTMetadataBase>> GetUserTokens(string userAddress, long chainId)
    {
        var client = GetApiClient(chainId);
        return await client.GetUserTokens(userAddress, chainId);
    }

    public async Task<IList<UserNftBase>> GetUserNftBases(string userAddress, long chainId)
    {
        var metadatas = await GetUserTokens(userAddress, chainId);
        var allContracts = new AllContracts().Inner.Select(item => item.Item2.ToLower());
        return metadatas.Where(item => allContracts.Contains(item.Contract.ToLower())).Select(item => new UserNftBase {TokenId = item.TokenId, Contract = item.Contract, ChainId = chainId, ImageData = NftMetadataParser.ParseImageSvg(item.MetadataUrl) }).ToList();
    }
}

// NFT metadata 统一用opensea格式，所以可以统一方式解析
public static class NftMetadataParser
{
    public static string ParseImageSvg(string metadataUrl)
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

    public class OpenseaTraits
    {
        [JsonPropertyName("trait_type")]
        public required string TraitType { get; set; } // "optionsKind"

        [JsonPropertyName("display_type")]
        public string? DisplayType { get; set; } // "date" or null or "number"...

        [JsonPropertyName("value")]
        public required JsonElement Value { get; set; } // 可能是字符串，也可能是数字，由display_type决定
    }

    class MetadataUrlPayload
    {
        [JsonPropertyName("image")]
        public required string Image { get; set; }

        [JsonPropertyName("attributes")]
        public IList<OpenseaTraits>? Attributes { get; set; }
    }

    public static DateTimeOffset ParseMaturityDate(string metadataUrl, string name)
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

    public static OptionsKind ParseOptionsKind(string metadataUrl)
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

    public static decimal ParseAmount(string metadataUrl, string name)
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

    public static decimal ParsePrice(string metadataUrl)
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

    public static string ParseString(string metadataUrl, string name)
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
}

public record NFTMetadataBase(long TokenId, string Contract, string MetadataUrl);

public class OpenseaNftApi(HttpClient httpClient) : IApiClient
{
    private readonly string _openseaApiKey = "25ad74a6d36042cfa5bb92c9980f9519";

    private readonly HttpClient _httpClient = httpClient;

    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        // https://api.opensea.io/api/v2/chain/sepolia/contract/address/nfts/0/refresh
        var curNetwork = SupportedNetworks.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        string url = $"{curNetwork.OpenseaApiHost}/contract/{contractAddress}/nfts/{tokenId}/refresh";
        if (!_httpClient.DefaultRequestHeaders.Contains("x-api-key"))
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _openseaApiKey);
        }
        await _httpClient.PostAsync(url, null);
    }

    public async Task<IList<NFTMetadataBase>> GetUserTokens(string userAddress, long chainId)
    {
        var curNetwork = SupportedNetworks.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        if (string.IsNullOrEmpty(curNetwork.OpenseaApiHost))
        {
            return [];
        }

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
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _openseaApiKey);
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
        return curTokens.Select(item => new NFTMetadataBase(long.Parse(item.Identifier), item.Contract, item.MetadataUrl)).ToList();
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
}

public class CovalentNftApi(HttpClient httpClient, ILogger<NftApi> logger) : IApiClient
{
    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        await Task.CompletedTask;
        logger.LogWarning("covalent doesn't support refresh");
    }

    public async Task<IList<NFTMetadataBase>> GetUserTokens(string userAddress, long chainId)
    {
        var curNetwork = SupportedNetworks.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        if (string.IsNullOrEmpty(curNetwork.CovalentApiHost))
        {
            return [];
        }

        string url = $"{curNetwork.CovalentApiHost}/address/{userAddress}/balances_nft/?with-uncached=true";
        var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes("cqt_rQ6VwJkQBx99RQmbBk896mrFfX9G:"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        var tokensResponse = await httpClient.GetFromJsonAsync<CovalentTokensResponse>(url);
        IList<NFTMetadataBase> result = [];
        foreach (var item in tokensResponse!.Data.Items)
        {
            var tokens = item.NFTData;
            foreach (var token in tokens)
            {
                result.Add(new NFTMetadataBase(long.Parse(token.TokenId), item.ContractAddress, token.TokenUrl));
            }
        }
        return result;
    }

    public class CovalentTokensResponse
    {
        [JsonPropertyName("data")]
        public required CovalentData Data { get; set; }
    }

    public class CovalentData
    {
        [JsonPropertyName("items")]
        public required IList<CovalentContractItem> Items { get; set; }
    }

    public class CovalentContractItem
    {
        [JsonPropertyName("contract_address")]
        public required string ContractAddress { get; set; }

        [JsonPropertyName("nft_data")]
        public required IList<CovalentNFTData> NFTData { get; set; }
    }

    public class CovalentNFTData
    {
        [JsonPropertyName("token_id")]
        public required string TokenId { get; set; }

        [JsonPropertyName("token_url")]
        public required string TokenUrl { get; set; }

    }
}
