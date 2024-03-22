namespace DeswapApp;

public class OptionsContract
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string QuoteAssetName { get; set; }

    public required string QuoteAssetAddress { get; set; }

    public required int QuoteAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    // 在中心化交易所的符号，用来获取参考价格
    public string? OkxTickSymbol { get; set; }

    public string? OkxOptUly { get; set; }

    public required Network Network { get; set; }

    // Like: https://sepolia.etherscan.io/token/0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9?a=0x7e727520B29773e7F23a8665649197aAf064CeF1
    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }

    public string Name => "Options NFT " + BaseAssetName + "/" + QuoteAssetName;
}

public static class OptionsContracts
{
    public static readonly IList<OptionsContract> Inner = [
        // WETH/TUSDC sepolia
        new OptionsContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x2556dfFB5a692cfBfABdD5DC2FB71A7502099c1F",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        // WETH/USDC sepolia
        new OptionsContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0x1c7D4B196Cb0C7B01d743Fbc6116a902379C7238",
            QuoteAssetDecimals = 6,
            NftAddress = "0x17d7A375E32D986d6e2A367293abD07D7Bac7f94",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        // mumbai WETH/TUSDC
        new OptionsContract{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5",
            QuoteAssetDecimals = 6,
            NftAddress = "0x2556dfFB5a692cfBfABdD5DC2FB71A7502099c1F",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
        // mumbai WETH/USDC
        new OptionsContract{
            BaseAssetName = "WMATIC",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0x9999f7Fea5938fD3b1E26A12c3f2fb024e194f97",
            QuoteAssetDecimals = 6,
            NftAddress = "0x17d7A375E32D986d6e2A367293abD07D7Bac7f94",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },

        // neon devnet
        new OptionsContract{
            BaseAssetName = "WNEON",
            BaseAssetAddress = "0x11adC2d986E334137b9ad0a0F290771F31e9517F",
            BaseAssetDecimals = 18,
            QuoteAssetName = "USDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0x512E48836Cd42F3eB6f50CEd9ffD81E0a7F15103",
            QuoteAssetDecimals = 6,
            NftAddress = "0xC04DD964ed36c0e4796F53A7168393ED4Fc38FF6",
            Network = SupportedNetworks.GetNetwork(245022926)!,
        },

        // manta pacific sepolia
        new OptionsContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x199d1a27684106dC3Deb673115fc0fc9cf6af287",
            BaseAssetDecimals = 18,
            QuoteAssetName = "TUSDC",
            OkxTickSymbol = null,
            QuoteAssetAddress = "0x912D36F448b9D9456736aB04Ce041767a8e827a1",
            QuoteAssetDecimals = 6,
            NftAddress = "0xf8fcC87D007004A156513ef0B5c8B9657ba1831c",
            Network = SupportedNetworks.GetNetwork(3441006)!,
        },

        // aurora testnet
        // new OptionsContract{
        //     BaseAssetName = "WETH",
        //     BaseAssetAddress = "0x8886E7A8883e9A40b30Bd4E16e0e25C2C3f29Cd8",
        //     BaseAssetDecimals = 18,
        //     QuoteAssetName = "USDC",
        //     OkxTickSymbol = null,
        //     QuoteAssetAddress = "0x3dcB6AdF46E4d854E94719b6ed9cfab6939cC1Cb",
        //     QuoteAssetDecimals = 6,
        //     NftAddress = "0xf8fcC87D007004A156513ef0B5c8B9657ba1831c",
        //     Network = SupportedNetworks.GetNetwork(1313161555)!,
        // },
    ];

