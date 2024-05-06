namespace DeswapApp;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// 发查询请求的客户端，不同网络可以使用不同的api，如ethereum走opensea，moonbeam走covan

public interface IApiClient
{
    Task RefreshMetadata(string contractAddress, long chainId, long tokenId);

    Task<IList<UserNftBase>> GetUserNftBase(string userAddress, long chainId);
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

    public decimal? BaseAssetAmount { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required string BaseAssetKind { get; set; }

    public long BaseAssetTokenId { get; set; }

    public required string QuoteAssetKind { get; set; }

    public required string QuoteAssetAddress { get; set; }

    public decimal? QuoteAssetAmount { get; set; }

    public BigInteger? QuoteAssetAmountWei { get; set; }

    public long QuoteAssetTokenId { get; set; } = 0;

    public bool Exercisable()
    {
        if (DateTime.Now < MaturityDate.AddDays(1))
        {
            return true;
        }
        return false;
    }

    public static UserOptionNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        string baseAssetKind = NftMetadataParser.ParseString(metadataUrl, "baseAssetKind"); // ERC20/721/1155
        string quoteAssetKind = NftMetadataParser.ParseString(metadataUrl, "quoteAssetKind"); // ERC20/721/1155

        return new UserOptionNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),

            MaturityDate = NftMetadataParser.ParseMaturityDate(metadataUrl, "maturityDate"),
            BaseAssetKind = baseAssetKind,
            BaseAssetAddress = NftMetadataParser.ParseString(metadataUrl, "baseAssetAddress"),
            BaseAssetAmount = baseAssetKind == "ERC20" ? NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount") : 0,
            BaseAssetTokenId = (baseAssetKind == "ERC721" || baseAssetKind == "ERC1155") ? NftMetadataParser.ParseLong(metadataUrl, "baseAssetTokenId") : 0,
            QuoteAssetKind = quoteAssetKind,
            QuoteAssetAddress = NftMetadataParser.ParseString(metadataUrl, "quoteAssetAddress"),
            QuoteAssetAmount = quoteAssetKind == "ERC20" ? NftMetadataParser.ParseAmount(metadataUrl, "quoteAssetAmount") : 0,
        };
    }
}

public record UserLotteryNFT : UserNftBase
{
    public required string Status { get; set; }

    public DateTimeOffset DrawTime { get; set; }

    public decimal BaseAssetAmount { get; set; }

    public required string AssetKind { get; set; }

    public long BaseAssetTokenId { get; set; }

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
        string assetKind = NftMetadataParser.ParseString(metadataUrl, "assetKind"); // ERC20/721/1155
        return new UserLotteryNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            Status = NftMetadataParser.ParseString(metadataUrl, "status"), // open|close
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            DrawTime = NftMetadataParser.ParseMaturityDate(metadataUrl, "drawTime"),
            AssetKind = assetKind,
            BaseAssetAmount = assetKind == "ERC20" ? NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount") : 0,
            BaseAssetTokenId = (assetKind == "ERC721" || assetKind == "ERC1155") ? NftMetadataParser.ParseLong(metadataUrl, "baseAssetTokenId") : 0,
        };
    }
}

public record UserRedEnvelopeNFT : UserNftBase
{
    public required string Status { get; set; }

    public decimal BaseAssetAmount { get; set; }

    public required string AssetKind { get; set; }

    public long BaseAssetTokenId { get; set; }

    public static UserRedEnvelopeNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        string assetKind = NftMetadataParser.ParseString(metadataUrl, "assetKind"); // ERC20/721/1155
        return new UserRedEnvelopeNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            Status = NftMetadataParser.ParseString(metadataUrl, "status"), // open|close
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            AssetKind = assetKind,
            BaseAssetAmount = assetKind == "ERC20" ? NftMetadataParser.ParseAmount(metadataUrl, "baseAssetAmount") : 0,
            BaseAssetTokenId = (assetKind == "ERC721" || assetKind == "ERC1155") ? NftMetadataParser.ParseLong(metadataUrl, "baseAssetTokenId") : 0,
        };
    }
}

public record UserRouletteNFT : UserNftBase
{
    public required string BaseAssetAddress { get; set; }

    public required string AssetKind { get; set; }

    public static UserRouletteNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserRouletteNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            AssetKind = NftMetadataParser.ParseString(metadataUrl, "assetKind"),
            BaseAssetAddress = NftMetadataParser.ParseString(metadataUrl, "baseAssetAddress"),
        };
    }
}

public record UserSicboNFT : UserNftBase
{
    public required string BaseAssetAddress { get; set; }

    public static UserSicboNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserSicboNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            BaseAssetAddress = NftMetadataParser.ParseString(metadataUrl, "baseAssetAddress"),
        };
    }
}

public record UserVoteNFT : UserNftBase
{
    public long ChoiceLength { get; set; }

