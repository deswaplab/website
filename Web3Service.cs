using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Metamask;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Siwe.Core.Recap;
using System.Numerics;

namespace DeswapApp;

public class Web3Service(MetamaskHostProvider metamaskHostProvider, ILogger<Web3Service> logger)
{
    private readonly string OptionsABI = """
[
    {"type":"function","name":"burn","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"mint","inputs":[{"name":"kind","type":"uint8","internalType":"enum OptionsNFT.OptionsKind"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"quoteAssetAmount","type":"uint256","internalType":"uint256"},{"name":"maturityDate","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"exercise","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
]
""";

    private readonly string LotteryABI = """
[
    {"type":"function","name":"draw","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"address","internalType":"address"}],"stateMutability":"nonpayable"},
    {"type":"function","name":"mint","inputs":[{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"drawTime","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"}
]
""";

    private readonly string RedEnvelopeABI = """
[
    {"type":"function","name":"mint","inputs":[{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"kind","type":"uint8","internalType":"enum RedEnvelopeNFT.RedEnvelopeKind"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"open","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"nonpayable"}
]
""";

    private readonly string RouletteABI = """
[
    {"type":"function","name":"bet","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"mint","inputs":[{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"openTime","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"open","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"address","internalType":"address"}],"stateMutability":"nonpayable"}
]
""";

    private readonly string BlackJackABI = """
[
    {"type":"function","name":"mint","inputs":[{"name":"amount","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"deposit","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"withdraw","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"PlaceBet","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"bet","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"Split","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"Stand","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"StartNewGame","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"bet","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"Hit","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"Insurance","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"DoubleDown","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"GetGame","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"user","type":"address","internalType":"address"}],
    "outputs":[
        {"name":"game","type":"tuple","internalType":"struct BlackJackNFT.Game","components":[
            {"name":"Id","type":"uint256","internalType":"uint256"},
            {"name":"Player","type":"address","internalType":"address"},
            {"name":"SafeBalance","type":"uint256","internalType":"uint256"},
            {"name":"OriginalBalance","type":"uint256","internalType":"uint256"},
            {"name":"SplitCounter","type":"uint256","internalType":"uint256"},
            {"name":"GamesPlayed","type":"uint256","internalType":"uint256"},
            {"name":"PlayerBet","type":"uint256","internalType":"uint256"},
            {"name":"InsuranceBet","type":"uint256","internalType":"uint256"},
            {"name":"PlayerCard1","type":"uint256","internalType":"uint256"},
            {"name":"PlayerCard2","type":"uint256","internalType":"uint256"},
            {"name":"PlayerNewCard","type":"uint256","internalType":"uint256"},
            {"name":"PlayerCardTotal","type":"uint256","internalType":"uint256"},
            {"name":"PlayerSplitTotal","type":"uint256","internalType":"uint256"},
            {"name":"DealerCard1","type":"uint256","internalType":"uint256"},
            {"name":"DealerCard2","type":"uint256","internalType":"uint256"},
            {"name":"DealerNewCard","type":"uint256","internalType":"uint256"},
            {"name":"DealerCardTotal","type":"uint256","internalType":"uint256"},
            {"name":"CanDoubleDown","type":"bool","internalType":"bool"},
            {"name":"CanInsure","type":"bool","internalType":"bool"},
            {"name":"CanSplit","type":"bool","internalType":"bool"},
            {"name":"IsSplitting","type":"bool","internalType":"bool"},
            {"name":"IsSoftHand","type":"bool","internalType":"bool"},
            {"name":"IsRoundInProgress","type":"bool","internalType":"bool"},
            {"name":"DealerMsg","type":"string","internalType":"string"}
        ]}],"stateMutability":"view"}
]
""";

    private readonly MetamaskHostProvider _metamaskHostProvider = metamaskHostProvider;

    private readonly ILogger<Web3Service> _logger = logger;

