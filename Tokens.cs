namespace DeswapApp;

using System.Net.Http.Json;

public class TokenPair
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string QuoteAssetName { get; set; }

    public required string QuoteAssetAddress { get; set; }

    public required int QuoteAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    // 在中心化交易所的符号，用来获取参考价格
    public string? OkxTickSymbol {get; set;}

    public string? OkxOptUly {get; set;}

    public required Network Network { get; set; }

    // Like: https://sepolia.etherscan.io/token/0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9?a=0x7e727520B29773e7F23a8665649197aAf064CeF1
    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }

    public string Name => BaseAssetName + "/" + QuoteAssetName;
}

public static class TokenPairs
{
    public static readonly IList<TokenPair> Inner = [
        // WETH/USDC
        new TokenPair{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            OkxTickSymbol = "ETH-USDC",
            OkxOptUly = "ETH-USD",
            QuoteAssetAddress = "0x1c7D4B196Cb0C7B01d743Fbc6116a902379C7238",
            QuoteAssetDecimals = 6,
            NftAddress = "0x3e6362CC4DF74F529C7Fb70371333FC2eb34bdBC",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            OkxTickSymbol = "MATIC-USDC",
            OkxOptUly = "MATIC-USD",
            QuoteAssetAddress = "0x9999f7Fea5938fD3b1E26A12c3f2fb024e194f97",
            QuoteAssetDecimals = 6,
            NftAddress = "0x3e6362CC4DF74F529C7Fb70371333FC2eb34bdBC",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },

        // WETH/TUSDC
        new TokenPair{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x1A19012C237D403dC495087a444269CA4c5b9025",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0xD10dBe32045aD92aaDDD1f4f5BC241e4ae26609c",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
    ];

    public static IList<TokenPair> FilterByChainId(long chainId)
    {
        return Inner.Where(item => item.Network.ChainId == chainId).ToList();
    }

    // list of nft contract address, in lower case for better comparision
    public static IList<string> FilterSupportedContracts(long chainId)
    {
        var inn = Inner.Where(item => item.Network.ChainId == chainId)
            .Select(p => p.NftAddress.ToLower())
            .ToList();
        return inn;
    }
}

public class Network
{
    public required string Name { get; set; }

    public long ChainId { get; set; }

    public required string EtherscanHost { get; set; }

    // opensea 主界面对应的host
    public required string OpenseaHost { get; set; }

    // opensea api 对应的host
    public required string OpenseaApiHost { get; set; }

    // reservoir 可能有些网络不支持，所以可能会null
    public string? ReservoirHost { get; set; }

    public required string Logo { get; set; } // svg文件，保存在 wwwroot/img 下

    public bool IsTestNet { get; set; }
}

public static class SupportedNetworks
{
    public static readonly IList<Network> Inner = [
        // we use reservoir to fetch user tokens, so supported chains are limited, in the future we should switch to other api providers
        new Network{
            Name = "Sepolia",
            ChainId=11155111,
            EtherscanHost="https://sepolia.etherscan.io",
            OpenseaHost="https://testnets.opensea.io/assets/sepolia",
            OpenseaApiHost="https://testnets-api.opensea.io/api/v2/chain/sepolia",
            ReservoirHost="https://api-sepolia.reservoir.tools",
            Logo="ethereum_logo.svg",
            IsTestNet=true
        },
        new Network{
            Name = "Pylogon Mumbai", 
            ChainId=80001, 
            EtherscanHost="https://mumbai.polygonscan.com", 
            OpenseaHost="https://testnets.opensea.io/assets/mumbai", 
            OpenseaApiHost="https://testnets-api.opensea.io/api/v2/chain/mumbai",
            ReservoirHost="https://api-mumbai.reservoir.tools", 
            Logo="polygon_logo.svg", 
            IsTestNet=true
        },

    ];

    public static Network? GetNetwork(long chainId)
    {
        var res = Inner.FirstOrDefault(item => item.ChainId == chainId);
        return res;
    }
}

public enum OptionsKind
{
    CALL,
    PUT,
}
