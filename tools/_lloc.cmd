@echo off

set _msbuildPath="C:\Program Files\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin"

%_msbuildPath%\msbuild "..\src\CommandLine.sln" /t:Build /p:Configuration=Debug /v:m /m

"..\src\CommandLine\bin\Debug\net48\roslynator" lloc "..\src\Roslynator.sln" ^
 --msbuild-path %_msbuildPath% ^
 --verbosity d ^
 --file-log "roslynator.log" ^
 --file-log-verbosity diag

pause