    public static UserVoteNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserVoteNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
            ChoiceLength = NftMetadataParser.ParseLong(metadataUrl, "choiceLength"),
        };
    }
}

public record UserWritingNFT : UserNftBase
{
    public static UserWritingNFT FromStr(long chainId, string contractAddress, long tokenId, string metadataUrl)
    {
        return new UserWritingNFT
        {
            TokenId = tokenId,
            ChainId = chainId,
            Contract = contractAddress,
            ImageData = NftMetadataParser.ParseImageSvg(metadataUrl),
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
    // contract, metadata_url
    // 通过contract进行过滤，通过metadata_url解析出详情
    private IApiClient? GetApiClient(long chainId)
    {
        NetworkCore? network = NetworkConfig.GetNetwork(chainId);
        if (network?.OpenseaApiHost is not null)
        {
            return new OpenseaNftApi(httpClient);
        }
        else if (network?.BlockscoutHost is not null)
        {
            return new BlockscoutNftApi(httpClient);
        }
        else if (network?.CovalentApiHost is not null)
        {
            return new CovalentNftApi(httpClient, logger);
        }
        return null;
    }

    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        var client = GetApiClient(chainId);
        if (client is not null)
        {
            await client.RefreshMetadata(contractAddress, chainId, tokenId);
        }
    }

    public async Task<IList<UserNftBase>> GetUserNftBases(string userAddress, long chainId)
    {
        var client = GetApiClient(chainId);
        if (client is not null)
        {
            var metadatas = await client.GetUserNftBase(userAddress, chainId);
            return metadatas;
        }
        return [];
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

    public static long ParseLong(string metadataUrl, string name)
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
                    return attr.Value.GetInt64();
                }
            }
        }
        throw new Exception("no long field found");
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

public class OpenseaNftApi(HttpClient httpClient) : IApiClient
{
    private readonly string _openseaApiKey = "25ad74a6d36042cfa5bb92c9980f9519";

