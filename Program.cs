using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics;

// We only need this to make sure the dll's are not trimmed.
if (Debugger.IsAttached)
{
    var builder = WebAssemblyHostBuilder.CreateDefault();
    await builder.Build().RunAsync();
}
