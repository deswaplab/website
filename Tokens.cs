namespace DeswapApp;

public record Erc20Contract(string Name, string Address, int Decimals);
public record SingleAssetNFTContract(string Name, string Address, Network Network, Erc20Contract BaseAsset);
public record TwoAssetNFTContract(string Name, string Address, Network Network, Erc20Contract BaseAsset, Erc20Contract QuoteAsset);

public class OptionsContracts
{
    public IList<TwoAssetNFTContract> Inner = [
        new TwoAssetNFTContract("Options WETH/TUSDC", "0x3A0058D768f936764b526Bd691fde0A0ad077e49", SupportedNetworks.EthereumSepolia, SupportedNetworks.EthereumSepolia.Weth!, SupportedNetworks.EthereumSepolia.Tusdc!),
        new TwoAssetNFTContract("Options WETH/TUSDC", "0x3A0058D768f936764b526Bd691fde0A0ad077e49", SupportedNetworks.PolygonMumbai, SupportedNetworks.PolygonMumbai.Weth!, SupportedNetworks.PolygonMumbai.Tusdc!),
        new TwoAssetNFTContract("Options WETH/TUSDC", "0x6c81708c36A37D0CF527fF9b0a2eC98249a84257", SupportedNetworks.MoonBaseAlpha, SupportedNetworks.MoonBaseAlpha.Weth!, SupportedNetworks.MoonBaseAlpha.Tusdc!),
        new TwoAssetNFTContract("Options WETH/TUSDC", "0xE37C321d3e1Af89A9D027f57CCa5D3d106b7921F", SupportedNetworks.MantaPacificSepolia, SupportedNetworks.MantaPacificSepolia.Weth!, SupportedNetworks.MantaPacificSepolia.Tusdc!),
        new TwoAssetNFTContract("Options WETH/TUSDC", "0x2C25496FF3b4cbA727c5fd6970079C5Ea481cdbf", SupportedNetworks.MantleSepolia, SupportedNetworks.MantleSepolia.Weth!, SupportedNetworks.MantleSepolia.Tusdc!),
    ];
}

public class LotteryContracts
{
    public IList<SingleAssetNFTContract> Inner = [
        new SingleAssetNFTContract("Lottery WETH", "0x329F7da3EE132a5836eFde559cA124557b2baa7B", SupportedNetworks.EthereumSepolia, SupportedNetworks.EthereumSepolia.Weth!),
        new SingleAssetNFTContract("Lottery WETH", "0x329F7da3EE132a5836eFde559cA124557b2baa7B", SupportedNetworks.PolygonMumbai, SupportedNetworks.PolygonMumbai.Weth!),
        new SingleAssetNFTContract("Lottery WETH", "0x45CA457Ca59FeD6541275Cc4f83c80Ede548D748", SupportedNetworks.MoonBaseAlpha, SupportedNetworks.MoonBaseAlpha.Weth!),
        new SingleAssetNFTContract("Lottery WETH", "0x0EBB63025caE604fd9230A2fdc811a84394A7861", SupportedNetworks.MantaPacificSepolia, SupportedNetworks.MantaPacificSepolia.Weth!),
        new SingleAssetNFTContract("Lottery WETH", "0x415f9bcD4747BB9F20beCadD3e2E70670d8bC3C3", SupportedNetworks.MantleSepolia, SupportedNetworks.MantleSepolia.Weth!),

    ];
}

public class RedEnvelopeContracts
{
    public readonly IList<SingleAssetNFTContract> Inner = [
        new SingleAssetNFTContract("RedEnvelope WETH", "0x5F864E929BF0E6AA1266DCE9437CAD366D11a115", SupportedNetworks.EthereumSepolia, SupportedNetworks.EthereumSepolia.Weth!),
        new SingleAssetNFTContract("RedEnvelope WETH", "0x5F864E929BF0E6AA1266DCE9437CAD366D11a115", SupportedNetworks.PolygonMumbai, SupportedNetworks.PolygonMumbai.Weth!),
        new SingleAssetNFTContract("RedEnvelope WETH", "0xbC88E564B8761E9179A8A41A60F5509Ad21E99F5", SupportedNetworks.MoonBaseAlpha, SupportedNetworks.MoonBaseAlpha.Weth!),
        new SingleAssetNFTContract("RedEnvelope WETH", "0x543B59955cEb03169EcbF8eE63312a5258212098", SupportedNetworks.MantaPacificSepolia, SupportedNetworks.MantaPacificSepolia.Weth!),
        new SingleAssetNFTContract("RedEnvelope WETH", "0xf6F0F9dE6b0D4e10A8F6423124Df47E4957d7205", SupportedNetworks.MantleSepolia, SupportedNetworks.MantleSepolia.Weth!),
    ];
}