    private readonly HttpClient _httpClient = httpClient;

    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        // https://api.opensea.io/api/v2/chain/sepolia/contract/address/nfts/0/refresh
        var curNetwork = NetworkConfig.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        string url = $"{curNetwork.OpenseaApiHost}/contract/{contractAddress}/nfts/{tokenId}/refresh";
        if (!_httpClient.DefaultRequestHeaders.Contains("x-api-key"))
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _openseaApiKey);
        }
        await _httpClient.PostAsync(url, null);
    }

    public async Task<IList<UserNftBase>> GetUserNftBase(string userAddress, long chainId)
    {
        var curNetwork = NetworkConfig.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
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
        var allContracts = ContractConfig.GetNFTContracts(curNetwork).Select(item => item.ToLower());

        return curTokens.Where(item => allContracts.Contains(item.Contract.ToLower()))
            .Select(item => new UserNftBase
            {
                TokenId = long.Parse(item.Identifier),
                ChainId = chainId,
                Contract = item.Contract,
                ImageData = NftMetadataParser.ParseImageSvg(item.MetadataUrl)
            }).ToList();
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

    public async Task<IList<UserNftBase>> GetUserNftBase(string userAddress, long chainId)
    {
        var curNetwork = NetworkConfig.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        if (string.IsNullOrEmpty(curNetwork.CovalentApiHost))
        {
            return [];
        }

        string url = $"{curNetwork.CovalentApiHost}/address/{userAddress}/balances_nft/?with-uncached=true";
        var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes("cqt_rQ6VwJkQBx99RQmbBk896mrFfX9G:"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        var tokensResponse = await httpClient.GetFromJsonAsync<CovalentTokensResponse>(url);
        IList<UserNftBase> result = [];
        foreach (var item in tokensResponse!.Data.Items)
        {
            var tokens = item.NFTData;
            foreach (var token in tokens)
            {
                result.Add(new UserNftBase{
                    TokenId = long.Parse(token.TokenId),
                    ChainId = chainId,
                    Contract = item.ContractAddress, 
                    ImageData =  NftMetadataParser.ParseImageSvg(token.TokenUrl)
                });
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


public class BlockscoutNftApi(HttpClient httpClient) : IApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly string DefaultImage = "data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjMDAwMDAwIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCAzMiAzMiIgaWQ9Imljb24iIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHN0eWxlPi5jbHMtMXtmaWxsOm5vbmU7fTwvc3R5bGU+PC9kZWZzPjx0aXRsZT5uby1pbWFnZTwvdGl0bGU+PHBhdGggZD0iTTMwLDMuNDE0MSwyOC41ODU5LDIsMiwyOC41ODU5LDMuNDE0MSwzMGwyLTJIMjZhMi4wMDI3LDIuMDAyNywwLDAsMCwyLTJWNS40MTQxWk0yNiwyNkg3LjQxNDFsNy43OTI5LTcuNzkzLDIuMzc4OCwyLjM3ODdhMiwyLDAsMCwwLDIuODI4NCwwTDIyLDE5bDQsMy45OTczWm0wLTUuODMxOC0yLjU4NTgtMi41ODU5YTIsMiwwLDAsMC0yLjgyODQsMEwxOSwxOS4xNjgybC0yLjM3Ny0yLjM3NzFMMjYsNy40MTQxWiIvPjxwYXRoIGQ9Ik02LDIyVjE5bDUtNC45OTY2LDEuMzczMywxLjM3MzMsMS40MTU5LTEuNDE2LTEuMzc1LTEuMzc1YTIsMiwwLDAsMC0yLjgyODQsMEw2LDE2LjE3MTZWNkgyMlY0SDZBMi4wMDIsMi4wMDIsMCwwLDAsNCw2VjIyWiIvPjxyZWN0IGlkPSJfVHJhbnNwYXJlbnRfUmVjdGFuZ2xlXyIgZGF0YS1uYW1lPSImbHQ7VHJhbnNwYXJlbnQgUmVjdGFuZ2xlJmd0OyIgY2xhc3M9ImNscy0xIiB3aWR0aD0iMzIiIGhlaWdodD0iMzIiLz48L3N2Zz4K";

    private string GetDefaultImageByAddress(NetworkCore network, string contractAddress)
    {
        NFTContract? contract = ContractConfig.GetNFTContract(network, contractAddress);
        if (contract is null)
        {
            return DefaultImage;
        }
        return contract.DefaultImage;
    }
    public async Task RefreshMetadata(string contractAddress, long chainId, long tokenId)
    {
        // TODO:
        await Task.CompletedTask;
    }

    public async Task<IList<UserNftBase>> GetUserNftBase(string userAddress, long chainId)
    {
        var curNetwork = NetworkConfig.GetNetwork(chainId) ?? throw new Exception($"invalid chainId, {chainId}");
        if (string.IsNullOrEmpty(curNetwork.BlockscoutHost))
        {
            return [];
        }
        var allContracts = ContractConfig.GetNFTContracts(curNetwork).Select(item => item.ToLower());

        bool hasMore = true;
        string next = "";
        IList<UserNftBase> nfts = [];
        while (hasMore)
        {
            string url = $"{curNetwork.BlockscoutHost}/api/v2/addresses/{userAddress}/tokens";
            if (next != "")
            {
                url += $"?{next}";
            }
            var tokensResponse = await _httpClient.GetFromJsonAsync<BlockscoutTokensResponse>(url);
            foreach (var item in tokensResponse!.Items)
            {
                if (item.TokenId is not null && item.Token.Address is not null && allContracts.Contains(item.Token.Address.ToLower()))
                {
                    nfts.Add(new UserNftBase
                    {
                        TokenId = item.TokenId.Value,
                        ChainId = chainId,
                        Contract = item.Token.Address,
                        ImageData = item.TokenInstance?.ImageUrl ?? GetDefaultImageByAddress(curNetwork, item.Token.Address),
                    });
                }
            }
            if (tokensResponse.NextPageParams is not null)
            {
                hasMore = true;
                var fiatValue = tokensResponse.NextPageParams.FiatValue;
                var id = tokensResponse.NextPageParams.Id;
                var itemsCount = tokensResponse.NextPageParams.ItemsCount;
                var value = tokensResponse.NextPageParams.Value;
                next = $"fiat_value={fiatValue}&id={id}&items_count={itemsCount}&value={value}";
            }
            else
            {
                break;
            }
        }

        return nfts;
    }

    public class BlockscoutTokensResponse
    {
        [JsonPropertyName("items")]
        public required IList<BlockscoutTokenItem> Items { get; set; }

        [JsonPropertyName("next_page_params")]
        public BlockscoutNextPageParams? NextPageParams { get; set; }
    }

    public class BlockscoutNextPageParams
    {
        [JsonPropertyName("fiat_value")]
        public string? FiatValue { get; set; }

        [JsonPropertyName("id")]
        public required long Id { get; set; }

        [JsonPropertyName("items_count")]
        public required int ItemsCount { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }
    }

    public class BlockscoutTokenItem
    {
        [JsonPropertyName("token")]
        public required BlockscoutToken Token { get; set; }


        [JsonPropertyName("token_id")]
        public long? TokenId { get; set; }

        [JsonPropertyName("token_instance")]
        public BlockscoutTokenInstance? TokenInstance { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; } // BigInteger for erc20
    }

    public class BlockscoutToken
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; } // ERC-20, ERC-721, ERC-1155
    }

    public class BlockscoutTokenInstance
    {
        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; } // data:image/svg+xml;base64,...
    }
}
