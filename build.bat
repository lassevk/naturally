@echo off

setlocal

if exist *.nupkg del *.nupkg
if errorlevel 1 goto error

if exist *.snupkg del *.snupkg
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

set SUFFIX=
for /f %%f in ('git rev-parse --abbrev-ref HEAD') do set GITBRANCH=%%f

if not "%GITBRANCH%" == "master" (
    set SUFFIX=beta
)

dotnet restore
if errorlevel 1 goto error

dotnet build /p:VERSIONSUFFIX=%SUFFIX%
if errorlevel 1 goto error

dotnet test /p:VERSIONSUFFIX=%SUFFIX%
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin\Debug\*.nupkg" move "%%f\bin\Debug\*.nupkg" .\
    if errorlevel 1 goto error
    if exist "%%f\bin\Debug\*.snupkg" move "%%f\bin\Debug\*.snupkg" .\
    if errorlevel 1 goto error
    if exist "%%f\bin\Release\*.nupkg" move "%%f\bin\Release\*.nupkg" .\
    if errorlevel 1 goto error
    if exist "%%f\bin\Release\*.snupkg" move "%%f\bin\Release\*.snupkg" .\
    if errorlevel 1 goto error
)

goto end

:error
exit /B 1

:end
