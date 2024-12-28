# Steps to reproduce FsUnit 7.0.0 build issue

```powershell
# clear cache.
dotnet nuget locals all --clear

dotnet new xunit3 --language 'F#'
dotnet test --verbosity diagnostic # success
```

```plaintext
C:\Program Files\dotnet\sdk\9.0.101\MSBuild.dll -nologo --property:VsTestUseMSBuildOutput=true -property:VSTestVerbosity=diagnostic -property:VSTestArtifactsProcessingMode=collect -property:VSTestSessionCorrelationId=18208_16fd9990-a9e1-43b8-8ade-abeba2d9716f -distributedlogger:Microsoft.DotNet.Tools.MSBuild.MSBuildLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll*Microsoft.DotNet.Tools.MSBuild.MSBuildForwardingLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll -maxcpucount -restore -target:VSTest -tlp:default=auto -verbosity:m -verbosity:diagnostic .\fsunit7.fsproj
Restore complete (0.8s)
    Determining projects to restore...
    Restored C:\fsunit7\fsunit7.fsproj (in 337 ms).
  fsunit7 succeeded (3.2s) → bin\Debug\net9.0\fsunit7.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.0.0+e341b939fe (64-bit .NET 9.0.0)
[xUnit.net 00:00:01.84]   Discovering: fsunit7
[xUnit.net 00:00:02.07]   Discovered:  fsunit7
[xUnit.net 00:00:02.30]   Starting:    fsunit7
[xUnit.net 00:00:02.55]   Finished:    fsunit7
  fsunit7 test succeeded (3.8s)

Test summary: total: 1, failed: 0, succeeded: 1, skipped: 0, duration: 3.8s
Build succeeded in 8.8s
```

When adding FsUnit 7.0.0, the build fails.

```powershell
dotnet add package FsUnit.xUnit --version 7.0.0
dotnet test --verbosity diagnostic # failure
```

```plaintext
> dotnet test --verbosity diagnostic
C:\Program Files\dotnet\sdk\9.0.101\MSBuild.dll -nologo --property:VsTestUseMSBuildOutput=true -property:VSTestVerbosity=diagnostic -property:VSTestArtifactsProcessingMode=collect -property:VSTestSessionCorrelationId=18744_eacad426-d2f7-4b10-8fff-fd0d4dd7f7cf -distributedlogger:Microsoft.DotNet.Tools.MSBuild.MSBuildLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll*Microsoft.DotNet.Tools.MSBuild.MSBuildForwardingLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll -maxcpucount -restore -target:VSTest -tlp:default=auto -verbosity:m -verbosity:diagnostic .\fsunit7.fsproj
Restore complete (0.7s)
    Determining projects to restore...
    Restored C:\fsunit7\fsunit7.fsproj (in 379 ms).
  fsunit7 succeeded with 2 warning(s) (2.5s) → bin\Debug\net9.0\fsunit7.dll
    C:\Program Files\dotnet\sdk\9.0.101\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3246: Resolved file has a bad image, no metadata, or is otherwise inaccessible. PE image does not have metadata. PE image does not have metadata.
    C:\Program Files\dotnet\sdk\9.0.101\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3243: No way to resolve conflict between "FsUnit.Xunit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" and "FsUnit.Xunit". Choosing "FsUnit.Xunit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" arbitrarily.
Error:
  An assembly specified in the application dependencies manifest (fsunit7.deps.json) has already been found but with a different file extension:
    package: 'FsUnit.xUnit', version: '7.0.0'
    path: 'lib/net6.0/FsUnit.Xunit.exe'
    previously found assembly: 'C:\fsunit7\bin\Debug\net9.0\FsUnit.Xunit.dll'
Testhost process for source(s) 'C:\fsunit7\bin\Debug\net9.0\fsunit7.dll' exited with error: Error:
  An assembly specified in the application dependencies manifest (fsunit7.deps.json) has already been found but with a different file extension:
    package: 'FsUnit.xUnit', version: '7.0.0'
    path: 'lib/net6.0/FsUnit.Xunit.exe'
    previously found assembly: 'C:\fsunit7\bin\Debug\net9.0\FsUnit.Xunit.dll'
. Please check the diagnostic logs for more information.
  fsunit7 test failed with 1 error(s) (0.5s)
    C:\fsunit7\bin\Debug\net9.0\fsunit7.dll : error TESTRUNABORT: Test Run Aborted.

Build failed with 1 error(s) and 2 warning(s) in 4.3s
```

After manually removing `FsUnit.Xunit.exe` section from `.deps.json`, `dotnet test` works fine.

```diff
       "FsUnit.xUnit/7.0.0": {
         "dependencies": {
           "FSharp.Core": "9.0.100",
           "NHamcrest": "4.0.0",
           "xunit.v3": "1.0.0"
         },
         "runtime": {
           "lib/net6.0/FsUnit.Xunit.dll": {
             "assemblyVersion": "1.0.0.0",
             "fileVersion": "1.0.0.0"
-          },
-          "lib/net6.0/FsUnit.Xunit.exe": {
-            "fileVersion": "1.0.0.0"
           }
         }
       },
```

```plaintext
C:\Program Files\dotnet\sdk\9.0.101\MSBuild.dll -nologo --property:VsTestUseMSBuildOutput=true -property:VSTestVerbosity=diagnostic -property:VSTestArtifactsProcessingMode=collect -property:VSTestSessionCorrelationId=21048_91a7e8d0-bce6-4560-a143-3378d1997de5 -distributedlogger:Microsoft.DotNet.Tools.MSBuild.MSBuildLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll*Microsoft.DotNet.Tools.MSBuild.MSBuildForwardingLogger,C:\Program Files\dotnet\sdk\9.0.101\dotnet.dll -maxcpucount -restore -target:VSTest -tlp:default=auto -verbosity:m -verbosity:diagnostic .\fsunit7.fsproj
Restore complete (0.3s)
    Determining projects to restore...
    All projects are up-to-date for restore.
  fsunit7 succeeded with 2 warning(s) (0.4s) → bin\Debug\net9.0\fsunit7.dll
    C:\Program Files\dotnet\sdk\9.0.101\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3246: Resolved file has a bad image, no metadata, or is otherwise inaccessible. PE image does not have metadata. PE image does not have metadata.
    C:\Program Files\dotnet\sdk\9.0.101\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3243: No way to resolve conflict between "FsUnit.Xunit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" and "FsUnit.Xunit". Choosing "FsUnit.Xunit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" arbitrarily.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.0.0+e341b939fe (64-bit .NET 9.0.0)
[xUnit.net 00:00:01.68]   Discovering: fsunit7
[xUnit.net 00:00:01.88]   Discovered:  fsunit7
[xUnit.net 00:00:02.11]   Starting:    fsunit7
[xUnit.net 00:00:02.53]   Finished:    fsunit7
  fsunit7 test succeeded (3.7s)

Test summary: total: 2, failed: 0, succeeded: 2, skipped: 0, duration: 3.7s
Build succeeded with 2 warning(s) in 5.0s
```
