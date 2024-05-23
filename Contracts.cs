namespace DeswapApp;

public record ERC20Contract(string Name, string Address, int Decimals);
public record NFTContract(string Name, string Address, string DefaultImage);

public record NetworkCore
{
    public required string Name { get; set; }

    public required string InnerName { get; set; }

    public string RpcUrl { get; set; } = "";

    public long ChainId { get; set; }

    public string? EtherscanHost { get; set; }

    public string? BlockscoutHost { get; set; }

    // opensea 主界面对应的host
    public string? OpenseaHost { get; set; }

    // opensea api 对应的host，很多链不支持opensea，应该填null
    public string? OpenseaApiHost { get; set; }

    public string? CovalentApiHost { get; set; }

    public required string Logo { get; set; } // svg文件，保存在 wwwroot/img 下

    public bool IsTestNet { get; set; }

    public string BuildListUrl(string contractAddress, long tokenId)
    {
        if (string.IsNullOrEmpty(OpenseaHost))
        {
            return "";
        }
        return $"{OpenseaHost}/{contractAddress}/{tokenId}/sell";
    }

    public string BuildDetailUrl(NFTContract contract, long tokenId)
    {
        return $"app/{InnerName}/{contract.Name.ToLower()}/{contract.Address}/{tokenId}";
    }

    public string BuildNftUrl(string contractAddress, long tokenId)
    {
        if (string.IsNullOrEmpty(OpenseaHost))
        {
            return "";
        }
        return $"{OpenseaHost}/{contractAddress}/{tokenId}";
    }

    public string BuildNftCollectionUrl(string contractAddress)
    {
        if (string.IsNullOrEmpty(OpenseaHost))
        {
            return "";
        }
        return $"{OpenseaHost}/{contractAddress}";
    }

    public string GetEtherScanTokenBalanceUrl(string contractAddress, string userAddress)
    {
        return EtherscanHost + "/token/" + contractAddress + "?a=" + userAddress;
    }

    public string GetEtherScanTokenUrl(string contractAddress)
    {
        return EtherscanHost + "/token/" + contractAddress;
    }
}

public static class NetworkConfig
{
    public static NetworkCore Local { get; } = new()
    {
        Name = "Local Testnet",
        InnerName = "local",
        ChainId = 31337,
        Logo = "ethereum_logo.svg",
        IsTestNet = true,
        RpcUrl = "http://127.0.0.1:8545",
    };

    public static NetworkCore EthereumSepolia { get; } = new()
    {
        Name = "Sepolia",
        InnerName = "sepolia",
        ChainId = 11155111,
        EtherscanHost = "https://sepolia.etherscan.io",
        BlockscoutHost = "https://eth-sepolia.blockscout.com",
        OpenseaHost = "https://testnets.opensea.io/assets/sepolia",
        OpenseaApiHost = "https://testnets-api.opensea.io/api/v2/chain/sepolia",
        Logo = "ethereum_logo.svg",
        IsTestNet = true,
        RpcUrl = "https://rpc.ankr.com/eth_sepolia",
    };

    public static NetworkCore MoonBaseAlpha { get; } = new()
    {
        Name = "MoonBase Alpha",
        InnerName = "moonbase_alpha",
        ChainId = 1287,
        EtherscanHost = "https://moonbase.moonscan.io",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "moonbase_alpha_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/moonbeam-moonbase-alpha",
        RpcUrl = "https://rpc.api.moonbase.moonbeam.network",
    };

    public static NetworkCore SolanaNeonDev { get; } = new()
    {
        Name = "Neon dev",
        InnerName = "neondev",
        ChainId = 245022926,
        EtherscanHost = "https://devnet.neonscan.org",
        BlockscoutHost = "https://neon-devnet.blockscout.com",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "neon_logo.svg",
        IsTestNet = true,
        RpcUrl = "https://devnet.neonevm.org",
    };

