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

    public static readonly string TestTokens = """
{"tokens":[{"token":{"chainId":11155111,"contract":"0x5cc0c202a402cf02be73387c20867158db8bb235","tokenId":"0","kind":"erc721","name":"#Deswap WETH/USDC #0","image":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZHV%2BYGB%2Bazr1wWFwCrOqAwI0RhTqtbnEJloVxSREk%2BHVI4bbmDWwvgnb0GEA22Fty4BN82yCUpirVBcXFn8h3mEoOqfYCvrn2HI1HZLtXS1l3aOFjBUAi9XqetZ9ukXf0%3D.svg?width=512","imageSmall":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZHV%2BYGB%2Bazr1wWFwCrOqAwI0RhTqtbnEJloVxSREk%2BHVI4bbmDWwvgnb0GEA22Fty4BN82yCUpirVBcXFn8h3mEoOqfYCvrn2HI1HZLtXS1l3aOFjBUAi9XqetZ9ukXf0%3D.svg?width=250","imageLarge":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZHV%2BYGB%2Bazr1wWFwCrOqAwI0RhTqtbnEJloVxSREk%2BHVI4bbmDWwvgnb0GEA22Fty4BN82yCUpirVBcXFn8h3mEoOqfYCvrn2HI1HZLtXS1l3aOFjBUAi9XqetZ9ukXf0%3D.svg?width=1000","metadata":{"imageOriginal":"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIiBmb250LXNpemU9IjE0Ij7wn5OIIFdFVEgvVVNEQzwvdGV4dD48dGV4dCB4PSI3MCIgeT0iMjQwIiBzdHlsZT0iZm9udC1zaXplOjEwMHB4Ij7wn4y7PC90ZXh0Pjx0ZXh0IHg9IjMwIiB5PSI0MDAiPklEOiAwPC90ZXh0Pjx0ZXh0IHg9IjMwIiB5PSI0MjAiPldFVEg6IDIwMDAwMDAwMDAwMDAwMDA8L3RleHQ+PHRleHQgeD0iMzAiIHk9IjQ0MCI+VVNEQzogMjIwMDAwMDA8L3RleHQ+PC9zdmc+","imageMimeType":"image/svg+xml"},"description":null,"rarityScore":12,"rarityRank":1,"supply":"1","remainingSupply":"1","media":null,"isFlagged":false,"isSpam":false,"isNsfw":false,"metadataDisabled":false,"lastFlagUpdate":null,"lastFlagChange":null,"collection":{"id":"0x5cc0c202a402cf02be73387c20867158db8bb235","name":"Deswap OptionsNFT","slug":"deswap-optionsnft-5","symbol":"WETH/USDC","imageUrl":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZHV%2BYGB%2Bazr1wWFwCrOqAwI0RhTqtbnEJloVxSREk%2BHVI4bbmDWwvgnb0GEA22Fty4BN82yCUpirVBcXFn8h3mEoOqfYCvrn2HI1HZLtXS1uwPHGfqwhEGvPwNRuQ0lxY%3D?width=250","isSpam":false,"isNsfw":false,"metadataDisabled":false,"openseaVerificationStatus":"not_requested","floorAskPrice":null,"royaltiesBps":50,"royalties":[{"bps":50,"recipient":"0x720ac46fdb6da28fa751bc60afb8094290c2b4b7"}]},"lastAppraisalValue":null},"ownership":{"tokenCount":"1","onSaleCount":"0","floorAsk":{"id":null,"price":null,"maker":null,"kind":null,"validFrom":null,"validUntil":null,"source":null},"acquiredAt":"2024-01-13T13:51:12.000Z"}},{"token":{"chainId":11155111,"contract":"0x4b8d9d541a5b60cc1dd90d2fc870d28c965ea2fb","tokenId":"0","kind":"erc721","name":"#Deswap WETH/TUSDC #0","image":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZtjLo0HFV44QTf2OjNRDzovN2XAWg8PqXeTwyYp1BCgoHH2xaCoz1RaRWvTfwK4%2Fxwun5Z6VV7fpW5BLrgfByOe%2FXcQKPnfFsui4acBLa2ZGtm%2FiuIObUqJ%2B2v8gZaSuI%3D.svg?width=512","imageSmall":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZtjLo0HFV44QTf2OjNRDzovN2XAWg8PqXeTwyYp1BCgoHH2xaCoz1RaRWvTfwK4%2Fxwun5Z6VV7fpW5BLrgfByOe%2FXcQKPnfFsui4acBLa2ZGtm%2FiuIObUqJ%2B2v8gZaSuI%3D.svg?width=250","imageLarge":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZtjLo0HFV44QTf2OjNRDzovN2XAWg8PqXeTwyYp1BCgoHH2xaCoz1RaRWvTfwK4%2Fxwun5Z6VV7fpW5BLrgfByOe%2FXcQKPnfFsui4acBLa2ZGtm%2FiuIObUqJ%2B2v8gZaSuI%3D.svg?width=1000","metadata":{"imageOriginal":"data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIiBmb250LXNpemU9IjE0Ij7wn5OIIFdFVEgvVFVTREM8L3RleHQ+PHRleHQgeD0iNzAiIHk9IjI0MCIgc3R5bGU9ImZvbnQtc2l6ZToxMDBweCI+8J+MuzwvdGV4dD48dGV4dCB4PSIzMCIgeT0iNDAwIj5JRDogMDwvdGV4dD48dGV4dCB4PSIzMCIgeT0iNDIwIj5XRVRIOiAyMDAwMDAwMDAwMDAwMDAwPC90ZXh0Pjx0ZXh0IHg9IjMwIiB5PSI0NDAiPlRVU0RDOiAyMzEwMDAwMDA8L3RleHQ+PC9zdmc+","imageMimeType":"image/svg+xml"},"description":null,"rarityScore":12,"rarityRank":1,"supply":"1","remainingSupply":"1","media":null,"isFlagged":false,"isSpam":false,"isNsfw":false,"metadataDisabled":false,"lastFlagUpdate":null,"lastFlagChange":null,"collection":{"id":"0x4b8d9d541a5b60cc1dd90d2fc870d28c965ea2fb","name":"Deswap OptionsNFT","slug":"deswap-optionsnft-4","symbol":"WETH/TUSDC","imageUrl":"https://img.reservoir.tools/images/v2/sepolia/7%2FrdF%2Fe%2F0iXY8HduhRCoIehkmFeXPeOQQFbbmIPfjCZtjLo0HFV44QTf2OjNRDzovN2XAWg8PqXeTwyYp1BCgoHH2xaCoz1RaRWvTfwK4%2Fxwun5Z6VV7fpW5BLrgfByOe%2FXcQKPnfFsui4acBLa2ZGGY4tKrZLPcg%2FKjuS9k4qY%3D?width=250","isSpam":false,"isNsfw":false,"metadataDisabled":false,"openseaVerificationStatus":"not_requested","floorAskPrice":null,"royaltiesBps":50,"royalties":[{"bps":50,"recipient":"0x720ac46fdb6da28fa751bc60afb8094290c2b4b7"}]},"lastAppraisalValue":null},"ownership":{"tokenCount":"1","onSaleCount":"0","floorAsk":{"id":null,"price":null,"maker":null,"kind":null,"validFrom":null,"validUntil":null,"source":null},"acquiredAt":"2024-01-12T11:56:12.000Z"}}],"continuation":null}
""";
}
