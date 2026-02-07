@echo off
echo Ejecutando aplicación con captura de errores...
echo.

cd bin\Debug\net6.0-windows
HistoriaClinicaApp.exe

if %ERRORLEVEL% NEQ 0 (
    echo Error al ejecutar: Codigo %ERRORLEVEL%
    pause
) else (
    echo Aplicacion cerrada normalmente
)

pause