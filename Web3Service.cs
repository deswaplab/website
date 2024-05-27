using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Metamask;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Numerics;

namespace DeswapApp;

public class Web3Service(MetamaskHostProvider metamaskHostProvider, ILogger<Web3Service> logger)
{
    private readonly string BarterABI = """
[
    {"type":"function","name":"mint_ft2ft","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"quoteAssetAddress","type":"address","internalType":"address"},{"name":"quoteAssetAmount","type":"uint256","internalType":"uint256"},{"name":"maturityDate","type":"uint64","internalType":"uint64"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint_nft2nft","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"},{"name":"quoteAssetAddress","type":"address","internalType":"address"},{"name":"maturityDate","type":"uint64","internalType":"uint64"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"exercise_ft2ft","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"exercise_nft2nft","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"assetTokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"burn","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"tokenMetadata","inputs":[{"name":"","type":"uint256","internalType":"uint256"}],"outputs":[
        {"name":"writer","type":"address","internalType":"address"},
        {"name":"maturityDate","type":"uint64","internalType":"uint64"},
        {"name":"baseAssetKind","type":"uint8","internalType":"enum Util.AssetKind"},
        {"name":"baseAssetAddress","type":"address","internalType":"address"},
        {"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},
        {"name":"baseAssetTokenId","type":"uint256","internalType":"uint256"},
        {"name":"quoteAssetKind","type":"uint8","internalType":"enum Util.AssetKind"},
        {"name":"quoteAssetAddress","type":"address","internalType":"address"},
        {"name":"quoteAssetAmount","type":"uint256","internalType":"uint256"},
        {"name":"quoteAssetTokenId","type":"uint256","internalType":"uint256"}],"stateMutability":"view"}
]
""";

    private readonly string LotteryABI = """
[
    {"type":"function","name":"draw","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"address","internalType":"address"}],"stateMutability":"nonpayable"},
    {"type":"function","name":"mint20","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"drawTime","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"price","type":"uint256","internalType":"uint256"},{"name":"soldBp","type":"uint96","internalType":"uint96"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint721","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"},{"name":"drawTime","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"price","type":"uint256","internalType":"uint256"},{"name":"soldBp","type":"uint96","internalType":"uint96"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint1155","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"drawTime","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"price","type":"uint256","internalType":"uint256"},{"name":"soldBp","type":"uint96","internalType":"uint96"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"canDraw","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"bool","internalType":"bool"}],"stateMutability":"view"},
    {"type":"function","name":"buy","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"payable"},
    {"type":"function","name":"tokenMetadata","inputs":[{"name":"","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"drawTime","type":"uint256","internalType":"uint256"},{"name":"writer","type":"address","internalType":"address"},{"name":"tokenCount","type":"uint256","internalType":"uint256"},{"name":"isDrawed","type":"bool","internalType":"bool"},{"name":"assetKind","type":"uint8","internalType":"enum Util.AssetKind"},{"name":"defaultPrice","type":"uint256","internalType":"uint256"},{"name":"soldBp","type":"uint96","internalType":"uint96"},{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"baseAssetTokenId","type":"uint256","internalType":"uint256"}],"stateMutability":"view"}
]
""";

    private readonly string RedEnvelopeABI = """
[
    {"type":"function","name":"mint20","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"kind","type":"uint8","internalType":"enum RedEnvelope.RedEnvelopeKind"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint1155","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"amount","type":"uint256","internalType":"uint256"},{"name":"kind","type":"uint8","internalType":"enum RedEnvelope.RedEnvelopeKind"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"open","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"nonpayable"}
]
""";

    private readonly string RouletteABI = """
[
    {"type":"function","name":"mint20","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"ownerFeeBp","type":"uint96","internalType":"uint96"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint721","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetTokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"mint1155","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetTokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"bet20","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"bet721","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"bet1155","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"open","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"address","internalType":"address"}],"stateMutability":"nonpayable"},
    {"type":"function","name":"bettedAddressMap","inputs":[{"name":"id","type":"uint256","internalType":"uint256"},{"name":"addr","type":"address","internalType":"address"}],"outputs":[{"name":"isBeted","type":"bool","internalType":"bool"}],"stateMutability":"view"},
    {"type":"function","name":"getTokenBetAmounts","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256[]","internalType":"uint256[]"}],"stateMutability":"view"},
    {"type":"function","name":"getTokenBetPlayers","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"address[]","internalType":"address[]"}],"stateMutability":"view"}
]
""";

