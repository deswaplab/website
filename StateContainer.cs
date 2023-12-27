public class StateContainer
{
    private string? savedSelectedAccount;

    public string SelectedAccount
    {
        get => savedSelectedAccount ?? string.Empty;
        set
        {
            savedSelectedAccount = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
