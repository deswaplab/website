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

public class TokenPairs
{
    public IList<TokenPair> Inner { get; set; }

    public TokenPairs()
    {
        Inner = [
            new TokenPair{
                BaseAssetName = "WETH",
                BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
                BaseAssetDecimals = 18,
                QuoteAssetName = "USDC",
                QuoteAssetAddress = "0xFCAE2250864A678155f8F4A08fb557127053E59E",
                QuoteAssetDecimals = 6,
                NftAddress = "0xe10C396C0635BEE8986de9A870852F528A0E0107",
                Network = SupportedNetworks.GetNetwork(11155111)!,
            },
        ];
    }

    public TokenPair? FindByName(string name)
    {
        var res = Inner.FirstOrDefault(item => item.BaseAssetName + "/" + item.QuoteAssetName == name);
        return res;
    }

    public TokenPair? FindByNftContract(string nftContract)
    {
        var res = Inner.FirstOrDefault(item => item.NftAddress.Equals(nftContract, StringComparison.CurrentCultureIgnoreCase));
        return res;
    }

    public IList<TokenPair> FilterByChainId(long chainId)
    {
        return Inner.Where(item => item.Network.ChainId == chainId).ToList();
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
        new Network{Name = "Pylogon Mumbai", ChainId=80001, EtherscanHost="https://sepolia.arbiscan.io", OpenseaHost="https://mumbai.polygonscan.com", ReservoirHost="https://api-mumbai.reservoir.tools", Logo="polygon_logo.svg", IsTestNet=true},

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
        "name": "calls",
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
        "name": "puts",
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
