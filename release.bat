@echo off

setlocal

set GITBRANCH=
for /f %%f in ('git rev-parse --abbrev-ref HEAD') do set GITBRANCH=%%f

if not "%GITBRANCH%" == "master" (
    echo Can only release from the 'master' branch
    exit /B 1
)

call build.bat
if errorlevel 1 goto error

echo=
echo================================================
set /P PUSHYESNO=Push packages to nuget? [y/N]
if "%PUSHYESNO%" == "Y" GOTO PUSH
if "%PUSHYESNO%" == "y" GOTO PUSH
exit /B 0

:PUSH
for /R %%f in (*.nupkg) do (
    nuget push "%%f" -Source https://api.nuget.org/v3/index.json
    if errorlevel 1 goto error
)
start "" "https://www.nuget.org/account/Packages"
goto end

:error
exit /B 1

:end