    public static NetworkCore MantaPacificSepolia { get; } = new()
    {
        Name = "Manta Pacific Sepolia", // nft api 501 not implemented
        InnerName = "manta_pacific_sepolia",
        ChainId = 3441006,
        EtherscanHost = null,
        BlockscoutHost = "https://pacific-explorer.sepolia-testnet.manta.network",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "manta_pacific_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/manta-testnet",
        RpcUrl = "https://pacific-rpc.sepolia-testnet.manta.network/http",
    };

    public static NetworkCore NearAuroraTestnet { get; } = new()
    {
        Name = "Aurora Testnet",
        InnerName = "aurora_testnet",
        ChainId = 1313161555,
        EtherscanHost = null,
        BlockscoutHost = "https://explorer.testnet.aurora.dev",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "aurora_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/aurora-testnet",
        RpcUrl = "https://testnet.aurora.dev",
    };

    public static NetworkCore MantleSepolia { get; } = new()
    {
        Name = "Mantle Sepolia", // nft api 501 not implemented
        InnerName = "mantle_sepolia",
        ChainId = 5003,
        EtherscanHost = null,
        BlockscoutHost = "https://explorer.sepolia.mantle.xyz",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "mantle_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/mantle-testnet",
        RpcUrl = "https://rpc.sepolia.mantle.xyz",
    };

    public static NetworkCore ScrollSepolia { get; } = new()
    {
        Name = "Scroll Sepolia", // nft api 501 not implemented
        InnerName = "scroll_sepolia",
        ChainId = 534351,
        EtherscanHost = "https://sepolia.scrollscan.com",
        OpenseaHost = null,
        OpenseaApiHost = null,
        Logo = "scroll_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/scroll-sepolia-testnet",
        RpcUrl = "https://sepolia-rpc.scroll.io",
    };

    public static readonly IList<NetworkCore> Inner = [
        EthereumSepolia,
        MoonBaseAlpha,
        // SolanaNeonDev,
        MantaPacificSepolia,
        // NearAuroraTestnet,
        MantleSepolia,
        // ScrollSepolia, // scroll doesn't support difficulty/prevrandao, doesn't support
    ];

    public static NetworkCore? GetNetwork(long chainId)
    {
        var res = Inner.FirstOrDefault(item => item.ChainId == chainId);
        return res;
    }

