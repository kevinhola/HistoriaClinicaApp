# Script para preparar instalador
Write-Host "Preparando instalador..." -ForegroundColor Green

# Limpiar build anterior
Write-Host "Limpiando build anterior..." -ForegroundColor Yellow
dotnet clean

# Publicar aplicación
Write-Host "Publicando aplicación..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=false

# Verificar publicación
$publishPath = "..\bin\Release\net6.0-windows\publish"
if (Test-Path $publishPath) {
    Write-Host "Publicación exitosa en: $publishPath" -ForegroundColor Green
    
    # Compilar instalador con Inno Setup
    $innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
    
    if (Test-Path $innoSetupPath) {
        Write-Host "Compilando instalador..." -ForegroundColor Yellow
        & $innoSetupPath "setup.iss"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Instalador creado exitosamente en: .\Output\" -ForegroundColor Green
        } else {
            Write-Host "Error al compilar instalador" -ForegroundColor Red
        }
    } else {
        Write-Host "Inno Setup no encontrado. Descargue de: https://jrsoftware.org/isdl.php" -ForegroundColor Red
    }
} else {
    Write-Host "Error en la publicación" -ForegroundColor Red
}

Write-Host "Proceso completado." -ForegroundColor Green