    private readonly HexBigInteger DefaultFee = new(BigInteger.Parse("1000000000000000")); // default: 0.001 eth

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; } = "";
    }

    public async Task<BigInteger> GetUserBalance(string userAddress, string contractAddress)
    {
        var balanceOfFunctionMessage = new BalanceOfFunction()
        {
            Owner = userAddress,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);
        return balance;
    }

    [Function("allowance", "uint256")]
    public class AllowanceFunction : FunctionMessage
    {
        [Parameter("address", "addr1", 1)]
        public string Addr1 { get; set; } = "";

        [Parameter("address", "addr2", 2)]
        public string Addr2 { get; set; } = "";

    }

    public async Task<BigInteger> GetAllowance(string payAddr, string recvAddr, string contractAddress)
    {
        var allowanceFunctionMessage = new AllowanceFunction()
        {
            Addr1 = payAddr,
            Addr2 = recvAddr,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var handler = web3.Eth.GetContractQueryHandler<AllowanceFunction>();
        var balance = await handler.QueryAsync<BigInteger>(contractAddress, allowanceFunctionMessage);
        return balance;
    }

    // ERC-1155 Uri
    [Function("uri", "string")]
    public class UriFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public BigInteger TokenId { get; set; }
    }
    public async Task<string> GetUri(string contractAddress, long tokenId)
    {
        var UriFunctionMessage = new UriFunction()
        {
            TokenId = tokenId,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<UriFunction>();
        var uri = await balanceHandler.QueryAsync<string>(contractAddress, UriFunctionMessage);
        _logger.LogInformation("get uri, {}, {}", tokenId, uri);
        return uri;
    }

    // ERC-721 tokenUri
    [Function("tokenURI", "string")]
    public class TokenUriFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public BigInteger TokenId { get; set; }
    }
    public async Task<string> GetTokenUri(string contractAddress, long tokenId)
    {
        var TokenUriFunctionMessage = new TokenUriFunction()
        {
            TokenId = tokenId,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<TokenUriFunction>();
        var uri = await balanceHandler.QueryAsync<string>(contractAddress, TokenUriFunctionMessage);
        _logger.LogInformation("get token uri, {}, {}", tokenId, uri);
        return uri;
    }

    [Event("Transfer")]
    public class TransferEventDTO : IEventDTO
    {

        [Parameter("address", "from", 1, true)]
        public string From { get; set; } = "";

        [Parameter("address", "to", 2, true)]
        public string To { get; set; } = "";

        [Parameter("uint256", "tokenId", 3, true)]
        public BigInteger TokenId { get; set; }
    }

    // erc1155
    [Event("TransferSingle")]
    public class TransferSingleEventDTO : IEventDTO
    {
        [Parameter("address", "operator", 1, true)]
        public string Operator { get; set; } = "";

        [Parameter("address", "from", 2, true)]
        public string From { get; set; } = "";

        [Parameter("address", "to", 3, true)]
        public string To { get; set; } = "";

        [Parameter("uint256", "id", 4, false)]
        public BigInteger Id { get; set; }

        [Parameter("uint256", "value", 5, false)]
        public BigInteger Value { get; set; }
    }

    private async Task<TransactionReceipt> SendTransactionThroughMetamask(Function callsFunction, long chainId, string from, HexBigInteger value, params object[] functionInput)
    {
        var gas = await callsFunction.EstimateGasAsync(
            from,
            null,
            value,
            functionInput
        );
        _logger.LogInformation("SendTransactionThroughMetamask estimateGas, chainId={} from={}, gas={}, value={}, functionInput={}", chainId, from, gas, value, functionInput);

        if (chainId == 5003)
        {
            // mantle 支持1559，但是priority fee建议填0, baseFee填 0.02gwei
            return await callsFunction.SendTransactionAndWaitForReceiptAsync(
                new HexBigInteger(2),
                from,
                gas,
                value,
                new HexBigInteger(BigInteger.Parse("20000000")),
                new HexBigInteger(0),
                functionInput
            );
        }
        else if (chainId == 245022926 || chainId == 534351)
        {
            // https://docs.neonevm.org/docs/evm_compatibility/overview
            // https://docs.scroll.io/en/technology/chain/differences/
            // neon, scroll doesn't support eip1559
            return await callsFunction.SendTransactionAndWaitForReceiptAsync(
                from,
                gas,
                value,
                CancellationToken.None,
                functionInput
            );
        }
        else
        {
            return await callsFunction.SendTransactionAndWaitForReceiptAsync(
                new HexBigInteger(2),
                from,
                gas,
                value,
                null,
                null,
                functionInput
            );
        }
    }

    // 用户取消授权的话，会报异常，调用方应该处理这个异常
    public async Task<string> Approve(string nftAddr, BigInteger wad, string erc20Contract)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        string erc20ApproveAbi = """[{"inputs":[{"internalType":"address","name":"guy","type":"address"},{"internalType":"uint256","name":"wad","type":"uint256"}],"name":"approve","outputs":[{"internalType":"bool","name":"","type":"bool"}],"stateMutability":"nonpayable","type":"function"}]""";
        var contract = web3.Eth.GetContract(erc20ApproveAbi, erc20Contract);
        var callsFunction = contract.GetFunction("approve");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, nftAddr, wad);
        return receipt.TransactionHash.ToString();
    }

    // Mint a Options NFT
    public async Task<long> MintOptions(OptionsKind kind, BigInteger baseAssetAmount, BigInteger quoteAssetAmount, long maturityUnix, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee;
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, kind, baseAssetAmount, quoteAssetAmount, maturityUnix);
        _logger.LogInformation("mintOptions, tx={}", receipt.TransactionHash.ToString());
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        // avoid mixing erc721 and erc20 transfer
        foreach (var item in events)
        {
            _logger.LogInformation($"from: {item.Event.From}, to: {item.Event.To}");
        }
        return events.Where(item => item.Event.To.Equals(_metamaskHostProvider.SelectedAccount, StringComparison.CurrentCultureIgnoreCase)).Select(item => (long)item.Event.TokenId).First();
    }

    // 期权行权
    public async Task<string> ExerciseOptions(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("exercise");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        return receipt.TransactionHash.ToString();
    }

    // 作废（燃烧）行权
    public async Task<string> BurnOptions(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        return receipt.TransactionHash.ToString();
    }

    // Mint a Lottery NFT
    public async Task<long> MintLottery(BigInteger baseAssetAmount, long maturityUnix, int amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee;
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, baseAssetAmount, maturityUnix, amount);
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<string> DrawLottery(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("draw");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        return receipt.TransactionHash.ToString();
    }

    // mint a red envelope
    public async Task<long> MintRedEnvelope(BigInteger baseAssetAmount, int amount, RedEnvelopeKind kind, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, baseAssetAmount, amount, kind);
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<string> OpenRedEnvelope(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("open");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        return receipt.TransactionHash.ToString();
    }

    // roulette
    public async Task<long> MintRoulette(int amount, long openTime, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, amount, openTime);
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<string> BetRoulette(BigInteger baseAssetAmount, long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bet");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId, baseAssetAmount);
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> OpenRoulette(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("open");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        return receipt.TransactionHash.ToString();
    }

    // blackjack
    public async Task<long> MintBlackJack(int amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, amount);
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<string> DepositBlackJack(long tokenId, BigInteger amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, nftAddress);
        var callsFunction = contract.GetFunction("deposit");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId, amount);
        return receipt.TransactionHash.ToString();
    }

    [Function("GetGame", "uint256")]
    public class GetGameFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public long TokenId { get; set; }

        [Parameter("string", "User", 2)]
        public string User { get; set; } = "";
    }

    [FunctionOutput]
    public class GetGameOutputDTO : IFunctionOutputDTO
    {
        [Parameter("int256", "Id", 1)]
        public virtual BigInteger Id { get; set; }

        [Parameter("address", "Player", 2)]
        public virtual string Player { get; set; } = "";

        [Parameter("int256", "SafeBalance", 3)]
        public virtual BigInteger SafeBalance { get; set; }

        [Parameter("int256", "OriginalBalance", 4)]
        public virtual BigInteger OriginalBalance { get; set; }

        [Parameter("int256", "SplitCounter", 5)]
        public virtual BigInteger SplitCounter { get; set; }

        [Parameter("int256", "GamesPlayed", 6)]
        public virtual BigInteger GamesPlayed { get; set; }

        [Parameter("int256", "PlayerBet", 7)]
        public virtual BigInteger PlayerBet { get; set; }

        [Parameter("int256", "InsuranceBet", 8)]
        public virtual BigInteger InsuranceBet { get; set; }

        [Parameter("int256", "PlayerCard1", 9)]
        public virtual BigInteger PlayerCard1 { get; set; }

        [Parameter("int256", "PlayerCard2", 10)]
        public virtual BigInteger PlayerCard2 { get; set; }

        [Parameter("int256", "PlayerNewCard", 11)]
        public virtual BigInteger PlayerNewCard { get; set; }

        [Parameter("int256", "PlayerCardTotal", 12)]
        public virtual BigInteger PlayerCardTotal { get; set; }

        [Parameter("int256", "PlayerSplitTotal", 13)]
        public virtual BigInteger PlayerSplitTotal { get; set; }

        [Parameter("int256", "DealerCard1", 14)]
        public virtual BigInteger DealerCard1 { get; set; }

        [Parameter("int256", "DealerCard2", 15)]
        public virtual BigInteger DealerCard2 { get; set; }

        [Parameter("int256", "DealerNewCard", 16)]
        public virtual BigInteger DealerNewCard { get; set; }

        [Parameter("int256", "DealerCardTotal", 17)]
        public virtual BigInteger DealerCardTotal { get; set; }

        [Parameter("bool", "CanDoubleDown", 18)]
        public virtual bool CanDoubleDown { get; set; }

        [Parameter("bool", "CanInsure", 19)]
        public virtual bool CanInsure { get; set; }

        [Parameter("bool", "CanSplit", 20)]
        public virtual bool CanSplit { get; set; }

        [Parameter("bool", "IsSplitting", 21)]
        public virtual bool IsSplitting { get; set; }

        [Parameter("bool", "IsSoftHand", 22)]
        public virtual bool IsSoftHand { get; set; }

        [Parameter("bool", "IsRoundInProgress", 23)]
        public virtual bool IsRoundInProgress { get; set; }

        [Parameter("string", "DealerMsg", 24)]
        public virtual string DealerMsg { get; set; } = "";

    }

    public async Task GetUserBlackJackGame(long tokenId, string userAddress, string contractAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, contractAddress);
        var callsFunction = contract.GetFunction("GetGame");

        var output = await callsFunction.CallDeserializingToObjectAsync<GetGameOutputDTO>(tokenId, userAddress);
        _logger.LogInformation("game is {}", output);
    }

    public async Task<string> StartBlackJack(long tokenId, BigInteger amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, nftAddress);
        var callsFunction = contract.GetFunction("StartNewGame");

        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId, amount);
        return receipt.TransactionHash.ToString();
    }
}

public enum RedEnvelopeKind
{
    Fixed = 0,
    Random = 1,
}