    public static IList<OptionsContract> FilterByChainId(long chainId)
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
            NftAddress = "0x59a84350f5b9071787623f3E0AC23c5768c98BA2",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new LotteryContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0x59a84350f5b9071787623f3E0AC23c5768c98BA2",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
        new LotteryContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0xD909178CC99d318e4D46e7E66a972955859670E1",
            BaseAssetDecimals = 18,
            NftAddress = "0xC04DD964ed36c0e4796F53A7168393ED4Fc38FF6",
            Network = SupportedNetworks.GetNetwork(1287)!,
        },
        new LotteryContract{
            BaseAssetName = "WNEON",
            BaseAssetAddress = "0x11adC2d986E334137b9ad0a0F290771F31e9517F",
            BaseAssetDecimals = 18,
            NftAddress = "0x7374FE94e34c209616cEc0610212DE13151D222f",
            Network = SupportedNetworks.GetNetwork(245022926)!,
        },
        new LotteryContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x199d1a27684106dC3Deb673115fc0fc9cf6af287",
            BaseAssetDecimals = 18,
            NftAddress = "0xF30AB0A2378d5Dc1436F81c72D2784748A863938",
            Network = SupportedNetworks.GetNetwork(3441006)!,
        },
        // new LotteryContract{
        //     BaseAssetName = "WETH",
        //     BaseAssetAddress = "0x8886E7A8883e9A40b30Bd4E16e0e25C2C3f29Cd8",
        //     BaseAssetDecimals = 18,
        //     NftAddress = "0xC04DD964ed36c0e4796F53A7168393ED4Fc38FF6",
        //     Network = SupportedNetworks.GetNetwork(1313161555)!,
        // },
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
            NftAddress = "0x9fB56828d10927cf8DbAe9585586C79E14365ecC",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new RedEnvelopeContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0x9fB56828d10927cf8DbAe9585586C79E14365ecC",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
        new RedEnvelopeContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0xD909178CC99d318e4D46e7E66a972955859670E1",
            BaseAssetDecimals = 18,
            NftAddress = "0x7374FE94e34c209616cEc0610212DE13151D222f",
            Network = SupportedNetworks.GetNetwork(1287)!,
        },
        new RedEnvelopeContract{
            BaseAssetName = "WNEON",
            BaseAssetAddress = "0x11adC2d986E334137b9ad0a0F290771F31e9517F",
            BaseAssetDecimals = 18,
            NftAddress = "0xbDcE3D50aB559474cDA1160253037FD02b97Df9b",
            Network = SupportedNetworks.GetNetwork(245022926)!,
        },
        new RedEnvelopeContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x199d1a27684106dC3Deb673115fc0fc9cf6af287",
            BaseAssetDecimals = 18,
            NftAddress = "0xD08D4d2046C234D32f4abf889E9CB93bCB756Dc5",
            Network = SupportedNetworks.GetNetwork(3441006)!,
        },
        // new RedEnvelopeContract{
        //     BaseAssetName = "WETH",
        //     BaseAssetAddress = "0x8886E7A8883e9A40b30Bd4E16e0e25C2C3f29Cd8",
        //     BaseAssetDecimals = 18,
        //     NftAddress = "0x7374FE94e34c209616cEc0610212DE13151D222f",
        //     Network = SupportedNetworks.GetNetwork(1313161555)!,
        // },
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

    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }
}

public static class RouletteContracts
{
    public static readonly IList<RouletteContract> Inner = [
        new RouletteContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            NftAddress = "0x36aeEAe86F4af54a7b9249C40E90D88aAC8431E5",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new RouletteContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0xbDcE3D50aB559474cDA1160253037FD02b97Df9b",
            Network = SupportedNetworks.GetNetwork(1287)!,
        },
        new RouletteContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0xD909178CC99d318e4D46e7E66a972955859670E1",
            BaseAssetDecimals = 18,
            NftAddress = "0x36aeEAe86F4af54a7b9249C40E90D88aAC8431E5",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
        new RouletteContract{
            BaseAssetName = "WNEON",
            BaseAssetAddress = "0x11adC2d986E334137b9ad0a0F290771F31e9517F",
            BaseAssetDecimals = 18,
            NftAddress = "0x912D36F448b9D9456736aB04Ce041767a8e827a1",
            Network = SupportedNetworks.GetNetwork(245022926)!,
        },
        new RouletteContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x199d1a27684106dC3Deb673115fc0fc9cf6af287",
            BaseAssetDecimals = 18,
            NftAddress = "0xe33D3D26d5C75bFFb0170d1F06A2c442e643F65E",
            Network = SupportedNetworks.GetNetwork(3441006)!,
        },
        // new RouletteContract{
        //     BaseAssetName = "WETH",
        //     BaseAssetAddress = "0x8886E7A8883e9A40b30Bd4E16e0e25C2C3f29Cd8",
        //     BaseAssetDecimals = 18,
        //     NftAddress = "0xbDcE3D50aB559474cDA1160253037FD02b97Df9b",
        //     Network = SupportedNetworks.GetNetwork(1313161555)!,
        // },
    ];
}

