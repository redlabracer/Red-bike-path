# Debug-Anleitung für Red Bike Path Mod

## Problem: Fußgängerwege werden immer noch eingefärbt

### Schritt 1: Log-Dateien überprüfen

Die Mod schreibt jetzt detaillierte Logs. Finde die Log-Datei hier:

```
%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\Logs\Player.log
```

### Schritt 2: Nach Mod-Logs suchen

Öffne die `Player.log` Datei und suche nach:
- `MaterialColorSystem`
- `Updated material:`
- `Skipped shared/ped material:`

### Schritt 3: Material-Namen identifizieren

Die Logs zeigen dir welche Materialien gefunden wurden, z.B.:
```
[INFO] Updated material: BikelaneTexture
[INFO] Skipped shared/ped material: PedestrianPath_Divided
```

### Schritt 4: Wenn Fußgängerwege immer noch eingefärbt werden

Falls der Fußgängerweg-Teil des "Divided Wide Pedestrian-Bicycle Path" immer noch rot ist:

1. **Finde den Material-Namen** in den Logs
2. **Füge ihn zur Ausschlussliste hinzu** in `MaterialColorSystem.cs`:

```csharp
private bool IsSharedPathMaterial(string materialName)
{
    string[] sharedPathKeywords = new[]
    {
        "divided",
        "shared",
        "mixed",
        "pedestrian",
        "ped",
        "sidewalk",
        "pathway",
        "pedway",
        "walk",
        "footpath",
        "pavement",
        "curb",
        "border",
        // HIER DEN NEUEN NAMEN HINZUFÜGEN
        "deinmaterialname"
    };
    // ...
}
```

### Schritt 5: Spezifische Material-Namen

Basierend auf dem Screenshot könnte der Material-Name sein:
- `DividedWide_Pedestrian`
- `Wide_Ped_Divided`
- `PedBike_Divided_Wide`
- Oder ähnlich

### Schritt 6: Alternative Lösung - Nur spezifische Materialien färben

Wenn die Ausschlussliste nicht funktioniert, können wir eine **Whitelist** verwenden:

```csharp
private bool IsBikePathMaterial(string materialName)
{
    // Nur DIESE spezifischen Materialien färben
    string[] exactBikePathNames = new[]
    {
        "bikelane",
        "bikepath",
        "cycle_lane",
        // Füge hier die genauen Namen hinzu
    };

    foreach (var name in exactBikePathNames)
    {
        if (materialName.Contains(name))
        {
            return true;
        }
    }

    return false;
}
```

## Hilfreiche PowerShell-Befehle

### Log-Datei in Echtzeit beobachten:
```powershell
Get-Content "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\Logs\Player.log" -Wait -Tail 50 | Select-String "MaterialColorSystem"
```

### Nur Mod-relevante Zeilen anzeigen:
```powershell
Select-String -Path "$env:LOCALAPPDATA\..\LocalLow\Colossal Order\Cities Skylines II\Logs\Player.log" -Pattern "MaterialColorSystem|Updated material|Skipped" | Select-Object -Last 20
```

## Nächste Schritte

1. Starte das Spiel neu
2. Baue einen "Divided Wide Pedestrian-Bicycle Path"
3. Schau in die Logs welche Materialien gefunden wurden
4. Teile mir die Material-Namen mit
5. Ich kann die Filter dann genau anpassen

## Temporärer Workaround

Wenn es nicht sofort funktioniert, kannst du die Option **"Nur Fahrradwege"** vorübergehend **deaktivieren**, dann werden alle Wege eingefärbt (auch wenn das nicht ideal ist).
