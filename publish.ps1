# Red Bike Path - Publish Script
# Vereinfachtes Skript für dotnet publish

Write-Host "`n?? Publishing Red Bike Path Mod...`n" -ForegroundColor Cyan

$projectPath = "Red bike path\Red bike path.csproj"

# Führe dotnet publish mit SkipModPostProcessor aus
dotnet publish $projectPath -c Release /p:PublishProfile=PublishNewMod /p:SkipModPostProcessor=true

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n? Publish erfolgreich!`n" -ForegroundColor Green
    
    # Zeige erstellte Dateien
    $publishDir = "Red bike path\bin\Release\net48"
    if (Test-Path $publishDir) {
        Write-Host "?? Veröffentlichte Dateien:" -ForegroundColor Cyan
        Get-ChildItem $publishDir -File | ForEach-Object {
            Write-Host "   • $($_.Name) - $([math]::Round($_.Length / 1KB, 2)) KB" -ForegroundColor Gray
        }
    }
    
    Write-Host "`n?? Nächster Schritt:" -ForegroundColor Yellow
    Write-Host "   Starte Cities: Skylines 2 und gehe zu:" -ForegroundColor Gray
    Write-Host "   Options ? Paradox Mods ? Upload Mod`n" -ForegroundColor Gray
} else {
    Write-Host "`n? Publish fehlgeschlagen!`n" -ForegroundColor Red
}
