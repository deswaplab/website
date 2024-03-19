using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Metamask;
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

    // 用户取消授权的话，会报异常，调用方应该处理这个异常
    public async Task<string> Approve(string nftAddr, BigInteger wad, string erc20Contract)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        string erc20ApproveAbi = """[{"inputs":[{"internalType":"address","name":"guy","type":"address"},{"internalType":"uint256","name":"wad","type":"uint256"}],"name":"approve","outputs":[{"internalType":"bool","name":"","type":"bool"}],"stateMutability":"nonpayable","type":"function"}]""";
        var contract = web3.Eth.GetContract(erc20ApproveAbi, erc20Contract);
        var callsFunction = contract.GetFunction("approve");
        var gas = await callsFunction.EstimateGasAsync(
            nftAddr,
            wad
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            nftAddr,
            wad
        );
        return receipt.TransactionHash.ToString();
    }

    // Mint a Options NFT
    public async Task<string> MintOptions(OptionsKind kind, BigInteger baseAssetAmount, BigInteger quoteAssetAmount, long maturityUnix, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000"));
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            kind,
            baseAssetAmount,
            quoteAssetAmount,
            maturityUnix
        );
        _logger.LogInformation($"Mint Options, gas={gas}, value={value}");
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            kind,
            baseAssetAmount,
            quoteAssetAmount,
            maturityUnix
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    // 期权行权
    public async Task<string> ExerciseOptions(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("exercise");
        var gas = await callsFunction.EstimateGasAsync(
            tokenId
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    // 作废（燃烧）行权
    public async Task<string> BurnOptions(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var gas = await callsFunction.EstimateGasAsync(
            tokenId
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    // Mint a Lottery NFT
    public async Task<string> MintLottery(BigInteger baseAssetAmount, long maturityUnix, int amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000"));
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            baseAssetAmount,
            maturityUnix,
            amount
        );
        _logger.LogInformation($"Mint Lottery, gas={gas}, value={value}");
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            baseAssetAmount,
            maturityUnix,
            amount
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> DrawLottery(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("draw");
        var gas = await callsFunction.EstimateGasAsync(
            tokenId
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    // mint a red envelope
    public async Task<string> MintRedEnvelope(BigInteger baseAssetAmount, int amount, RedEnvelopeKind kind, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000")); // 0.001 fee
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            baseAssetAmount,
            amount,
            kind
        );
        _logger.LogInformation("Mint RedEnvelope, gas={}, value={}", gas, value);
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            baseAssetAmount,
            amount,
            kind
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> OpenRedEnvelope(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("open");
        var gas = await callsFunction.EstimateGasAsync(
            tokenId
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    // roulette
    public async Task<string> MintRoulette(int amount, long openTime, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000")); // 0.001 fee
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            amount,
            openTime
        );
        _logger.LogInformation("Mint Roulette, gas={}, value={}", gas, value);
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            amount,
            openTime
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> BetRoulette(BigInteger baseAssetAmount, long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bet");
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            tokenId,
            baseAssetAmount
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId,
            baseAssetAmount
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> OpenRoulette(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("open");
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            tokenId
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    // blackjack
    public async Task<string> MintBlackJack(int amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000")); // 0.001 fee
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            amount
        );
        _logger.LogInformation("Mint BlackJack, gas={}, value={}", gas, value);
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            amount
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> DepositBlackJack(long tokenId, BigInteger amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BlackJackABI, nftAddress);
        var callsFunction = contract.GetFunction("deposit");
        
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000")); // 0.001 fee
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            tokenId,
            amount
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            tokenId,
            amount
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
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
    public class GetGameOutputDTO: IFunctionOutputDTO 
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
        
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            tokenId,
            amount
        );
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            CancellationToken.None,
            tokenId,
            amount
        );
        // TODO: 返回tokenId，添加一个跳转去交易的链接
        return receipt.TransactionHash.ToString();
    }
}

public enum RedEnvelopeKind
{
    Fixed = 0,
    Random = 1,
}
