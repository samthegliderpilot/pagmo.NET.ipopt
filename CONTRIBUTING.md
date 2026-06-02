# Contributing to Pagmo.NET.Ipopt

## Prerequisites

- .NET 10 SDK
- `Pagmo.NET` 1.0.0-beta.6 (or build from source)
- vcpkg with `VCPKG_ROOT` set
- PowerShell 7+ (`pwsh`)

## Cloning

```powershell
git clone --recurse-submodules https://github.com/samthegliderpilot/pagmo.NET.ipopt
```

## Building the native layer

IPOPT is compiled into the base `PagmoWrapper.dll` via the `pagmoNet` submodule build script. The `ports/coin-or-ipopt/` overlay in this repo must be on the vcpkg overlay path.

```powershell
$env:VCPKG_ROOT = "C:\vcpkg"
pwsh pagmoNet/scripts/build-native.ps1 -Configuration Release
Copy-Item pagmoNet/native/win-build/PagmoWrapper.dll native/ -Force
```

## Building and testing

```powershell
dotnet build Pagmo.NET.Ipopt.csproj
dotnet test Pagmo.NET.Ipopt.csproj -p:Platform=x64
```

## Repo layout

| Path | Contents |
|---|---|
| `generated/` | SWIG-generated C# wrapper classes |
| `extensions/` | Hand-written C# extensions |
| `swig/ipopt.i` | SWIG interface file |
| `ports/coin-or-ipopt/` | vcpkg overlay port for COIN-OR IPOPT |
| `pagmoNet/` | Submodule — shared SWIG + native bridge |

## License

LGPL-2.1-or-later. See [LICENSE](LICENSE).  
IPOPT: EPL-2.0. See [NOTICE](NOTICE).