public class RouletteContract
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    public required Network Network { get; set; }

    public string Name => "Roulette NFT " + BaseAssetName;

    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return Network.EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }
}

public static class BlackJackContracts
{
    public static readonly IList<BlackJackContract> Inner = [
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            NftAddress = "0xCEe20496BAa9C4F41Ca217f6fDb89213c62676bC",
            Network = SupportedNetworks.GetNetwork(11155111)!,
        },
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0xCEe20496BAa9C4F41Ca217f6fDb89213c62676bC",
            Network = SupportedNetworks.GetNetwork(80001)!,
        },
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0xD909178CC99d318e4D46e7E66a972955859670E1",
            BaseAssetDecimals = 18,
            NftAddress = "0x912D36F448b9D9456736aB04Ce041767a8e827a1",
            Network = SupportedNetworks.GetNetwork(1287)!,
        }
    ];
}

public class BlackJackContract
{
    public required string BaseAssetName { get; set; }

    public required string BaseAssetAddress { get; set; }

    public required int BaseAssetDecimals { get; set; }

    public required string NftAddress { get; set; }

    public required Network Network { get; set; }

    public string Name => "BlackJack NFT " + BaseAssetName;

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

    public string? CovalentApiHost { get; set; }

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
            IsTestNet=true,
        },
        new Network{
            Name = "Pylogon Mumbai",
            ChainId=80001,
            EtherscanHost="https://mumbai.polygonscan.com",
            OpenseaHost="https://testnets.opensea.io/assets/mumbai",
            OpenseaApiHost="https://testnets-api.opensea.io/api/v2/chain/mumbai",
            ReservoirHost="https://api-mumbai.reservoir.tools",
            Logo="polygon_logo.svg",
            IsTestNet=true,
        },
        new Network{
            Name = "MoonBase Alpha",
            ChainId=1287,
            EtherscanHost="https://moonbase.moonscan.io",
            OpenseaHost="",
            OpenseaApiHost="",
            ReservoirHost="",
            Logo="moonbase_alpha_logo.svg",
            IsTestNet=true,
            CovalentApiHost = "https://api.covalenthq.com/v1/moonbeam-moonbase-alpha",
        },
        new Network{
            Name = "Neon dev",
            ChainId=245022926,
            EtherscanHost="https://devnet.neonscan.org",
            OpenseaHost="",
            OpenseaApiHost="",
            ReservoirHost="",
            Logo="neon_logo.svg",
            IsTestNet=true,
        },
        new Network{
            Name = "Manta Pacific Sepolia",
            ChainId=3441006,
            EtherscanHost="https://pacific-explorer.sepolia-testnet.manta.network",
            OpenseaHost="",
            OpenseaApiHost="",
            ReservoirHost="",
            Logo="manta_pacific_logo.svg",
            IsTestNet=true,
            CovalentApiHost = "https://api.covalenthq.com/v1/manta-testnet",
        },
        // new Network{
        //     Name = "Aurora Testnet",
        //     ChainId=1313161555,
        //     EtherscanHost="https://explorer.testnet.aurora.dev",
        //     OpenseaHost="",
        //     OpenseaApiHost="",
        //     ReservoirHost="",
        //     Logo="aurora_logo.svg",
        //     IsTestNet=true,
        //     CovalentApiHost = "https://api.covalenthq.com/v1/aurora-testnet",
        // },
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
