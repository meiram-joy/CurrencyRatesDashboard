using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace CurrencyRates.Infrastructure.Services.Export;

public class FileUtil
{
    public static async Task SaveAs(string filename, string contentType, byte[] data)
    {
        var jsRuntime = GetJsRuntime();
        await jsRuntime.InvokeAsync<object>(
            "saveAsFile",
            filename,
            contentType,
            Convert.ToBase64String(data));
    }

    private static IJSRuntime GetJsRuntime()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IJSRuntime, JSRuntime>()
            .BuildServiceProvider();
        return serviceProvider.GetRequiredService<IJSRuntime>();
    }
}