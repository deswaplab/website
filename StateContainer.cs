namespace DeswapApp;


public class StateContainer
{
    private string? selectedAccount;

    public string SelectedAccount
    {
        get => selectedAccount ?? string.Empty;
        set
        {
            selectedAccount = value;
            NotifyStateChanged();
        }
    }

    private long selectedChainId;

    public long SelectedChainId
    {
        get => selectedChainId;
        set
        {
            selectedChainId = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
