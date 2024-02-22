using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Metamask;

namespace DeswapApp;

public class Web3Service(MetamaskHostProvider metamaskHostProvider, ILogger<Web3Service> logger)
{
    private readonly string OptionsABI = """
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
            },
            {
                "internalType": "uint256",
                "name": "amount",
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
        "stateMutability": "payable",
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
    public async Task<string> MintOptions(BigInteger baseAssetAmount, BigInteger quoteAssetAmount, long maturityUnix, int amount, string nftAddress)
    {
        var web3 = await _metamaskHostProvider.GetWeb3Async();
        var contract = web3.Eth.GetContract(OptionsABI, nftAddress);
        var callsFunction = contract.GetFunction("mint");
        var value = new Nethereum.Hex.HexTypes.HexBigInteger(BigInteger.Parse("1000000000000000"));
        var gas = await callsFunction.EstimateGasAsync(
            _metamaskHostProvider.SelectedAccount,
            new Nethereum.Hex.HexTypes.HexBigInteger(0),
            value,
            baseAssetAmount, 
            quoteAssetAmount, 
            maturityUnix,
            amount
        );
        _logger.LogInformation($"Mint Options, gas={gas}, value={value}");
        var receipt = await callsFunction.SendTransactionAndWaitForReceiptAsync(
            _metamaskHostProvider.SelectedAccount,
            gas,
            value,
            CancellationToken.None,
            baseAssetAmount, 
            quoteAssetAmount, 
            maturityUnix,
            amount
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
}
