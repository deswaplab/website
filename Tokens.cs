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

    public TokenPair? FindByPair(string name)
    {
        var res = Inner.FirstOrDefault(item => item.BaseAssetName + "/" + item.QuoteAssetName == name);
        return res;
    }
}
