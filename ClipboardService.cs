namespace DeswapApp;

using Microsoft.JSInterop;

public interface IClipboardService
{
    Task CopyToClipboard(string text);
}

public class ClipboardService(IJSRuntime jsInterop) : IClipboardService
{
    private readonly IJSRuntime _jsInterop = jsInterop;

    public async Task CopyToClipboard(string text)
    {
        await _jsInterop.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}