    private readonly string VoteABI = """
[
    {"type":"function","name":"mint","inputs":[{"name":"title","type":"string","internalType":"string"},{"name":"choices","type":"bytes","internalType":"bytes"},{"name":"choiceLength","type":"uint256","internalType":"uint256"},{"name":"expireTime","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"vote","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"answer","type":"uint8","internalType":"enum Vote.Answer"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"burn","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"tokenVoter","inputs":[{"name":"","type":"uint256","internalType":"uint256"},{"name":"","type":"address","internalType":"address"}],"outputs":[{"name":"","type":"bool","internalType":"bool"}],"stateMutability":"view"}
]
""";

    private readonly string SicboABI = """
[
    {"type":"function","name":"mint","inputs":[{"name":"baseAssetAddress","type":"address","internalType":"address"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"deposit","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"placeBet","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"},{"name":"baseAssetAmount","type":"uint256","internalType":"uint256"},{"name":"result","type":"uint8","internalType":"enum SicBo.DiceResults"}],"outputs":[{"name":"","type":"bool","internalType":"bool"},{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"nonpayable"},
    {"type":"function","name":"burn","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"}
]
""";

    private readonly string WritingABI = """
[
    {"type":"function","name":"mint","inputs":[{"name":"title","type":"string","internalType":"string"},{"name":"content","type":"string","internalType":"string"}],"outputs":[{"name":"","type":"uint256","internalType":"uint256"}],"stateMutability":"payable"},
    {"type":"function","name":"burn","inputs":[{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"},
    {"type":"function","name":"tokenMetadata","inputs":[{"name":"","type":"uint256","internalType":"uint256"}],"outputs":[{"name":"title","type":"string","internalType":"string"},{"name":"content","type":"string","internalType":"string"},{"name":"writer","type":"address","internalType":"address"}],"stateMutability":"view"}
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
    public class Erc20BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; } = "";
    }

    public async Task<BigInteger> Erc20BalanceOf(string userAddress, string contractAddress)
    {
        var balanceOfFunctionMessage = new Erc20BalanceOfFunction()
        {
            Owner = userAddress,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<Erc20BalanceOfFunction>();
        var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);
        return balance;
    }

    public async Task<decimal> Erc20BalanceOf(string userAddress, ERC20Contract contract)
    {
        BigInteger baseBalanceInWei = await Erc20BalanceOf(userAddress, contract.Address);
        return Web3.Convert.FromWei(baseBalanceInWei, contract.Decimals);
    }

    [Function("decimals", "uint8")]
    public class Erc20DecimalsFunction : FunctionMessage
    {
    }

    public async Task<int> Erc20Decimals(string contractAddress)
    {
        var message = new Erc20DecimalsFunction();
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var handler = web3.Eth.GetContractQueryHandler<Erc20DecimalsFunction>();
        var result = await handler.QueryAsync<int>(contractAddress, message);
        return result;
    }

    [Function("symbol", "string")]
    public class Erc20SymbolFunction : FunctionMessage
    {
    }

    public async Task<string> Erc20Symbol(string contractAddress)
    {
        var message = new Erc20SymbolFunction();
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var handler = web3.Eth.GetContractQueryHandler<Erc20SymbolFunction>();
        var result = await handler.QueryAsync<string>(contractAddress, message);
        return result;
    }

    [Function("ownerOf", "address")]
    public class Erc721OwnerOfFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public BigInteger TokenId { get; set; }
    }

    public async Task<string> Erc721OwnerOf(string contractAddress, long tokenId)
    {
        var ownerOfFunctionMessage = new Erc721OwnerOfFunction()
        {
            TokenId = tokenId,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<Erc721OwnerOfFunction>();
        var owner = await balanceHandler.QueryAsync<string>(contractAddress, ownerOfFunctionMessage);
        return owner;
    }

    [Function("balanceOf", "uint256")]
    public class Erc721BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; } = "";
    }

    public async Task<int> Erc721BalanceOf(string contractAddress, string userAddress)
    {
        var message = new Erc721BalanceOfFunction()
        {
            Owner = userAddress,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<Erc721BalanceOfFunction>();
        var owner = await balanceHandler.QueryAsync<int>(contractAddress, message);
        return owner;
    }

    [Function("balanceOf", "uint256")]
    public class Erc1155BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public string Account { get; set; } = "";

        [Parameter("uint256", "id", 2)]
        public BigInteger Id { get; set; }
    }

    public async Task<int> Erc1155BalanceOf(string userAddress, string contractAddress, long tokenId)
    {
        var message = new Erc1155BalanceOfFunction()
        {
            Account = userAddress,
            Id = tokenId,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var balanceHandler = web3.Eth.GetContractQueryHandler<Erc1155BalanceOfFunction>();
        var balance = await balanceHandler.QueryAsync<int>(contractAddress, message);
        _logger.LogInformation("erc1155 balance of, {}, {}, {}", tokenId, balance, message);
        return balance;
    }

    [Function("isApprovedForAll", "bool")]
    public class Erc1155IsApprovedForAllFunction : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public string Account { get; set; } = "";

        [Parameter("address", "operator", 2)]
        public string Operator { get; set; } = "";
    }

    public async Task<bool> Erc1155IsApprovedForAll(string userAddress, string operatorAddress, string contractAddress)
    {
        var message = new Erc1155IsApprovedForAllFunction()
        {
            Account = userAddress,
            Operator = operatorAddress,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var handler = web3.Eth.GetContractQueryHandler<Erc1155IsApprovedForAllFunction>();
        var result = await handler.QueryAsync<bool>(contractAddress, message);
        return result;
    }

    [Function("setApprovalForAll")]
    public class Erc1155SetApprovalForAllFunction : FunctionMessage
    {
        [Parameter("address", "operator", 1)]
        public string Operator { get; set; } = "";

        [Parameter("bool", "approved", 2)]
        public bool Approved { get; set; }
    }

    public async Task<string> Erc1155SetApprovalForAll(string operatorAddress, bool approved, string contractAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract<Erc1155SetApprovalForAllFunction>(contractAddress);
        var callsFunction = contract.GetFunction("setApprovalForAll");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            operatorAddress,
            approved
        );
        return receipt.TransactionHash.ToString();
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

    public async Task<decimal> GetAllowance(string payAddr, string recvAddr, ERC20Contract contract)
    {
        var balanceInWei = await GetAllowance(payAddr, recvAddr, contract.Address);
        return Web3.Convert.FromWei(balanceInWei, contract.Decimals);
    }

    [Function("getApproved", "address")]
    public class GetApprovedFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public BigInteger TokenId { get; set; }
    }

    public async Task<string> Erc721GetApproved(long tokenId, string nftContract)
    {
        var message = new GetApprovedFunction()
        {
            TokenId = tokenId,
        };

        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var handler = web3.Eth.GetContractQueryHandler<GetApprovedFunction>();
        return await handler.QueryAsync<string>(nftContract, message);
    }

    // 授权 lottery721Addr 可以拥有 tokenId
    public async Task<string> Erc721Approve(string lottery721Addr, long tokenId, string erc721Address)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        string erc721ApproveAbi = """[{"type":"function","name":"approve","inputs":[{"name":"to","type":"address","internalType":"address"},{"name":"tokenId","type":"uint256","internalType":"uint256"}],"outputs":[],"stateMutability":"nonpayable"}]""";
        var contract = web3.Eth.GetContract(erc721ApproveAbi, erc721Address);
        var callsFunction = contract.GetFunction("approve");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, lottery721Addr, tokenId);
        return receipt.TransactionHash.ToString();
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

    [Event("Betted")]
    public class SicboBettedEventDTO : IEventDTO
    {

        [Parameter("address", "player", 1, true)]
        public string Player { get; set; } = "";

        [Parameter("uint256", "tokenId", 2, true)]
        public BigInteger TokenId { get; set; }

        [Parameter("bool", "isPlayerWin", 3, false)]
        public bool IsPlayerWin { get; set; }

        [Parameter("uint256", "diceNumbers", 4, false)]
        public BigInteger DiceNumbers { get; set; }

        [Parameter("uint8", "result", 5, false)]
        public short DiceResult { get; set; }
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
    public async Task<string> Erc20Approve(string nftAddr, BigInteger wad, string erc20Contract)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        string erc20ApproveAbi = """[{"inputs":[{"internalType":"address","name":"guy","type":"address"},{"internalType":"uint256","name":"wad","type":"uint256"}],"name":"approve","outputs":[{"internalType":"bool","name":"","type":"bool"}],"stateMutability":"nonpayable","type":"function"}]""";
        var contract = web3.Eth.GetContract(erc20ApproveAbi, erc20Contract);
        var callsFunction = contract.GetFunction("approve");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, nftAddr, wad);
        return receipt.TransactionHash.ToString();
    }

    [FunctionOutput]
    public class BarterTokenMetadataOutputDTO : IFunctionOutputDTO
    {
        [Parameter("address", "writer", 1)]
        public string Writer { get; set; } = "";

        [Parameter("uint64", "maturityDate", 2)]
        public long MaturityDate { get; set; }

        [Parameter("uint8", "baseAssetKind", 3)]
        public short BaseAssetKind { get; set; }

        [Parameter("address", "baseAssetAddress", 4)]
        public string BaseAssetAddress { get; set; } = "";

        [Parameter("uint256", "baseAssetAmount", 5)]
        public BigInteger BaseAssetAmount { get; set; }

        [Parameter("uint256", "baseAssetTokenId", 6)]
        public BigInteger BaseAssetTokenId { get; set; }

        [Parameter("uint8", "quoteAssetKind", 7)]
        public short QuoteAssetKind { get; set; }

        [Parameter("address", "quoteAssetAddress", 8)]
        public string QuoteAssetAddress { get; set; } = "";

        [Parameter("uint256", "quoteAssetAmount", 9)]
        public BigInteger QuoteAssetAmount { get; set; }

        [Parameter("uint256", "quoteAssetTokenId", 10)]
        public BigInteger QuoteAssetTokenId { get; set; }
    }

    public async Task<BarterTokenMetadataOutputDTO> GetBarterTokenMetadata(long tokenId, string contractAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, contractAddress);
        var callsFunction = contract.GetFunction("tokenMetadata");

        var output = await callsFunction.CallDeserializingToObjectAsync<BarterTokenMetadataOutputDTO>(tokenId);
        return output;
    }

    public async Task<long> MintBarter_FT2FT(string baseAssetAddress, BigInteger baseAssetAmount, string quoteAssetAddress, BigInteger quoteAssetAmount, long maturityUnix, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, nftAddress);
        var callsFunction = contract.GetFunction("mint_ft2ft");
        var value = DefaultFee;
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetAmount,
            quoteAssetAddress,
            quoteAssetAmount,
            maturityUnix
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return events.Where(item => item.Event.To.Equals(_metamaskHostProvider.SelectedAccount, StringComparison.CurrentCultureIgnoreCase)).Select(item => (long)item.Event.TokenId).First();
    }

    public async Task<long> MintBarter_NFT2NFT(string baseAssetAddress, BigInteger baseAssetId, string quoteAssetAddress, long maturityUnix, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, nftAddress);
        var callsFunction = contract.GetFunction("mint_nft2nft");
        var value = DefaultFee;
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetId,
            quoteAssetAddress,
            maturityUnix
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return events.Where(item => item.Event.To.Equals(_metamaskHostProvider.SelectedAccount, StringComparison.CurrentCultureIgnoreCase)).Select(item => (long)item.Event.TokenId).First();
    }

    public async Task<ulong> ExerciseBarter_FT2FT(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, nftAddress);
        var callsFunction = contract.GetFunction("exercise_ft2ft");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId
        );
        return receipt.Status.ToUlong();
    }

    public async Task<ulong> ExerciseBarter_NFT2NFT(long tokenId, long assetTokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, nftAddress);
        var callsFunction = contract.GetFunction("exercise_nft2nft");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            assetTokenId
        );
        return receipt.Status.ToUlong();
    }

    public async Task<string> BurnBarter(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(BarterABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }


    public async Task<long> MintLottery20(string baseAssetAddress, BigInteger baseAssetAmount, long maturityUnix, int amount, decimal defaultPrice, int soldPercent, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("mint20");
        var value = DefaultFee;
        var priceInWei = Web3.Convert.ToWei(defaultPrice);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetAmount,
            maturityUnix,
            amount,
            priceInWei,
            soldPercent * 100
        );
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<long> MintLottery721(string baseAssetAddress, BigInteger baseAssetId, long maturityUnix, int amount, decimal defaultPrice, int soldPercent, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("mint721");
        var value = DefaultFee;
        var priceInWei = Web3.Convert.ToWei(defaultPrice);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetId,
            maturityUnix,
            amount,
            priceInWei,
            soldPercent * 100
        );
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<long> MintLottery1155(string baseAssetAddress, BigInteger baseAssetId, long baseAssetAmount, long maturityUnix, int amount, decimal defaultPrice, int soldPercent, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("mint1155");
        var value = DefaultFee;
        var priceInWei = Web3.Convert.ToWei(defaultPrice);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetId,
            baseAssetAmount,
            maturityUnix,
            amount,
            priceInWei,
            soldPercent * 100
        );
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    [Event("Drawed")]
    public class DrawedEventDTO : IEventDTO
    {

        [Parameter("address", "winner", 1, true)]
        public string Winner { get; set; } = "";

        [Parameter("uint256", "tokenId", 2, true)]
        public BigInteger TokenId { get; set; }
    }

    // return winner address
    public async Task<string> DrawLottery(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("draw");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        var events = receipt.DecodeAllEvents<DrawedEventDTO>();
        return events.First().Event.Winner;
    }

    public async Task<string> BuyLottery(long tokenId, int amount, decimal defaultPrice, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("buy");
        var priceInWei = Web3.Convert.ToWei(defaultPrice);
        var value = new HexBigInteger(priceInWei * amount);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId, amount);
        return receipt.TransactionHash.ToString();
    }

    public async Task<bool> CanDrawLottery(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("canDraw");
        var result = await callsFunction.CallAsync<bool>(tokenId);
        return result;
    }

    [FunctionOutput]
    public class LotteryTokenMetadataOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "drawTime", 1)]
        public long DrawTime { get; set; }

        [Parameter("address", "writer", 2)]
        public string Writer { get; set; } = "";

        [Parameter("uint256", "tokenCount", 3)]
        public long TokenCount { get; set; }

        [Parameter("bool", "isDrawed", 4)]
        public bool IsDrawed { get; set; }

        [Parameter("uint256", "assetKind", 5)]
        public int AssetKind { get; set; }

        [Parameter("uint256", "defaultPrice", 6)]
        public BigInteger DefaultPrice { get; set; }

        [Parameter("uint256", "soldBp", 7)]
        public int SoldBp { get; set; }

        [Parameter("address", "baseAssetAddress", 8)]
        public string BaseAssetAddress { get; set; } = "";

        [Parameter("uint256", "baseAssetAmount", 9)]
        public BigInteger BaseAssetAmount { get; set; }

        [Parameter("uint256", "baseAssetTokenId", 10)]
        public BigInteger BaseAssetTokenId { get; set; }
    }

    public async Task<LotteryTokenMetadataOutputDTO> GetLotteryTokenMetadata(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(LotteryABI, nftAddress);
        var callsFunction = contract.GetFunction("tokenMetadata");
        var result = await callsFunction.CallAsync<LotteryTokenMetadataOutputDTO>(tokenId);
        return result;
    }

    // mint a red envelope
    public async Task<long> MintRedEnvelope20(string baseAssetAddress, BigInteger baseAssetAmount, int amount, RedEnvelopeKind kind, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("mint20");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetAmount,
            amount,
            kind
        );
        var events = receipt.DecodeAllEvents<TransferSingleEventDTO>();
        return (long)events.First().Event.Id;
    }

    public async Task<long> MintRedEnvelope1155(string baseAssetAddress, BigInteger baseAssetId, BigInteger baseAssetAmount, int amount, RedEnvelopeKind kind, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RedEnvelopeABI, nftAddress);
        var callsFunction = contract.GetFunction("mint1155");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetId,
            baseAssetAmount,
            amount,
            kind
        );
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
    public async Task<bool> UserBettedRoulette(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bettedAddressMap");
        var result = await callsFunction.CallAsync<bool>(tokenId, _metamaskHostProvider.SelectedAccount);
        return result;
    }

    public async Task<List<BigInteger>> GetTokenBetAmounts(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("getTokenBetAmounts");
        try
        {
            var result = await callsFunction.CallAsync<List<BigInteger>>(tokenId);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("error, {}", e);
            return [];
        }
    }

    public async Task<List<string>> GetTokenBetPlayers(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("getTokenBetPlayers");
        try
        {
            var result = await callsFunction.CallAsync<List<string>>(tokenId);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("error, {}", e);
            return [];
        }
    }

    public async Task<long> MintRoulette20(string erc20Address, BigInteger erc20Amount, int ownerFeeBp, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint20");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            erc20Address,
            erc20Amount,
            ownerFeeBp
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<long> MintRoulette721(string baseAssetAddress, BigInteger baseAssetTokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint721");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetTokenId
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<long> MintRoulette1155(string baseAssetAddress, BigInteger baseAssetTokenId, BigInteger baseAssetAmount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint1155");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            baseAssetAddress,
            baseAssetTokenId,
            baseAssetAmount
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<string> BetRoulette20(long tokenId, BigInteger baseAssetAmount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bet20");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            baseAssetAmount
        );
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> BetRoulette721(long tokenId, BigInteger baseAssetId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bet721");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            baseAssetId
        );
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> BetRoulette1155(long tokenId, BigInteger baseAssetId, long baseAssetAmount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("bet1155");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            baseAssetId,
            baseAssetAmount
        );
        return receipt.TransactionHash.ToString();
    }

    [Event("Opened")]
    public class OpenedEventDTO : IEventDTO
    {

        [Parameter("address", "winner", 1, true)]
        public string Winner { get; set; } = "";

        [Parameter("uint256", "tokenId", 2, true)]
        public BigInteger TokenId { get; set; }
    }

    public async Task<string> OpenRoulette(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(RouletteABI, nftAddress);
        var callsFunction = contract.GetFunction("open");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(callsFunction, _metamaskHostProvider.SelectedNetworkChainId, _metamaskHostProvider.SelectedAccount, value, tokenId);
        // return winner address
        var events = receipt.DecodeAllEvents<OpenedEventDTO>();
        return events.First().Event.Winner;
    }

    // Sicbo
    public async Task<long> MintSicbo(string erc20Address, BigInteger erc20Amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(SicboABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            erc20Address,
            erc20Amount
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<string> BurnSicbo(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(SicboABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> DepositSicbo(long tokenId, BigInteger erc20Amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(SicboABI, nftAddress);
        var callsFunction = contract.GetFunction("deposit");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            erc20Amount
        );
        return receipt.TransactionHash.ToString();
    }

    public async Task<(bool, int)> BetSicbo(long tokenId, BigInteger erc20Amount, int result, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(SicboABI, nftAddress);
        var callsFunction = contract.GetFunction("placeBet");
        var value = new HexBigInteger(0);
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            null,
            value,
            tokenId,
            erc20Amount,
            result
        );
        gas.Value *= 2; // 此处gas估计不准，容易out of gas，所以加倍
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            new HexBigInteger(2),
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            null,
            null,
            tokenId,
            erc20Amount,
            result
        );
        var events = receipt.DecodeAllEvents<SicboBettedEventDTO>();
        _logger.LogInformation("event length is {}", events.Count);
        return (events.First().Event.IsPlayerWin, (int)events.First().Event.DiceNumbers);
    }

    // Vote
    public async Task<long> MintVote(string title, IList<string> choices, DateTime expireTime, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(VoteABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        long ts = new DateTimeOffset(expireTime).ToUnixTimeSeconds();
        var abiEncode = new ABIEncode();
        var choiceData = abiEncode.GetABIEncoded(choices.ToArray());
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            title,
            choiceData,
            choices.Count,
            ts
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<string> BurnVote(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(VoteABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    public async Task<string> PlaceVote(long tokenId, int answer, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(VoteABI, nftAddress);
        var callsFunction = contract.GetFunction("vote");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId,
            answer
        );
        return receipt.TransactionHash.ToString();
    }


    public async Task<bool> UserVoted(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(VoteABI, nftAddress);
        var callsFunction = contract.GetFunction("tokenVoter");
        var result = await callsFunction.CallAsync<bool>(tokenId, _metamaskHostProvider.SelectedAccount);
        return result;
    }

    // Writing
    public async Task<long> MintWriting(string title, string content, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(WritingABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = DefaultFee; // 0.001 fee
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            title,
            content
        );
        var events = receipt.DecodeAllEvents<TransferEventDTO>();
        return (long)events.First().Event.TokenId;
    }

    public async Task<string> BurnWriting(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(WritingABI, nftAddress);
        var callsFunction = contract.GetFunction("burn");
        var value = new HexBigInteger(0);
        var receipt = await SendTransactionThroughMetamask(
            callsFunction,
            _metamaskHostProvider.SelectedNetworkChainId,
            _metamaskHostProvider.SelectedAccount,
            value,
            tokenId
        );
        return receipt.TransactionHash.ToString();
    }

    [FunctionOutput]
    public class WritingTokenMetadataOutputDTO : IFunctionOutputDTO
    {
        [Parameter("string", "title", 1)]
        public string Title { get; set; } = "";

        [Parameter("string", "content", 2)]
        public string Content { get; set; } = "";

        [Parameter("address", "writer", 3)]
        public string Writer { get; set; } = "";

    }

    public async Task<string> GetWritingContent(long tokenId, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(WritingABI, nftAddress);
        var callsFunction = contract.GetFunction("tokenMetadata");
        var result = await callsFunction.CallAsync<WritingTokenMetadataOutputDTO>(tokenId);
        return result.Content;
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


public enum DiceResult
{
    Small = 0,
    Big,

    SpecificDouble_1, // 对1
    SpecificDouble_2,
    SpecificDouble_3,
    SpecificDouble_4,
    SpecificDouble_5,
    SpecificDouble_6,

    SpecificTriple_1, // 三条1
    SpecificTriple_2,
    SpecificTriple_3,
    SpecificTriple_4,
    SpecificTriple_5,
    SpecificTriple_6,

    AnyTriple, // 三同

    ThreeDiceTotal_4, // 三个骰子的点数之和为4
    ThreeDiceTotal_5, // 三个骰子的点数之和为5
    ThreeDiceTotal_6, // 三个骰子的点数之和为6
    ThreeDiceTotal_7, // 三个骰子的点数之和为7
    ThreeDiceTotal_8, // 三个骰子的点数之和为8
    ThreeDiceTotal_9, // 三个骰子的点数之和为9
    ThreeDiceTotal_10, // 三个骰子的点数之和为10
    ThreeDiceTotal_11, // 三个骰子的点数之和为11
    ThreeDiceTotal_12, // 三个骰子的点数之和为12
    ThreeDiceTotal_13, // 三个骰子的点数之和为13
    ThreeDiceTotal_14, // 三个骰子的点数之和为14
    ThreeDiceTotal_15, // 三个骰子的点数之和为15
    ThreeDiceTotal_16, // 三个骰子的点数之和为16
    ThreeDiceTotal_17, // 三个骰子的点数之和为17

    TwoDice_12, // 中两个骰子
    TwoDice_13,
    TwoDice_14,
    TwoDice_15,
    TwoDice_16,
    TwoDice_23,
    TwoDice_24,
    TwoDice_25,
    TwoDice_26,
    TwoDice_34,
    TwoDice_35,
    TwoDice_36,
    TwoDice_45,
    TwoDice_46,
    TwoDice_56,

    OneDice_1, // 中1个骰子
    OneDice_2, // 中2个骰子
    OneDice_3, // 中3个骰子
    OneDice_4,
    OneDice_5,
    OneDice_6
}
