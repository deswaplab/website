using System.Numerics;
using Nethereum.Web3;

namespace DeswapApp;

public interface INftFetcher
{
    Task<IList<UserToken>> GetUserOptionTokens(string userAddress, long chainId);
}

public class UserToken
{
    public long TokenId { get; set; }

    public long ChainId { get; set; }

    public required string Contract { get; set; }

    public required string ImageData { get; set; }

    public DateTimeOffset MaturityDate { get; set; }

    public OptionsKind OptionsKind { get; set; }

    public Decimal BaseAssetAmount { get; set; }

    public Decimal QuoteAssetAmount { get; set; }

    public Decimal Price { get; set; }

    public bool Listable()
    {
        if (!Burnable() && !Exercisable())
        {
            return true;
        }
        return false;
    }

    public bool Burnable()
    {
        var now = DateTime.Now;
        if (now >= MaturityDate.AddDays(1))
        {
            return true;
        }
        return false;
    }

    public bool Exercisable()
    {
        var now = DateTime.Now;
        // TODO: 方便测试暂且注释，上线时解除
        // if (now >= MaturityDate && now < MaturityDate.AddDays(1))
        // {
        //     return true;
        // }
        if (now < MaturityDate.AddDays(1))
        {
            return true;
        }
        return false;
    }

    public (string, BigInteger) GetPayAsset()
    {
        var tokenPair = TokenPairs.FilterByChainId(ChainId)
            .Where(item => item.NftAddress.Equals(Contract, StringComparison.CurrentCultureIgnoreCase))
            .First();
        if (OptionsKind == OptionsKind.CALL)
        {
            return (tokenPair.QuoteAssetAddress, Web3.Convert.ToWei(QuoteAssetAmount, tokenPair.QuoteAssetDecimals));
        }
        else if (OptionsKind == OptionsKind.PUT)
        {
            return (tokenPair.BaseAssetAddress, Web3.Convert.ToWei(BaseAssetAmount, tokenPair.BaseAssetDecimals));
        }
        throw new Exception("invalid token");
    }
}
