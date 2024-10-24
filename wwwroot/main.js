async function main() {
    /* alternatively, you can use the Blazor.start method to start the Blazor application
    
    await Blazor.start({
        applicationCulture: "nl-NL"
    });
    const runtime = getDotnetRuntime(0);
    
    */

    const { dotnet } = await import('./_framework/dotnet.js')
    const runtime = await dotnet
        .withEnvironmentVariable("MONO_LOG_LEVEL", "debug")
        .withEnvironmentVariable("MONO_LOG_MASK", "gc")
        .create();

    const library = await runtime.getAssemblyExports("BlazorMemoryInfo");
    const testClass = library.BlazorMemoryInfo.TestClass;

    console.log("Start allocating objects...");
    //testClass?.AllocateObjects(3890);
    testClass?.AllocateObjects(3790);
    console.log("Disposing allocated objects...");
    testClass?.DisposeObjects();
    testClass?.DisposeObjects();

    runtime.INTERNAL.mono_wasm_perform_heapshot();

}

main();