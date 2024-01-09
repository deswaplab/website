namespace DeswapApp;

public class TokenPair
{
    public string BaseAssetName { get; set; } = "";

    public string BaseAssetAddress { get; set; } = "";

    public int BaseAssetDecimals { get; set; } = 0;

    public string QuoteAssetName { get; set; } = "";

    public string QuoteAssetAddress { get; set; } = "";

    public int QuoteAssetDecimals { get; set; } = 0;

    public string NftAddress { get; set; } = "";

    public int ChainId { get; set; } = 0;

    public string EtherScanHost {get; set;} = "";

    // Like: https://sepolia.etherscan.io/token/0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9?a=0x7e727520B29773e7F23a8665649197aAf064CeF1
    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return EtherScanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }
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
                ChainId = 11155111,
                EtherScanHost = "https://sepolia.etherscan.io" 
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
}

public class EthNetwork
{
    public static string ChainIdToNetwork(long chainId)
    {
        if (chainId == 11155111)
        {
            return "sepolia";
        } else if (chainId == 5)
        {
            return "goerli";
        }
        return "ethereum";
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
