namespace DeswapApp;

public record ERC20Contract(string Name, string Address, int Decimals);
public record NFTContract(string Name, string Address);

public record NetworkCore
{
    public required string Name { get; set; }

    public required string InnerName { get; set; }

    public string RpcUrl { get; set; } = "";

    public long ChainId { get; set; }

    public required string EtherscanHost { get; set; }

    // opensea 主界面对应的host
    public required string OpenseaHost { get; set; }

    // opensea api 对应的host
    public required string OpenseaApiHost { get; set; }

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

    public string BuildDetailUrl(string contractAddress, long tokenId)
    {
        return $"/app/{InnerName}/{contractAddress}/{tokenId}";
    }

    public string BuildNftUrl(string contractAddress, long tokenId)
    {
        if (string.IsNullOrEmpty(OpenseaHost))
        {
            return "";
        }
        return $"{OpenseaHost}/{contractAddress}/{tokenId}";
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
    public static NetworkCore EthereumSepolia { get; } = new()
    {
        Name = "Sepolia",
        InnerName = "sepolia",
        ChainId = 11155111,
        EtherscanHost = "https://sepolia.etherscan.io",
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
        OpenseaHost = "",
        OpenseaApiHost = "",
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
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "neon_logo.svg",
        IsTestNet = true,
        RpcUrl = "https://devnet.neonevm.org",
    };

    public static NetworkCore MantaPacificSepolia { get; } = new()
    {
        Name = "Manta Pacific Sepolia", // nft api 501 not implemented
        InnerName = "manta_pacific_sepolia",
        ChainId = 3441006,
        EtherscanHost = "https://pacific-explorer.sepolia-testnet.manta.network",
        OpenseaHost = "",
        OpenseaApiHost = "",
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
        EtherscanHost = "https://explorer.testnet.aurora.dev",
        OpenseaHost = "",
        OpenseaApiHost = "",
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
        EtherscanHost = "https://explorer.sepolia.mantle.xyz",
        OpenseaHost = "",
        OpenseaApiHost = "",
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
        OpenseaHost = "",
        OpenseaApiHost = "",
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

    public static readonly Dictionary<NetworkCore, NFTContract> Barter = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Barter", "0x677dfb01d6F3C30EA1Ce4497775AB94EAB699981") },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Barter", "0xb9afd20d69CEEdC249E32649ef10015af6C05c3F") },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Barter", "0xBAA68A83fa8a0eb7c1144EE275350c3C4F5B61b0") },
        { NetworkConfig.MantleSepolia,       new NFTContract("Barter", "0x5A4cA8E3Cd655F5Ec72a353A40aD82159A37FC58") },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Lottery = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Lottery", "0xF1a162C2d43ea2d1f2Af764cC6E564EaC2A7dC01") },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Lottery", "0x5cAE158176D8c30F7038b127Da9FC845B916a99A") },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Lottery", "0x86cF9996b7a8aF46E74A9FBbd28b149a2f7F33e2") },
        { NetworkConfig.MantleSepolia,       new NFTContract("Lottery", "0xD40D28361Ef1c9FB6E672E0905548CAdD4d70AA1") },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> RedEnvelope = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("RedEnvelope", "0x629391548a7D4f9848e2F9377aFFC5540F4158A7") },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("RedEnvelope", "0xE7cd7918C6f402cf29779DAC7F4Ba582F7448016") },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("RedEnvelope", "0xCeA52909295CAABEFF8Dc7820bDF9Bf8403683D9") },
        { NetworkConfig.MantleSepolia,       new NFTContract("RedEnvelope", "0x8106ac53d21B3180471a19D359d610A19c15aD9B") },
    };

    public static readonly Dictionary<NetworkCore, NFTContract> Roulette = new() {
        { NetworkConfig.EthereumSepolia,     new NFTContract("Roulette", "0xaC0f2E348A8501349c366cEfe49658918CaccA01") },
        { NetworkConfig.MoonBaseAlpha,       new NFTContract("Roulette", "0xCeA52909295CAABEFF8Dc7820bDF9Bf8403683D9") },
        { NetworkConfig.MantaPacificSepolia, new NFTContract("Roulette", "0x508A1f2c538Ae9f1BF1D923f0d90ff3372c9720B") },
        { NetworkConfig.MantleSepolia,       new NFTContract("Roulette", "0x7C38B98cc50fcA103529253F0Be1C079f7E39b82") },
    };

    public static string GetContractKind(NetworkCore? network, string contractAddress)
    {
        if (network == null)
        {
            return "";
        }
        if (Barter.TryGetValue(network, out var barter) && barter.Address.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase))
        {
            return "Barter";
        }
        if (Lottery.TryGetValue(network, out var lottery) && lottery.Address.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase))
        {
            return "Lottery";
        }
        if (RedEnvelope.TryGetValue(network, out var redEnvelope) && redEnvelope.Address.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase))
        {
            return "RedEnvelope";
        }
        if (Roulette.TryGetValue(network, out var roulette) && roulette.Address.Equals(contractAddress, StringComparison.CurrentCultureIgnoreCase))
        {
            return "Roulette";
        }

        return "";
    }

    public static IList<string> GetAllContracts(NetworkCore? network)
    {
        if (network == null)
        {
            return [];
        }
        var res = new List<string>();
        if (Barter.TryGetValue(network, out var barter))
        {
            res.Add(barter.Address);
        }
        if (Lottery.TryGetValue(network, out var lottery))
        {
            res.Add(lottery.Address);
        }
        if (RedEnvelope.TryGetValue(network, out var redEnvelope))
        {
            res.Add(redEnvelope.Address);
        }
        if (Roulette.TryGetValue(network, out var roulette))
        {
            res.Add(roulette.Address);
        }

        return res;
    }
}
