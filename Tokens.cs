namespace DeswapApp;

public class TokenPair
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string QuoteAssetName { get; set; }

    public required string QuoteAssetAddress { get; set; }

    public required int QuoteAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    public required OptionsKind OptionsKind { get; set; }

    // 在中心化交易所的符号，用来获取参考价格
    public string? OkxTickSymbol { get; set; }

    public string? OkxOptUly { get; set; }

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
        // WETH/TUSDC Call
        new TokenPair{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0xA57d0Caa974caf3a5F508051464E4ac0a69FdA0C",
            Network = SupportedNetworks.GetNetwork(11155111)!,
            OptionsKind = OptionsKind.CALL,
        },
        // WETH/TUSDC Put
        new TokenPair{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x5552dD062df90ED68443A2e3E194e3b814a86C2d",
            Network = SupportedNetworks.GetNetwork(11155111)!,
            OptionsKind = OptionsKind.PUT
        },
        // mumbai WETH/TUSDC Call
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0xA57d0Caa974caf3a5F508051464E4ac0a69FdA0C",
            Network = SupportedNetworks.GetNetwork(80001)!,
            OptionsKind = OptionsKind.CALL
        },
        // mumbai WETH/TUSDC Put
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x5552dD062df90ED68443A2e3E194e3b814a86C2d",
            Network = SupportedNetworks.GetNetwork(80001)!,
            OptionsKind = OptionsKind.PUT
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

public static class LotteryContracts
{
    public static readonly IList<LotteryContract> Inner = [
        new LotteryContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            NftAddress = "0x062056aC6249DE97483Eb0800fc8a28E977c3754",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new LotteryContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0x062056aC6249DE97483Eb0800fc8a28E977c3754",
            Network = SupportedNetworks.GetNetwork(80001)!,
        }
    ];
}

public class LotteryContract
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    public required Network Network { get; set; }

    public string Name => "Lottery NFT " + BaseAssetName;

    // Like: https://sepolia.etherscan.io/token/0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9?a=0x7e727520B29773e7F23a8665649197aAf064CeF1
    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }
}

public static class RedEnvelopeContracts
{
    public static readonly IList<RedEnvelopeContract> Inner = [
        new RedEnvelopeContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            NftAddress = "0x74e3199174ec457c8D237F36cB4DC9F60bF66208",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new RedEnvelopeContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0x74e3199174ec457c8D237F36cB4DC9F60bF66208",
            Network = SupportedNetworks.GetNetwork(80001)!,
        }
    ];
}

public class RedEnvelopeContract
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    public required Network Network { get; set; }

    public string Name => "RedEnvelope NFT " + BaseAssetName;

    // Like: https://sepolia.etherscan.io/token/0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9?a=0x7e727520B29773e7F23a8665649197aAf064CeF1
    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
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
