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
        return $"/{InnerName}/{contract.Name.ToLower()}/{contract.Address}/{tokenId}";
    }

    public string BuildContractTokensUrl(NFTContract contract)
    {
        return $"/{InnerName}/{contract.Address}";
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

    public static readonly NetworkCore[] Inner = [
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

    public static readonly Dictionary<NetworkCore, ERC20Contract>[] KnownERC20Contracts = [
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

    private static readonly string DefaultImage = "data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjMDAwMDAwIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCAzMiAzMiIgaWQ9Imljb24iIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHN0eWxlPi5jbHMtMXtmaWxsOm5vbmU7fTwvc3R5bGU+PC9kZWZzPjx0aXRsZT5uby1pbWFnZTwvdGl0bGU+PHBhdGggZD0iTTMwLDMuNDE0MSwyOC41ODU5LDIsMiwyOC41ODU5LDMuNDE0MSwzMGwyLTJIMjZhMi4wMDI3LDIuMDAyNywwLDAsMCwyLTJWNS40MTQxWk0yNiwyNkg3LjQxNDFsNy43OTI5LTcuNzkzLDIuMzc4OCwyLjM3ODdhMiwyLDAsMCwwLDIuODI4NCwwTDIyLDE5bDQsMy45OTczWm0wLTUuODMxOC0yLjU4NTgtMi41ODU5YTIsMiwwLDAsMC0yLjgyODQsMEwxOSwxOS4xNjgybC0yLjM3Ny0yLjM3NzFMMjYsNy40MTQxWiIvPjxwYXRoIGQ9Ik02LDIyVjE5bDUtNC45OTY2LDEuMzczMywxLjM3MzMsMS40MTU5LTEuNDE2LTEuMzc1LTEuMzc1YTIsMiwwLDAsMC0yLjgyODQsMEw2LDE2LjE3MTZWNkgyMlY0SDZBMi4wMDIsMi4wMDIsMCwwLDAsNCw2VjIyWiIvPjxyZWN0IGlkPSJfVHJhbnNwYXJlbnRfUmVjdGFuZ2xlXyIgZGF0YS1uYW1lPSImbHQ7VHJhbnNwYXJlbnQgUmVjdGFuZ2xlJmd0OyIgY2xhc3M9ImNscy0xIiB3aWR0aD0iMzIiIGhlaWdodD0iMzIiLz48L3N2Zz4K";

    private static readonly string RouletteDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5Sb3VsZXR0ZTwvdGV4dD48dGV4dCB4PSI3MCIgeT0iMjQwIiBzdHlsZT0iZm9udC1zaXplOjEwMHB4Ij7wn46xPC90ZXh0Pjwvc3ZnPgo=";

    private static readonly string SicboDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5TaWNCbzwvdGV4dD48dGV4dCB4PSI3MCIgeT0iMjQwIiBzdHlsZT0iZm9udC1zaXplOjEwMHB4Ij7wn46yPC90ZXh0Pjwvc3ZnPgo=";

    private static readonly string RedEnvelopeDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5SZWQgRW52ZWxvcGU8L3RleHQ+PHRleHQgeD0iNzAiIHk9IjI0MCIgc3R5bGU9ImZvbnQtc2l6ZToxMDBweCI+8J+npzwvdGV4dD48L3N2Zz4K";

    private static readonly string LotteryDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iMzAiIHk9IjcwIj5Mb3R0ZXJ5PC90ZXh0Pjx0ZXh0IHg9IjcwIiB5PSIyNDAiIHN0eWxlPSJmb250LXNpemU6MTAwcHgiPvCfjp/vuI88L3RleHQ+PC9zdmc+Cg==";

    private static readonly string BarterDefaultImage = "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgdmlld0JveD0iMCAwIDI5MCA1MDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHN0eWxlPnRleHR7Zm9udC1zaXplOjEycHg7ZmlsbDojZmZmfTwvc3R5bGU+PGNsaXBQYXRoIGlkPSJjb3JuZXJzIj48cmVjdCB3aWR0aD0iMjkwIiBoZWlnaHQ9IjUwMCIgcng9IjQyIiByeT0iNDIiLz48L2NsaXBQYXRoPjxnIGNsaXAtcGF0aD0idXJsKCNjb3JuZXJzKSI+PHBhdGggZD0iTTAgMGgyOTB2NTAwSDB6Ii8+PC9nPjx0ZXh0IGNsYXNzPSJoMSIgeD0iNDAiIHk9IjcwIiBmb250LXNpemU9IjE0Ij5CYXJ0ZXI8L3RleHQ+PHRleHQgeD0iNzAiIHk9IjI0MCIgc3R5bGU9ImZvbnQtc2l6ZToxMDBweCI+8J+MuzwvdGV4dD48L3N2Zz4K";

    public static readonly Dictionary<NetworkCore, NFTContract> Barter = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Barter", "0xdfd52faeb40dea6e7271375e7e8554af22debb80", BarterDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Barter", "0x0069801981cecb1bb6b2cc56d12aca2946300617", BarterDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Barter", "0xc8b067ed68d04cdcecca0cba98a0abd38e1c74a9", BarterDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Barter", "0x4af8106d145b20f7abc17d3cc9af02be66e942a2", BarterDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Lottery = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Lottery", "0x6a3297a6f0760776ac38c7fcbe82a83d8c44eec8", LotteryDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Lottery", "0xc8b067ed68d04cdcecca0cba98a0abd38e1c74a9", LotteryDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Lottery", "0xd40d28361ef1c9fb6e672e0905548cadd4d70aa1", LotteryDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Lottery", "0x0190883430ccb9ce35ab76c9e14c04842e323183", LotteryDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> RedEnvelope = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("RedEnvelope", "0x7daeb128f63b77f655e626a818886b0b98f909f4", RedEnvelopeDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("RedEnvelope", "0xd40d28361ef1c9fb6e672e0905548cadd4d70aa1", RedEnvelopeDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("RedEnvelope", "0x8106ac53d21b3180471a19d359d610a19c15ad9b", RedEnvelopeDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("RedEnvelope", "0x81736a220a681da9a944ee916b94363e2bf1b895", RedEnvelopeDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Roulette = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Roulette", "0x4e9c39f01e4b8deee89415c8d3a49f1da7d63042", RouletteDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Roulette", "0x8106ac53d21b3180471a19d359d610a19c15ad9b", RouletteDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Roulette", "0x0be3d90fe9555e14d61004d626497ee9b41b402f", RouletteDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Roulette", "0x993b85354689c27acceb5d6e4f2a1c16674f3ca6", RouletteDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Sicbo = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Sicbo", "0x5b16f86ba0d7edcbe678bcef579fee27abaa4f13", SicboDefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Sicbo", "0x0be3d90fe9555e14d61004d626497ee9b41b402f", SicboDefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Sicbo", "0x5a4ca8e3cd655f5ec72a353a40ad82159a37fc58", SicboDefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Sicbo", "0x75b2f67384a66173c4b1fe167512b7a522531b34", SicboDefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Vote = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Vote", "0x7979ebe399c2faea4e01583784b34d6e1f1dfdcd", DefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Vote", "0x5a4ca8e3cd655f5ec72a353a40ad82159a37fc58", DefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Vote", "0xc54a5127d9b434debd5b3e5613cfa1c8478d7dda", DefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Vote", "0x693cd34c5eec26bacb80f4b19f9d75c0cd4048e2", DefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Writing = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Writing", "0xfed304ecd9c45ced0893710eeafdfeb7a332b2cd", DefaultImage) },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Writing", "0xc54a5127d9b434debd5b3e5613cfa1c8478d7dda", DefaultImage) },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Writing", "0x7c38b98cc50fca103529253f0be1c079f7e39b82", DefaultImage) },
        { NetworkConfig.MantleSepolia,       new NFTContract("Writing", "0x5c19c863d930c28b503701c3bcdb61852edda122", DefaultImage) },
    };

    public static readonly Dictionary<NetworkCore, NFTContract>[] KnownNFTContracts = [
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
