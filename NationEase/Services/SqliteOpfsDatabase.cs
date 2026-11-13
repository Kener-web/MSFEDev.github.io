using Microsoft.JSInterop;

namespace NationEase.Services;

public sealed class SqliteOpfsDatabase(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? module;
    private Task<IJSObjectReference>? moduleTask;

    public async Task InitializeAsync()
    {
        var jsModule = await GetModuleAsync();
        await jsModule.InvokeVoidAsync("initialize");
    }

    public async Task<List<T>> QueryAsync<T>(string sql, params object?[] parameters)
    {
        var jsModule = await GetModuleAsync();
        return await jsModule.InvokeAsync<List<T>>("query", sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, params object?[] parameters)
    {
        var jsModule = await GetModuleAsync();
        return await jsModule.InvokeAsync<int>("execute", sql, parameters);
    }

    public async Task<T?> InvokeAsync<T>(string operation, params object?[] arguments)
    {
        var jsModule = await GetModuleAsync();
        return await jsModule.InvokeAsync<T?>("invoke", operation, arguments);
    }

    public async ValueTask DisposeAsync()
    {
        if (module is not null)
        {
            await module.InvokeVoidAsync("dispose");
            await module.DisposeAsync();
        }
    }

    private Task<IJSObjectReference> GetModuleAsync()
    {
        moduleTask ??= ImportModuleAsync();
        return moduleTask;
    }

    private async Task<IJSObjectReference> ImportModuleAsync()
    {
        module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/sqliteDatabase.js");
        return module;
    }
}