    public static NetworkCore? GetNetworkByName(string name)
    {
        return Inner.FirstOrDefault(item => item.InnerName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
}

public static class ContractConfig
{
    public static readonly Dictionary<NetworkCore, ERC20Contract> Weth = new() {
        { NetworkConfig.EthereumSepolia,     new ERC20Contract("WETH", "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9", 18) },
        { NetworkConfig.MoonBaseAlpha,       new ERC20Contract("WETH", "0xD909178CC99d318e4D46e7E66a972955859670E1", 18) },
        { NetworkConfig.MantaPacificSepolia, new ERC20Contract("WETH", "0x199d1a27684106dC3Deb673115fc0fc9cf6af287", 18) },
        { NetworkConfig.MantleSepolia,       new ERC20Contract("WETH", "0x17f711A85D359cBF0224a017d8e3dd7A29c9932E", 18) },
    };

    public static readonly Dictionary<NetworkCore, ERC20Contract> Tusdc = new() {
        { NetworkConfig.Local,     new ERC20Contract("TUSDC", "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5", 6) },
        { NetworkConfig.EthereumSepolia,     new ERC20Contract("TUSDC", "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5", 6) },
        { NetworkConfig.MoonBaseAlpha,       new ERC20Contract("TUSDC", "0x0EBB63025caE604fd9230A2fdc811a84394A7861", 6) },
        { NetworkConfig.MantaPacificSepolia, new ERC20Contract("TUSDC", "0x7EDaec7d9db23D4Af62eA2FF71F7104279347f14", 6) },
        { NetworkConfig.MantleSepolia,       new ERC20Contract("TUSDC", "0x238285119Ad0842051B4a46A9428139d30869B55", 6) },
    };

    public static readonly IList<Dictionary<NetworkCore, ERC20Contract>> KnownERC20Contracts = [
        Weth,
        Tusdc,
    ];

    public static IList<ERC20Contract> GetERC20Contracts(NetworkCore? network)
    {
        var res = new List<ERC20Contract>();
        if (network is null)
        {
            return res;
        }
        foreach (var item in KnownERC20Contracts)
        {
            if (item.TryGetValue(network, out var contract))
            {
                res.Add(contract);
            }
        }
        return res;
    }

    private static string DefaultImage = "data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjMDAwMDAwIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCAzMiAzMiIgaWQ9Imljb24iIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHN0eWxlPi5jbHMtMXtmaWxsOm5vbmU7fTwvc3R5bGU+PC9kZWZzPjx0aXRsZT5uby1pbWFnZTwvdGl0bGU+PHBhdGggZD0iTTMwLDMuNDE0MSwyOC41ODU5LDIsMiwyOC41ODU5LDMuNDE0MSwzMGwyLTJIMjZhMi4wMDI3LDIuMDAyNywwLDAsMCwyLTJWNS40MTQxWk0yNiwyNkg3LjQxNDFsNy43OTI5LTcuNzkzLDIuMzc4OCwyLjM3ODdhMiwyLDAsMCwwLDIuODI4NCwwTDIyLDE5bDQsMy45OTczWm0wLTUuODMxOC0yLjU4NTgtMi41ODU5YTIsMiwwLDAsMC0yLjgyODQsMEwxOSwxOS4xNjgybC0yLjM3Ny0yLjM3NzFMMjYsNy40MTQxWiIvPjxwYXRoIGQ9Ik02LDIyVjE5bDUtNC45OTY2LDEuMzczMywxLjM3MzMsMS40MTU5LTEuNDE2LTEuMzc1LTEuMzc1YTIsMiwwLDAsMC0yLjgyODQsMEw2LDE2LjE3MTZWNkgyMlY0SDZBMi4wMDIsMi4wMDIsMCwwLDAsNCw2VjIyWiIvPjxyZWN0IGlkPSJfVHJhbnNwYXJlbnRfUmVjdGFuZ2xlXyIgZGF0YS1uYW1lPSImbHQ7VHJhbnNwYXJlbnQgUmVjdGFuZ2xlJmd0OyIgY2xhc3M9ImNscy0xIiB3aWR0aD0iMzIiIGhlaWdodD0iMzIiLz48L3N2Zz4K";

    private static readonly string RouletteDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5Sb3VsZXR0ZTwvdGV4dD48dGV4dCB4PSI3MCIgeT0iMjQwIiBzdHlsZT0iZm9udC1zaXplOjEwMHB4Ij7wn46xPC90ZXh0Pjwvc3ZnPgo=";

    private static readonly string SicboDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5TaWNCbzwvdGV4dD48dGV4dCB4PSI3MCIgeT0iMjQwIiBzdHlsZT0iZm9udC1zaXplOjEwMHB4Ij7wn46yPC90ZXh0Pjwvc3ZnPgo=";

    private static readonly string RedEnvelopeDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5SZWQgRW52ZWxvcGU8L3RleHQ+PHRleHQgeD0iNzAiIHk9IjI0MCIgc3R5bGU9ImZvbnQtc2l6ZToxMDBweCI+8J+npzwvdGV4dD48L3N2Zz4K";

    private static readonly string LotteryDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5Mb3R0ZXJ5PC90ZXh0Pjx0ZXh0IHg9IjcwIiB5PSIyNDAiIHN0eWxlPSJmb250LXNpemU6MTAwcHgiPvCfjp/vuI88L3RleHQ+PC9zdmc+Cg==";

    private static readonly string BarterDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iNDAiIHk9IjcwIiBmb250LXNpemU9IjE0Ij5CYXJ0ZXI8L3RleHQ+PHRleHQgeD0iNzAiIHk9IjI0MCIgc3R5bGU9ImZvbnQtc2l6ZToxMDBweCI+8J+MuzwvdGV4dD48L3N2Zz4K";

    public static readonly Dictionary<NetworkCore, NFTContract> Barter = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Barter", "0x677dfb01d6F3C30EA1Ce4497775AB94EAB699981", BarterDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Barter", "0xb9afd20d69CEEdC249E32649ef10015af6C05c3F", BarterDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Barter", "0xBAA68A83fa8a0eb7c1144EE275350c3C4F5B61b0", BarterDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Barter", "0x5A4cA8E3Cd655F5Ec72a353A40aD82159A37FC58", BarterDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Lottery = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Lottery", "0xF1a162C2d43ea2d1f2Af764cC6E564EaC2A7dC01", LotteryDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Lottery", "0x5cAE158176D8c30F7038b127Da9FC845B916a99A", LotteryDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Lottery", "0x86cF9996b7a8aF46E74A9FBbd28b149a2f7F33e2", LotteryDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Lottery", "0xD40D28361Ef1c9FB6E672E0905548CAdD4d70AA1", LotteryDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> RedEnvelope = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("RedEnvelope", "0x629391548a7D4f9848e2F9377aFFC5540F4158A7", RedEnvelopeDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("RedEnvelope", "0xE7cd7918C6f402cf29779DAC7F4Ba582F7448016", RedEnvelopeDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("RedEnvelope", "0xCeA52909295CAABEFF8Dc7820bDF9Bf8403683D9", RedEnvelopeDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("RedEnvelope", "0x8106ac53d21B3180471a19D359d610A19c15aD9B", RedEnvelopeDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Roulette = new() {
        { NetworkConfig.Local,               new NFTContract("Roulette", "0x9EDcDf3cac168bA983bD3b0321B5337BD265Afc8", RouletteDefaultImage) },
        { NetworkConfig.EthereumSepolia,     new NFTContract("Roulette", "0x9EDcDf3cac168bA983bD3b0321B5337BD265Afc8", RouletteDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Roulette", "0x238285119Ad0842051B4a46A9428139d30869B55", RouletteDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Roulette", "0x37d0d06a35d9538AC16C02c50911Fc0Fc3298a71", RouletteDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Roulette", "0x8C5D92347289638A72175fea9be8617F0491d36F", RouletteDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Sicbo = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Sicbo", "0xd5F79e329bf07c8beF48F32246752Ba018CAFa1A", SicboDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Sicbo", "0x5494bC19Ce5AA656437Db2D6d151DCf47b8b9C6F", SicboDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Sicbo", "0x47CEA152577A40EB97Ef02cF6E64b5645fB1D748", SicboDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Sicbo", "0xAC03A3475F3E76169aF6Ce11D21490a4389FE4aB", SicboDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Vote = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Vote", "0x3b8A315a925dA3bc904304B11E66618D1A167954", DefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Vote", "0x415f9bcD4747BB9F20beCadD3e2E70670d8bC3C3", DefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Vote", "0x238285119Ad0842051B4a46A9428139d30869B55", DefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Vote", "0xC6024186e356eaa74d564475127244a96C86eA3D", DefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Writing = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Writing", "0x6C061bF5223fEc8d6E990bdd4b805F0E08604bF9", DefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Writing", "0xf6F0F9dE6b0D4e10A8F6423124Df47E4957d7205", DefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Writing", "0x720B8eDf9f9507ae0531e31b09793f291932548c", DefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Writing", "0xA32dcae4b15419c35763ea8D29AeFCe85CaB4A85", DefaultImage) },
    };

    public static readonly IList<Dictionary<NetworkCore, NFTContract>> KnownNFTContracts = [
        Barter,
        Lottery,
        RedEnvelope,
        Roulette,
        Sicbo,
        Vote,
        Writing,
    ];

    public static NFTContract? GetNFTContract(NetworkCore? network, string contractAddress)
    {
        if (network == null)
        {
            return null;
        }
        foreach (var item in KnownNFTContracts)
        {
            if (item.TryGetValue(network, out var contract) && contract.Address.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase))
            {
                return contract;
            }
        }

        return null;
    }

    public static IList<string> GetNFTContracts(NetworkCore? network)
    {
        if (network == null)
        {
            return [];
        }
        var res = new List<string>();
        foreach (var item in KnownNFTContracts)
        {
            if (item.TryGetValue(network, out var contract))
            {
                res.Add(contract.Address);
            }
        }

        return res;
    }
}
