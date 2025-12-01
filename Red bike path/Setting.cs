using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using UnityEngine;

namespace Red_bike_path
{
    [FileLocation(nameof(Red_bike_path))]
    [SettingsUIGroupOrder(kMainGroup, kCustomColorGroup, kAdvancedColorGroup)]
    [SettingsUIShowGroupName(kMainGroup, kCustomColorGroup, kAdvancedColorGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";
        public const string kMainGroup = "Settings";
        public const string kCustomColorGroup = "CustomColor";
        public const string kAdvancedColorGroup = "AdvancedColor";

        private string m_LastProcessedHex = "";
        private bool m_SyncingColors = false;

        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        [SettingsUISection(kSection, kMainGroup)]
        public BikePathColorPreset ColorPreset { get; set; }

        [SettingsUITextInput]
        [SettingsUISection(kSection, kCustomColorGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(IsNotCustomColor))]
        public string CustomColorHex { get; set; }

        [SettingsUISlider(min = 0f, max = 255f, step = 1f, scalarMultiplier = 1f, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kCustomColorGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(IsNotCustomColor))]
        public float CustomColorRed { get; set; }

        [SettingsUISlider(min = 0f, max = 255f, step = 1f, scalarMultiplier = 1f, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kCustomColorGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(IsNotCustomColor))]
        public float CustomColorGreen { get; set; }

        [SettingsUISlider(min = 0f, max = 255f, step = 1f, scalarMultiplier = 1f, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kCustomColorGroup)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(IsNotCustomColor))]
        public float CustomColorBlue { get; set; }

        [SettingsUISlider(min = 0.0f, max = 1.0f, step = 0.01f)]
        [SettingsUISection(kSection, kMainGroup)]
        public float ColorIntensity { get; set; }

        [SettingsUISlider(min = 0.0f, max = 1.0f, step = 0.01f)]
        [SettingsUISection(kSection, kAdvancedColorGroup)]
        public float Saturation { get; set; }

        [SettingsUISlider(min = 0.0f, max = 1.0f, step = 0.01f)]
        [SettingsUISection(kSection, kAdvancedColorGroup)]
        public float Brightness { get; set; }

        [SettingsUISection(kSection, kMainGroup)]
        public bool OnlyBikeLanesNotPedestrian { get; set; }

        [SettingsUISection(kSection, kMainGroup)]
        public bool ColorMixedPaths { get; set; }

        public bool IsNotCustomColor()
        {
            return ColorPreset != BikePathColorPreset.Custom;
        }

        public Color GetBikePathColor()
        {
            Color baseColor;
            
            switch (ColorPreset)
            {
                case BikePathColorPreset.Red:
                    baseColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case BikePathColorPreset.Blue:
                    baseColor = new Color(0.0f, 0.5f, 1.0f, 1.0f);
                    break;
                case BikePathColorPreset.Aqua:
                    baseColor = new Color(0.0f, 0.8f, 0.9f, 1.0f);
                    break;
                case BikePathColorPreset.LondonBlue:
                    baseColor = new Color(0.0f, 0.6f, 0.85f, 1.0f);
                    break;
                case BikePathColorPreset.Cyan:
                    baseColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
                    break;
                case BikePathColorPreset.Turquoise:
                    baseColor = new Color(0.25f, 0.88f, 0.82f, 1.0f);
                    break;
                case BikePathColorPreset.Purple:
                    baseColor = new Color(0.6f, 0.0f, 0.8f, 1.0f);
                    break;
                case BikePathColorPreset.Orange:
                    baseColor = new Color(1.0f, 0.5f, 0.0f, 1.0f);
                    break;
                case BikePathColorPreset.Yellow:
                    baseColor = new Color(1.0f, 0.9f, 0.0f, 1.0f);
                    break;
                case BikePathColorPreset.Pink:
                    baseColor = new Color(1.0f, 0.4f, 0.7f, 1.0f);
                    break;
                case BikePathColorPreset.Brown:
                    baseColor = new Color(0.6f, 0.4f, 0.2f, 1.0f);
                    break;
                case BikePathColorPreset.White:
                    baseColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    break;
                case BikePathColorPreset.Black:
                    baseColor = new Color(0.1f, 0.1f, 0.1f, 1.0f);
                    break;
                case BikePathColorPreset.Green:
                    baseColor = new Color(0.0f, 0.8f, 0.2f, 1.0f);
                    break;
                case BikePathColorPreset.LimeGreen:
                    baseColor = new Color(0.5f, 1.0f, 0.0f, 1.0f);
                    break;
                case BikePathColorPreset.Magenta:
                    baseColor = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                    break;
                case BikePathColorPreset.Custom:
                    baseColor = GetCustomColor();
                    break;
                default:
                    baseColor = new Color(0.0f, 0.8f, 0.2f, 1.0f);
                    break;
            }

            return ApplySaturationAndBrightness(baseColor);
        }

        private Color ApplySaturationAndBrightness(Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            
            s = s * Saturation;
            v = v * Brightness;
            
            s = Mathf.Clamp01(s);
            v = Mathf.Clamp01(v);
            
            return Color.HSVToRGB(h, s, v);
        }

        private Color GetCustomColor()
        {
            if (m_SyncingColors)
            {
                return CreateColorFromRGB();
            }

            // Nur wenn der Hex-Code sich geändert hat, RGB-Werte aktualisieren
            if (!string.IsNullOrEmpty(CustomColorHex) && 
                CustomColorHex != m_LastProcessedHex && 
                TryParseHexColor(CustomColorHex, out Color hexColor))
            {
                m_SyncingColors = true;
                m_LastProcessedHex = CustomColorHex;
                CustomColorRed = hexColor.r * 255f;
                CustomColorGreen = hexColor.g * 255f;
                CustomColorBlue = hexColor.b * 255f;
                m_SyncingColors = false;
                return hexColor;
            }

            // Ansonsten RGB-Werte verwenden und Hex aktualisieren
            return CreateColorFromRGB();
        }

        private Color CreateColorFromRGB()
        {
            float r = Mathf.Clamp01(CustomColorRed / 255f);
            float g = Mathf.Clamp01(CustomColorGreen / 255f);
            float b = Mathf.Clamp01(CustomColorBlue / 255f);
            
            // Hex-Code aktualisieren, wenn wir nicht gerade synchronisieren
            if (!m_SyncingColors)
            {
                UpdateHexFromRGB(r, g, b);
            }
            
            return new Color(r, g, b, 1.0f);
        }

        private void UpdateHexFromRGB(float r, float g, float b)
        {
            int rInt = Mathf.RoundToInt(r * 255f);
            int gInt = Mathf.RoundToInt(g * 255f);
            int bInt = Mathf.RoundToInt(b * 255f);
            string newHex = $"#{rInt:X2}{gInt:X2}{bInt:X2}";
            
            if (CustomColorHex != newHex)
            {
                m_SyncingColors = true;
                CustomColorHex = newHex;
                m_LastProcessedHex = newHex;
                m_SyncingColors = false;
            }
        }

        private bool TryParseHexColor(string hex, out Color color)
        {
            color = Color.white;
            
            if (string.IsNullOrEmpty(hex))
                return false;

            hex = hex.Trim();
            
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            if (hex.Length != 6)
                return false;

            try
            {
                int r = System.Convert.ToInt32(hex.Substring(0, 2), 16);
                int g = System.Convert.ToInt32(hex.Substring(2, 2), 16);
                int b = System.Convert.ToInt32(hex.Substring(4, 2), 16);

                color = new Color(r / 255f, g / 255f, b / 255f, 1.0f);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void SetDefaults()
        {
            ColorPreset = BikePathColorPreset.Red;
            ColorIntensity = 1.0f;
            Saturation = 1.0f;
            Brightness = 1.0f;
            OnlyBikeLanesNotPedestrian = false;
            ColorMixedPaths = false;
            CustomColorHex = "#FF0000";
            CustomColorRed = 255f;
            CustomColorGreen = 0f;
            CustomColorBlue = 0f;
            m_LastProcessedHex = "#FF0000";
        }
    }

    public enum BikePathColorPreset
    {
        Red,
        Blue,
        Aqua,
        LondonBlue,
        Cyan,
        Turquoise,
        Green,
        LimeGreen,
        Purple,
        Magenta,
        Orange,
        Yellow,
        Pink,
        Brown,
        White,
        Black,
        Custom
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Red Bike Path" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kMainGroup), "Settings" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kCustomColorGroup), "Custom Color" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kAdvancedColorGroup), "Advanced Color Controls" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorPreset)), "Color Preset" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorPreset)), "Choose a color preset for bike paths or select 'Custom' for your own color" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorHex)), "Hex Color Code" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorHex)), "Enter a hex color code (e.g. #FF0000 for red, #00FF00 for green). Changes will update the RGB sliders." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorRed)), "Red (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorRed)), "Red component (0-255). Changes will update the hex code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorGreen)), "Green (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorGreen)), "Green component (0-255). Changes will update the hex code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorBlue)), "Blue (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorBlue)), "Blue component (0-255). Changes will update the hex code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorIntensity)), "Color Intensity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorIntensity)), "Intensity of the bike path color effect" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Saturation)), "Saturation" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Saturation)), "Control the color saturation (0 = grayscale, 1 = full color)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Brightness)), "Brightness" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Brightness)), "Control the color brightness (0 = black, 1 = full brightness)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Only Bike Lanes (Skip Mixed Paths)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "If enabled, skip mixed/shared bike+pedestrian paths to avoid coloring pedestrian areas. Note: This may also skip some pure bike paths." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorMixedPaths)), "Color Mixed Bike+Pedestrian Paths" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorMixedPaths)), "If enabled, color mixed paths where bikes and pedestrians share the same surface. Warning: This will also color the pedestrian areas!" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Red), "Red" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Blue), "Blue" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Green), "Green" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Aqua), "Aqua" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.LondonBlue), "London Blue (Cycle Superhighway)" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Cyan), "Cyan" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Turquoise), "Turquoise" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Purple), "Purple" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Orange), "Orange" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Yellow), "Yellow" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Pink), "Pink" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Brown), "Brown" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.White), "White" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Black), "Black" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.LimeGreen), "Lime Green" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Magenta), "Magenta" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Custom), "Custom" },
            };
        }

        public void Unload()
        {

        }
    }

    public class LocaleDE : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleDE(Setting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Roter Fahrradweg" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Hauptmenü" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kMainGroup), "Einstellungen" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kCustomColorGroup), "Benutzerdefinierte Farbe" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kAdvancedColorGroup), "Erweiterte Farbsteuerung" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorPreset)), "Farbvoreinstellung" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorPreset)), "Wähle eine Farbvoreinstellung für Fahrradwege oder wähle 'Benutzerdefiniert' für eine eigene Farbe" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorHex)), "Hex-Farbcode" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorHex)), "Gib einen Hex-Farbcode ein (z.B. #FF0000 für Rot, #00FF00 für Grün). Änderungen aktualisieren die RGB-Schieberegler." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorRed)), "Rot (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorRed)), "Rot-Komponente (0-255). Änderungen aktualisieren den Hex-Code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorGreen)), "Grün (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorGreen)), "Grün-Komponente (0-255). Änderungen aktualisieren den Hex-Code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CustomColorBlue)), "Blau (0-255)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CustomColorBlue)), "Blau-Komponente (0-255). Änderungen aktualisieren den Hex-Code." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorIntensity)), "Farbintensität" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorIntensity)), "Intensität des Fahrradweg-Farbeffekts" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Saturation)), "Sättigung" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Saturation)), "Steuert die Farbsättigung (0 = Graustufen, 1 = volle Farbe)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Brightness)), "Helligkeit" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Brightness)), "Steuert die Helligkeit (0 = schwarz, 1 = volle Helligkeit)" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Nur Fahrradspuren (Gemischte Wege überspringen)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Wenn aktiviert, werden gemischte/geteilte Fahrrad+Fußgänger-Wege übersprungen, um Fußgängerbereiche nicht einzufärben. Hinweis: Dies kann auch einige reine Fahrradwege überspringen." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorMixedPaths)), "Gemischte Fahrrad+Fußgänger-Wege färben" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorMixedPaths)), "Wenn aktiviert, werden gemischte Wege gefärbt, wo Fahrräder und Fußgänger die gleiche Fläche teilen. Warnung: Dies färbt auch die Fußgängerbereiche!" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Red), "Rot" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Blue), "Blau" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Green), "Grün" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Aqua), "Aqua" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.LondonBlue), "London Blau (Cycle Superhighway)" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Cyan), "Cyan" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Turquoise), "Türkis" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Purple), "Lila" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Orange), "Orange" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Yellow), "Gelb" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Pink), "Rosa" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Brown), "Braun" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.White), "Weiß" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Black), "Schwarz" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.LimeGreen), "Limettengrün" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Magenta), "Magenta" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Custom), "Benutzerdefiniert" },
            };
        }

        public void Unload()
        {

        }
    }
}
