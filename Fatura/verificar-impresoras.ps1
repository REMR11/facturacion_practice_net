# Script para verificar impresoras instaladas
# Ejecutar en PowerShell

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "  VERIFICADOR DE IMPRESORAS" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Buscando impresoras instaladas..." -ForegroundColor Yellow
Write-Host ""

$printers = Get-Printer | Select-Object Name, DriverName, PortName, PrinterStatus

if ($printers.Count -eq 0) {
    Write-Host "No se encontraron impresoras instaladas." -ForegroundColor Red
    exit
}

Write-Host "Impresoras encontradas:" -ForegroundColor Green
Write-Host ""

$index = 1
foreach ($printer in $printers) {
    Write-Host "[$index] $($printer.Name)" -ForegroundColor White
    Write-Host "    Driver: $($printer.DriverName)" -ForegroundColor Gray
    Write-Host "    Puerto: $($printer.PortName)" -ForegroundColor Gray
    Write-Host "    Estado: $($printer.PrinterStatus)" -ForegroundColor Gray
    Write-Host ""
    $index++
}

Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Buscar impresoras térmicas
$thermalPrinters = $printers | Where-Object { 
    $_.Name -like "*RPT*" -or 
    $_.Name -like "*POS*" -or 
    $_.Name -like "*Thermal*" -or
    $_.Name -like "*80mm*"
}

if ($thermalPrinters) {
    Write-Host "Impresoras térmicas detectadas:" -ForegroundColor Green
    foreach ($tp in $thermalPrinters) {
        Write-Host "  ✓ $($tp.Name)" -ForegroundColor Green
    }
} else {
    Write-Host "⚠ No se detectaron impresoras térmicas automáticamente." -ForegroundColor Yellow
    Write-Host "  Si tienes una impresora térmica, renómbrala a 'RPT004'" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Preguntar si quiere renombrar
$rename = Read-Host "¿Deseas renombrar alguna impresora a 'RPT004'? (S/N)"

if ($rename -eq "S" -or $rename -eq "s") {
    $printerNumber = Read-Host "Ingresa el número de la impresora que deseas renombrar"
    
    try {
        $selectedPrinter = $printers[$printerNumber - 1]
        $oldName = $selectedPrinter.Name
        
        Write-Host ""
        Write-Host "Renombrando '$oldName' a 'RPT004'..." -ForegroundColor Yellow
        
        # Renombrar impresora
        Rename-Printer -Name $oldName -NewName "RPT004"
        
        Write-Host "✓ Impresora renombrada exitosamente!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Ahora puedes usar el botón 'Imprimir Ticket' en la aplicación." -ForegroundColor Cyan
    }
    catch {
        Write-Host "✗ Error al renombrar la impresora: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "  Asegúrate de ejecutar PowerShell como Administrador" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Presiona cualquier tecla para salir..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
