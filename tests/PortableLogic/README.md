# CSV import regressions

Run from the repository root with .NET SDK 10:

```sh
dotnet run --project tests/PortableLogic/PortableLogic.csproj --configuration Release
```

Links the actual BetterParserEngine, CsvParser and IParserEngine source. Covers empty fields, final quoted fields, escaped quotes, custom separators, CRLF endings, malformed quotes, dictionary mapping and concurrent calls on one engine. Includes the tab-separated import format used by MainPage. WinRT async operations are minimal Task adapters; cancellation, COM and the Windows UI are not emulated. No Azure writes or HTTP requests are made.

The harness has no NuGet dependencies, clears package sources, compiles with C# 5 syntax, and does not publish anything. CI runs the same command with read-only repository permissions and commit-pinned actions. A passing host test is **not a build or runtime validation of the historical application**. Application projects and their original Windows Store XAML/WinRT and Windows Phone 7.1 targets remain unchanged.

## Dependency and application validation still needed

The original package manifests, checked-in libraries, and application project references are intentionally unchanged. This PR does not resolve their security or support status. A manifest-only update would not update the old DLL HintPaths or validate compatibility.

- Newtonsoft.Json 13.0.4 lists desktop .NET and .NET Standard assets, with some Phone/Store compatibility computed by NuGet. Windows Phone 7.1 is not listed; desktop net40 must not be confused with Silverlight v4.0. [Package frameworks](https://www.nuget.org/packages/Newtonsoft.Json/13.0.4#supportedframeworks-body-tab)
- RestSharp 106.12.0, proposed by existing bot PRs in some of these projects, is a separate compatibility investigation, not a proven Phone 7.1 replacement. [Package frameworks](https://www.nuget.org/packages/RestSharp/106.12.0#supportedframeworks-body-tab)
- Reproduce the original app build with the matching Phone/Windows Store SDK and existing licensed SDK references before selecting per-target upgrades. [Microsoft compatibility guidance](https://learn.microsoft.com/en-us/visualstudio/releases/2022/compatibility)

Compatible next steps are to keep the original app targets and test candidate package assets in the historical SDK, or make a separately approved platform migration plan. This portable harness does not require that migration decision.

## CSV limits

This fixes single-record field parsing; record splitting remains newline-based. Embedded newlines in quoted fields, quoted header names and headerless dictionaries are not implemented by this change. Invalid unterminated quotes or text after a closing quote now fail explicitly with FormatException instead of truncating silently. Blank records remain ignored. The live import service and its current data have not been contacted.