public class PasswordRedEnvelopeContracts
{
    public readonly IList<SingleAssetNFTContract> Inner = [
        new SingleAssetNFTContract("Pass RedEnvelope WETH", "0x4CA4765346cfc31E02E3FcBD63bF56Eb55084030", SupportedNetworks.EthereumSepolia, SupportedNetworks.EthereumSepolia.Weth!),
        new SingleAssetNFTContract("Pass RedEnvelope WETH", "0x4CA4765346cfc31E02E3FcBD63bF56Eb55084030", SupportedNetworks.PolygonMumbai, SupportedNetworks.PolygonMumbai.Weth!),
        new SingleAssetNFTContract("Pass RedEnvelope WETH", "0x0F7B8AFb2C26B19162ea455bb4EdA613A159A45B", SupportedNetworks.MoonBaseAlpha, SupportedNetworks.MoonBaseAlpha.Weth!),
        new SingleAssetNFTContract("Pass RedEnvelope WETH", "0x2e82B58b8B5e0B694a51a864e21D7b845c6Fc372", SupportedNetworks.MantaPacificSepolia, SupportedNetworks.MantaPacificSepolia.Weth!),
        new SingleAssetNFTContract("Pass RedEnvelope WETH", "0x8C486c8C6D2EbE319511Ee130037333bA4Ac9a49", SupportedNetworks.MantleSepolia, SupportedNetworks.MantleSepolia.Weth!),
    ];
}

public class RouletteContracts
{
    public readonly IList<SingleAssetNFTContract> Inner = [
        new SingleAssetNFTContract("Roulette WETH", "0x33f05D53a140B25DFBd3b787269F438b3200B202", SupportedNetworks.EthereumSepolia, SupportedNetworks.EthereumSepolia.Weth!),
        new SingleAssetNFTContract("Roulette WETH", "0x33f05D53a140B25DFBd3b787269F438b3200B202", SupportedNetworks.PolygonMumbai, SupportedNetworks.PolygonMumbai.Weth!),
        new SingleAssetNFTContract("Roulette WETH", "0x3aac46e7202Dd08b8bE2B92D30773Cb927F4d737", SupportedNetworks.MoonBaseAlpha, SupportedNetworks.MoonBaseAlpha.Weth!),
        new SingleAssetNFTContract("Roulette WETH", "0xD43727BdbC47047889E53F6aD1c45460D829FcAD", SupportedNetworks.MantaPacificSepolia, SupportedNetworks.MantaPacificSepolia.Weth!),
        new SingleAssetNFTContract("Roulette WETH", "0x47CEA152577A40EB97Ef02cF6E64b5645fB1D748", SupportedNetworks.MantleSepolia, SupportedNetworks.MantleSepolia.Weth!),
    ];
}

