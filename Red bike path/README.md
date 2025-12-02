# Red Bike Path - Cities: Skylines 2 Mod

Eine Mod für Cities: Skylines 2, die die Farbe des Untergrunds von Fahrradwegen anpassbar macht mit mehreren Farbvoreinstellungen und erweiterten Steuerungsoptionen.

## ? Features

### ?? Vielfältige Farbauswahl
- **10 Farbvoreinstellungen** zur Auswahl: Rot, Blau, Grün, Lila, Orange, Gelb, Rosa, Braun, Weiß, Schwarz
- **Benutzerdefinierte Farben** ?? - Vollständige Kontrolle über die Farbe:
  - ?? **Hex-Farbcode Eingabe** - Gib beliebige HTML/Hex-Farbcodes ein (z.B. #FF0000, #00FF00)
  - ??? **RGB-Schieberegler** - Feinjustierung mit separaten Rot-, Grün- und Blau-Reglern (0-255)
  - ?? **Automatische Synchronisation** - Hex-Code und RGB-Werte bleiben synchron
- **Einstellbare Farbintensität** (0-100%) für subtile oder kräftige Effekte
- Echtzeit-Farbaktualisierung ohne Neustart

### ?? Erweiterte Steuerung
- **"Nur Fahrradspuren" Modus** - Überspringt gemischte Fahrrad+Fußgänger-Wege
- **"Gemischte Wege färben" Option** - Separate Kontrolle für gemischte Oberflächen
- **Intelligente Material-Erkennung** - Unterscheidet zwischen Fahrrad- und Fußgängerbereichen
- **Präzise Filterung** - Färbt nur Fahrradwege, keine Fahrzeuge oder Gebäude

### ?? Was wird eingefärbt
- Dedizierte Fahrradspuren auf Straßen ??
- Getrennte Fahrradwege ??
- Fahrradweg-Oberflächen ??
- Optional: Gemischte Fahrrad/Fußgänger-Wege (konfigurierbar)

### ? Was bleibt unverändert
- Reine Fußgängerwege (immer)
- Gemischte Wege (wenn in Einstellungen deaktiviert)
- Straßenoberflächen
- Fahrzeuge (Autos, Busse, etc.)
- Gebäude und andere Infrastruktur

## Installation

1. Kopiere die Mod-Dateien in:
   `%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\Mods\Red_bike_path\`

2. Alternativ wird die Mod beim Build automatisch in diesen Ordner kopiert.

3. Starte Cities: Skylines 2

4. Aktiviere die Mod im Mod-Manager des Spiels

## Verwendung

### Einstellungen anpassen

1. Öffne das Optionsmenü im Spiel
2. Navigiere zu den Mod-Einstellungen
3. Wähle "Red Bike Path" / "Roter Fahrradweg"
4. Passe die folgenden Einstellungen an:
   - **Color Preset / Farbvoreinstellung**: Wähle aus 10 verschiedenen Farben oder 'Custom' für eigene Farben
     - Rot, Blau, Grün, Lila, Orange, Gelb, Rosa, Braun, Weiß, Schwarz, **Benutzerdefiniert**
   - **Custom Color (nur bei 'Custom')**: 
     - **Hex Color Code**: Gib einen Hex-Code ein (z.B. #123456, #FF00AA)
     - **RGB Sliders**: Nutze die Schieberegler für Rot, Grün und Blau (0-255)
   - **Color Intensity / Farbintensität**: Stelle die Intensität der Farbe ein (0.0 - 1.0)
   - **Only Bike Lanes / Nur Fahrradwege**: Wenn aktiviert, werden nur reine Fahrradwege eingefärbt (geteilte Fußgängerwege bleiben unverändert)

#### Beispiele für Hex-Farbcodes:
- `#FF0000` - Leuchtend Rot
- `#00FF00` - Leuchtend Grün  
- `#0080FF` - Himmelblau
- `#FF6600` - Orange
- `#9900FF` - Violett
- `#FF1493` - Pink/Rosa
- `#40E0D0` - Türkis
- `#FFD700` - Gold

### Funktionsweise

Die Mod verwendet drei verschiedene Systeme, um die Fahrradweg-Farben zu ändern:

1. **MaterialColorSystem**: Ändert die Material-Farben der Fahrradwege zur Laufzeit
2. **BikePathColorSystem**: Verwaltet die Overlay-Farben von Fahrradweg-Entitäten
3. **PrefabColorModifierSystem**: Modifiziert Fahrradweg-Prefabs beim Spielstart

Diese Multi-System-Architektur gewährleistet maximale Kompatibilität mit dem Spiel.

## Bekannte Probleme

- Der ModPostProcessor zeigt einen Fehler beim Build an, aber die Mod funktioniert trotzdem korrekt
- Bei sehr komplexen Städten kann es einige Sekunden dauern, bis alle Fahrradwege aktualisiert sind
- Die Mod funktioniert am besten mit neu gebauten Fahrradwegen

### Gelöst:
- ? Geteilte Fahrrad-/Fußgängerwege: Mit der Option "Nur Fahrradwege" werden geteilte Wege nicht mehr eingefärbt
- ? Fahrzeuge/Gebäude werden eingefärbt: Verbesserte Material-Filterung verhindert nun, dass Nicht-Fahrradweg-Materialien eingefärbt werden

## Deinstallation

1. Deaktiviere die Mod im Spiel
2. Lösche den Ordner `%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\Mods\Red_bike_path\`
3. Starte das Spiel neu

## Entwicklung

### Voraussetzungen

- .NET Framework 4.8
- Visual Studio 2022 oder höher
- Cities: Skylines 2 installiert
- Cities: Skylines 2 Modding Toolchain

### Build

1. Öffne das Projekt in Visual Studio
2. Stelle sicher, dass die Umgebungsvariable `CSII_TOOLPATH` gesetzt ist
3. Build das Projekt (Debug oder Release)
4. Die kompilierte DLL wird automatisch in den Mods-Ordner kopiert

### Projektstruktur

```
Red bike path/
??? Mod.cs                              # Haupt-Mod-Klasse
??? Setting.cs                          # Einstellungen und Lokalisierung
??? Systems/
    ??? BikePathColorSystem.cs          # Entity-basiertes Farbsystem
    ??? MaterialColorSystem.cs          # Material-basiertes Farbsystem
    ??? PrefabColorModifierSystem.cs    # Prefab-Modifikationssystem
```

## Lizenz

Dieses Projekt steht unter keiner spezifischen Lizenz. Verwende und modifiziere es nach Belieben.

## Credits

Entwickelt für Cities: Skylines 2 von Colossal Order.

## Changelog

### Version 2.3.0
- **?? Fixed Color Blending Issue**: Bright colors like white and cyan now display correctly
  - Colors are no longer multiplied with the existing green bike lane color
  - White (#FFFFFF) now appears as white instead of green
  - Cyan and light blue colors display accurately without greenish tint
  - ColorIntensity now properly controls the blend between white and the target color
  - Fixed issue where colors always gravitated towards green

### Version 2.2.0
- **?? Benutzerdefinierte Farben**: Vollständige Farbkontrolle mit Hex-Codes und RGB-Schiebereglern
  - Hex-Farbcode Eingabefeld (z.B. #123456)
  - Separate RGB-Schieberegler (0-255) für präzise Kontrolle
  - Automatische Synchronisation zwischen Hex und RGB
  - Unbegrenzte Farbmöglichkeiten!

### Version 2.1.0
- **?? Wichtiger Bugfix**: Fahrzeuge und Gebäude werden nicht mehr eingefärbt
- **Verbesserte Material-Erkennung**: Präzisere Filterung für Fahrradweg-spezifische Materialien
- **Erweiterte Ausschlüsse**: Automatisches Ausschließen von Fahrzeug-, Gebäude- und UI-Materialien
- **Shader-basierte Filterung**: Nur Terrain/Path-Shader werden berücksichtigt

### Version 2.0.0
- **Farbpalette hinzugefügt**: 10 vordefinierte Farben zur Auswahl
- **"Nur Fahrradwege" Option**: Schließt geteilte Fußgängerwege aus
- Vereinfachte Benutzeroberfläche mit Dropdown statt Schieberegler

### Version 1.0.0
- Initiales Release
- Grundlegende Farbänderung von Grün zu Rot
- Anpassbare Farbe und Intensität
- Deutsche und englische Lokalisierung
