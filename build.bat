@echo off

setlocal

if exist *.nupkg del *.nupkg
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin" rd /s /q "%%f\bin"
    if errorlevel 1 goto error
    if exist "%%f\obj" rd /s /q "%%f\obj"
    if errorlevel 1 goto error
)
if errorlevel 1 goto error

dotnet restore
if errorlevel 1 goto error

dotnet build
if errorlevel 1 goto error

dotnet test
if errorlevel 1 goto error

for /d %%f in (*.*) do (
    if exist "%%f\bin\Debug\*.nupkg" copy "%%f\bin\Debug\*.nupkg" .\
    if errorlevel 1 goto error
    if exist "%%f\bin\Release\*.nupkg" copy "%%f\bin\Release\*.nupkg" .\
    if errorlevel 1 goto error
)

goto end

:error
exit /B 1

:end
