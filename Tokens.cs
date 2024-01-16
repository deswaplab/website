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

    public required Network Network {get; set;}

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
            QuoteAssetAddress = "0x1c7D4B196Cb0C7B01d743Fbc6116a902379C7238",
            QuoteAssetDecimals = 6,
            NftAddress = "0x5cC0c202a402cf02Be73387C20867158db8bb235",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            QuoteAssetAddress = "0x9999f7Fea5938fD3b1E26A12c3f2fb024e194f97",
            QuoteAssetDecimals = 6,
            NftAddress = "0x5cC0c202a402cf02Be73387C20867158db8bb235",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },

        // WETH/TUSDC
        new TokenPair{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x4b8D9d541A5B60CC1dD90D2fc870D28c965Ea2Fb",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new TokenPair{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x4b8D9d541A5B60CC1dD90D2fc870D28c965Ea2Fb",
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
    public string Name { get; set; } = "";

    public long ChainId { get; set; }

    public string EtherscanHost { get; set; } = "";

    public string OpenseaHost { get; set; } = "";

    public string ReservoirHost { get; set; } = "";

    public string Logo {get; set;} = ""; // svg文件，保存在 wwwroot/img 下

    public bool IsTestNet {get; set;}
}

public static class SupportedNetworks
{
    public static readonly IList<Network> Inner = [
        // we use reservoir to fetch user tokens, so supported chains are limited, in the future we should switch to other api providers
        new Network{Name = "Sepolia", ChainId=11155111, EtherscanHost="https://sepolia.etherscan.io", OpenseaHost="https://testnets.opensea.io/assets/sepolia", ReservoirHost="https://api-sepolia.reservoir.tools", Logo="ethereum_logo.svg", IsTestNet=true},
        new Network{Name = "Pylogon Mumbai", ChainId=80001, EtherscanHost="https://mumbai.polygonscan.com", OpenseaHost="https://testnets.opensea.io/assets/mumbai", ReservoirHost="https://api-mumbai.reservoir.tools", Logo="polygon_logo.svg", IsTestNet=true},

    ];

    public static Network? GetNetwork(long chainId)
    {
        var res = Inner.FirstOrDefault(item => item.ChainId == chainId);
        return res;
    }
}

public class OptionsNFT
{
    public static readonly string abi = """
[
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "burn",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "enum OptionsNFT.OptionsKind",
                "name": "kind",
                "type": "uint8"
            },
            {
                "internalType": "uint256",
                "name": "baseAssetAmount",
                "type": "uint256"
            },
            {
                "internalType": "uint256",
                "name": "quoteAssetAmount",
                "type": "uint256"
            },
            {
                "internalType": "uint256",
                "name": "maturityDate",
                "type": "uint256"
            }
        ],
        "name": "mint",
        "outputs": [
            {
                "internalType": "uint256",
                "name": "",
                "type": "uint256"
            }
        ],
        "stateMutability": "nonpayable",
        "type": "function"
    },
    {
        "inputs": [
            {
                "internalType": "uint256",
                "name": "tokenId",
                "type": "uint256"
            }
        ],
        "name": "exercise",
        "outputs": [],
        "stateMutability": "nonpayable",
        "type": "function"
    }
]
""";
}
