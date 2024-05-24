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

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
