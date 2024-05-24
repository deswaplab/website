namespace DeswapApp;

public class StateContainer
{
    private IList<UserNftBase> CachedNftsInner { get; set; } = [];

    public IList<UserNftBase> CachedNfts
    {
        get => CachedNftsInner;
        set
        {
            CachedNftsInner = value;
            NotifyStateChanged();
        }
    }

    private Dictionary<string, IList<UserNftBase>> CachedContractNftsInner { get; set; } = [];

    public IList<UserNftBase> GetContractNfts(string contractAddress)
    {
        if (CachedContractNftsInner.TryGetValue(contractAddress, out IList<UserNftBase>? value))
        {
            return value ?? [];
        }
        return [];
    }

    public void SetContractNfts(string contractAddress, IList<UserNftBase> value)
    {
        CachedContractNftsInner[contractAddress] = value;
        NotifyStateChanged();
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
