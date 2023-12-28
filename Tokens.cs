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
}

public class TokenPairs
{
    public IList<TokenPair> Inner {get; set;}

    public TokenPairs() {
        Inner = [
            new TokenPair{BaseAssetName = "WETH", BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9", BaseAssetDecimals = 18, QuoteAssetName = "USDC", QuoteAssetAddress = "0x6CcB30b54Bf2B1Cf47E093B92aECCE404F9824Cd", QuoteAssetDecimals = 6, NftAddress = "0xEB0bDd73ADB291B245CDB48c650Cf684607D2033", ChainId = 11155111 },
        ];
    }

    public TokenPair? FindByPair(string name) {
        var res = Inner.FirstOrDefault(item => item.BaseAssetName + "/" + item.QuoteAssetName == name);
        return res;
    }
    public static IList<TokenPair> GetTokenPairs()
    {
        return [
                new TokenPair{BaseAssetName = "WETH", BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9", BaseAssetDecimals = 18, QuoteAssetName = "USDC", QuoteAssetAddress = "0x6CcB30b54Bf2B1Cf47E093B92aECCE404F9824Cd", QuoteAssetDecimals = 6, NftAddress = "0xEB0bDd73ADB291B245CDB48c650Cf684607D2033", ChainId = 11155111 },
        ];
    }
}
