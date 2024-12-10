# how to monkey patch dotnet SDK with modified dotnet runtime

# install SDK that you can corrupt

https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.403-windows-x64-binaries

and unpack it to local folder, in my case `c:\Dev\.dotnet\`

```sh
$env:DOTNET_ROOT="C:/Dev/.dotnet2/"
$env:PATH="C:/Dev/.dotnet2/;$env:PATH"
where.exe dotnet
C:/Dev/.dotnet2/dotnet --info
```

# locate runtime pack on your system by building the target application

```sh
# make sure that you have wasm workload
dotnet workload restore

# find out where are the runtime pack bits on your system
# by building your project with binlog
dotnet build -bl:net8.binlog
```

Open `net8.binlog` using [Binary Log Viewer](https://msbuildlog.com/)

And search for `Microsoft.NETCore.App.Runtime.Mono.browser-wasm` to find the directory on your system.
In my case that's `PackageDirectory = c:\Dev\.dotnet\packs\Microsoft.NETCore.App.Runtime.Mono.browser-wasm\8.0.10`

# build your own runtime pack

Clone and checkout the [custom branch](https://github.com/dotnet/runtime/compare/release/8.0-staging...pavelsavara:runtime:wasm-heapshot-8?expand=1)
```sh
git clone git@github.com:pavelsavara/runtime.git
git checkout --track origin/wasm-heapshot-8
```

Make sure that you are targeting the same major release. In my case it's Net8 `8.0.10`.
See `<ProductVersion>` in `eng\Versions.props` 

And compile your branch
```sh
./build.sh -os browser -c Release
```

Confirm that `artifacts\packages\Release\Shipping\Microsoft.NETCore.App.Runtime.Mono.browser-wasm.8.0.10.nupkg` was created.
Rename `Microsoft.NETCore.App.Runtime.Mono.browser-wasm.8.0.10.nupkg` to `.zip` so that you could unpack it later.
Rename `Microsoft.NET.Runtime.WebAssembly.Sdk.8.0.10.nupkg` to `.zip` so that you could unpack it later.

# replace the runtime pack in the SDK installation

- make backup of `packs\Microsoft.NETCore.App.Runtime.Mono.browser-wasm\8.0.10` in your SDK folder
- rename `Microsoft.NETCore.App.Runtime.Mono.browser-wasm\8.0.10` to `8.0.10-original`
- create empty folder `8.0.10` in place of the original
- unpack `Microsoft.NETCore.App.Runtime.Mono.browser-wasm.8.0.10.nupkg` into `8.0.10`
- validate that it contains the expected change. In my case it's the `heapshot.c` file in `8.0.10\runtimes\browser-wasm\native\src\heapshot.c`

- make backup of `packs\Microsoft.NET.Runtime.WebAssembly.Sdk\8.0.10` in your SDK folder
- rename `Microsoft.NET.Runtime.WebAssembly.Sdk\8.0.10` to `8.0.10-original`
- create empty folder `8.0.10` in place of the original
- unpack `Microsoft.NET.Runtime.WebAssembly.Sdk.8.0.10.nupkg` into `8.0.10`
- validate that it contains `heapshot.c` file in `Sdk\WasmApp.Native.targets`

#update the project
```xml
    <PropertyGroup>
        <WasmBuildNative>true</WasmBuildNative>
        <WasmNativeStrip>false</WasmNativeStrip>
    </PropertyGroup>
```

# re-build the target application with the custom runtime pack
```sh
dotnet clean
dotnet build -bl:net8-patched.binlog
```

# test the app
```js
getDotnetRuntime(0).INTERNAL.mono_wasm_perform_heapshot()
```