public static class BlackJackContracts
{
    public static readonly IList<BlackJackContract> Inner = [
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9",
            BaseAssetDecimals = 18,
            NftAddress = "0xCEe20496BAa9C4F41Ca217f6fDb89213c62676bC",
            Network = SupportedNetworks.EthereumSepolia,
        },
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889",
            BaseAssetDecimals = 18,
            NftAddress = "0xCEe20496BAa9C4F41Ca217f6fDb89213c62676bC",
            Network = SupportedNetworks.PolygonMumbai,
        },
        new BlackJackContract{
            BaseAssetName = "WETH",
            BaseAssetAddress = "0xD909178CC99d318e4D46e7E66a972955859670E1",
            BaseAssetDecimals = 18,
            NftAddress = "0x912D36F448b9D9456736aB04Ce041767a8e827a1",
            Network = SupportedNetworks.MoonBaseAlpha,
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

public record Network
{
    public required string Name { get; set; }

    public long ChainId { get; set; }

    public required string EtherscanHost { get; set; }

    // opensea 主界面对应的host
    public required string OpenseaHost { get; set; }

    // opensea api 对应的host
    public required string OpenseaApiHost { get; set; }

    public string? CovalentApiHost { get; set; }

    public required string Logo { get; set; } // svg文件，保存在 wwwroot/img 下

    public bool IsTestNet { get; set; }

    public Erc20Contract? Weth { get; set; }

    public Erc20Contract? Tusdc { get; set; }

    public string BuildListUrl(string contractAddress, long tokenId)
    {
        if (string.IsNullOrEmpty(OpenseaHost))
        {
            return "";
        }
        return $"{OpenseaHost}/{contractAddress}/{tokenId}/sell";
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
}

public static class SupportedNetworks
{
    public static Network EthereumSepolia { get; } = new Network
    {
        Name = "Sepolia",
        ChainId = 11155111,
        EtherscanHost = "https://sepolia.etherscan.io",
        OpenseaHost = "https://testnets.opensea.io/assets/sepolia",
        OpenseaApiHost = "https://testnets-api.opensea.io/api/v2/chain/sepolia",
        Logo = "ethereum_logo.svg",
        IsTestNet = true,
        Weth = new Erc20Contract("WETH", "0x7b79995e5f793A07Bc00c21412e50Ecae098E7f9", 18),
        Tusdc = new Erc20Contract("TUSDC", "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5", 6)
    };

    public static Network PolygonMumbai { get; } = new Network
    {
        Name = "Pylogon Mumbai",
        ChainId = 80001,
        EtherscanHost = "https://mumbai.polygonscan.com",
        OpenseaHost = "https://testnets.opensea.io/assets/mumbai",
        OpenseaApiHost = "https://testnets-api.opensea.io/api/v2/chain/mumbai",
        Logo = "polygon_logo.svg",
        IsTestNet = true,
                Weth = new Erc20Contract("WETH", "0x9c3C9283D3e44854697Cd22D3Faa240Cfb032889", 18),
        Tusdc = new Erc20Contract("TUSDC", "0xb53ff72177708cd6A643544B7caD9a2768aCC8E5", 6)

    };

    public static Network MoonBaseAlpha { get; } = new Network
    {
        Name = "MoonBase Alpha",
        ChainId = 1287,
        EtherscanHost = "https://moonbase.moonscan.io",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "moonbase_alpha_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/moonbeam-moonbase-alpha",
        Weth = new Erc20Contract("WETH", "0xD909178CC99d318e4D46e7E66a972955859670E1", 18),
        Tusdc = new Erc20Contract("TUSDC", "0x3070a7eA1bC31049068f055f9b31f5d2D7bdfb5d", 6)
    };

    public static Network SolanaNeonDev { get; } = new Network
    {
        Name = "Neon dev",
        ChainId = 245022926,
        EtherscanHost = "https://devnet.neonscan.org",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "neon_logo.svg",
        IsTestNet = true,
        Weth = new Erc20Contract("WETH", "0x11adC2d986E334137b9ad0a0F290771F31e9517F", 18),
        Tusdc = new Erc20Contract("TUSDC", "0xe33D3D26d5C75bFFb0170d1F06A2c442e643F65E", 6)
    };

    public static Network MantaPacificSepolia { get; } = new Network
    {
        Name = "Manta Pacific Sepolia", // nft api 501 not implemented
        ChainId = 3441006,
        EtherscanHost = "https://pacific-explorer.sepolia-testnet.manta.network",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "manta_pacific_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/manta-testnet",
        Weth = new Erc20Contract("WETH", "0x199d1a27684106dC3Deb673115fc0fc9cf6af287", 18),
        Tusdc = new Erc20Contract("TUSDC", "0x912D36F448b9D9456736aB04Ce041767a8e827a1", 6)
    };

    public static Network NearAuroraTestnet { get; } = new Network
    {
        Name = "Aurora Testnet",
        ChainId = 1313161555,
        EtherscanHost = "https://explorer.testnet.aurora.dev",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "aurora_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/aurora-testnet",
        Weth = new Erc20Contract("WETH", "0x8886E7A8883e9A40b30Bd4E16e0e25C2C3f29Cd8", 18),
        Tusdc = new Erc20Contract("TUSDC", "0x45CA457Ca59FeD6541275Cc4f83c80Ede548D748", 6)
    };

    public static Network MantleSepolia { get; } = new Network
    {
        Name = "Mantle Sepolia", // nft api 501 not implemented
        ChainId = 5003,
        EtherscanHost = "https://explorer.sepolia.mantle.xyz",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "mantle_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/mantle-testnet",
        Weth = new Erc20Contract("WETH", "0x17f711A85D359cBF0224a017d8e3dd7A29c9932E", 18),
        Tusdc = new Erc20Contract("TUSDC", "0x3070a7eA1bC31049068f055f9b31f5d2D7bdfb5d", 6)
    };

    public static Network ScrollSepolia { get; } = new Network
    {
        Name = "Scroll Sepolia", // nft api 501 not implemented
        ChainId = 534351,
        EtherscanHost = "https://sepolia.scrollscan.com",
        OpenseaHost = "",
        OpenseaApiHost = "",
        Logo = "scroll_logo.svg",
        IsTestNet = true,
        CovalentApiHost = "https://api.covalenthq.com/v1/scroll-sepolia-testnet",
        Weth = new Erc20Contract("WETH", "0x5300000000000000000000000000000000000004", 18),
        Tusdc = new Erc20Contract("TUSDC", "0x7374FE94e34c209616cEc0610212DE13151D222f", 6)
    };

    public static readonly IList<Network> Inner = [
        EthereumSepolia,
        PolygonMumbai,
        MoonBaseAlpha,
        // SolanaNeonDev,
        MantaPacificSepolia,
        // NearAuroraTestnet,
        MantleSepolia,
        // ScrollSepolia, // scroll doesn't support difficulty/prevrandao, doesn't support
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
