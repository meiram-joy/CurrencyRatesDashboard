using Microsoft.JSInterop;

namespace CurrencyRates.Infrastructure.Services.Export;

public class FileUtil
{
    private readonly IJSRuntime _jsRuntime;

    public FileUtil(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveAs(string filename, string contentType, byte[] data)
    {
        await _jsRuntime.InvokeVoidAsync(
            "saveAsFile",
            filename,
            contentType,
            Convert.ToBase64String(data));
    }
}