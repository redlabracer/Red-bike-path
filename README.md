# Red Bike Path - Cities: Skylines 2 Mod

Eine Mod f�r Cities: Skylines 2, die die Farbe des Untergrunds von Fahrradwegen anpassbar macht mit mehreren Farbvoreinstellungen und erweiterten Steuerungsoptionen.

## ? Features

### ?? Vielf�ltige Farbauswahl
- **10 Farbvoreinstellungen** zur Auswahl: Rot, Blau, Gr�n, Lila, Orange, Gelb, Rosa, Braun, Wei�, Schwarz
- **Einstellbare Farbintensit�t** (0-100%) f�r subtile oder kr�ftige Effekte
- Echtzeit-Farbaktualisierung ohne Neustart

### ?? Erweiterte Steuerung
- **"Nur Fahrradspuren" Modus** - �berspringt gemischte Fahrrad+Fu�g�nger-Wege
- **"Gemischte Wege f�rben" Option** - Separate Kontrolle f�r gemischte Oberfl�chen
- **Intelligente Material-Erkennung** - Unterscheidet zwischen Fahrrad- und Fu�g�ngerbereichen

### ??? Was wird eingef�rbt
- Dedizierte Fahrradspuren auf Stra�en ?
- Getrennte Fahrradwege ?
- Fahrradweg-Oberfl�chen ?
- Optional: Gemischte Fahrrad/Fu�g�nger-Wege (konfigurierbar)

### ?? Was bleibt unver�ndert
- Reine Fu�g�ngerwege (immer)
- Gemischte Wege (wenn in Einstellungen deaktiviert)
- Stra�enoberfl�chen
- Geb�ude und andere Infrastruktur

## Installation

1. Kopiere die Mod-Dateien in:
   `%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\Mods\Red_bike_path\`

2. Alternativ wird die Mod beim Build automatisch in diesen Ordner kopiert.

3. Starte Cities: Skylines 2

4. Aktiviere die Mod im Mod-Manager des Spiels

## Verwendung

### Einstellungen anpassen

1. �ffne das Optionsmen� im Spiel
2. Navigiere zu den Mod-Einstellungen
3. W�hle "Red Bike Path" / "Roter Fahrradweg"
4. Passe die folgenden Einstellungen an:
   - **Color Preset / Farbvoreinstellung**: W�hle aus 10 verschiedenen Farben (Standard: Rot)
     - Rot, Blau, Gr�n, Lila, Orange, Gelb, Rosa, Braun, Wei�, Schwarz
   - **Color Intensity / Farbintensit�t**: Stelle die Intensit�t der Farbe ein (0.0 - 1.0)
   - **Only Bike Lanes / Nur Fahrradwege**: Wenn aktiviert, werden nur reine Fahrradwege eingef�rbt (geteilte Fu�g�ngerwege bleiben unver�ndert)

### Funktionsweise

Die Mod verwendet drei verschiedene Systeme, um die Fahrradweg-Farben zu �ndern:

1. **MaterialColorSystem**: �ndert die Material-Farben der Fahrradwege zur Laufzeit
2. **BikePathColorSystem**: Verwaltet die Overlay-Farben von Fahrradweg-Entit�ten
3. **PrefabColorModifierSystem**: Modifiziert Fahrradweg-Prefabs beim Spielstart

Diese Multi-System-Architektur gew�hrleistet maximale Kompatibilit�t mit dem Spiel.

## Bekannte Probleme

- Der ModPostProcessor zeigt einen Fehler beim Build an, aber die Mod funktioniert trotzdem korrekt
- Bei sehr komplexen St�dten kann es einige Sekunden dauern, bis alle Fahrradwege aktualisiert sind
- Die Mod funktioniert am besten mit neu gebauten Fahrradwegen

### Gel�st:
- ? Geteilte Fahrrad-/Fu�g�ngerwege: Mit der Option "Nur Fahrradwege" werden geteilte Wege nicht mehr eingef�rbt

## Deinstallation

1. Deaktiviere die Mod im Spiel
2. L�sche den Ordner `%LocalAppData%\..\LocalLow\Colossal Order\Cities Skylines II\Mods\Red_bike_path\`
3. Starte das Spiel neu

## Entwicklung

### Voraussetzungen

- .NET Framework 4.8
- Visual Studio 2022 oder h�her
- Cities: Skylines 2 installiert
- Cities: Skylines 2 Modding Toolchain

### Build

1. �ffne das Projekt in Visual Studio
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

Entwickelt f�r Cities: Skylines 2 von Colossal Order.

## Changelog

### Version 2.0.0
- **Farbpalette hinzugef�gt**: 10 vordefinierte Farben zur Auswahl
- **"Nur Fahrradwege" Option**: Schlie�t geteilte Fu�g�ngerwege aus
- Vereinfachte Benutzeroberfl�che mit Dropdown statt Schieberegler

### Version 1.0.0
- Initiales Release
- Grundlegende Farb�nderung von Gr�n zu Rot
- Anpassbare Farbe und Intensit�t
- Deutsche und englische Lokalisierung
