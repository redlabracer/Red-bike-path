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
    [SettingsUIGroupOrder(kMainGroup)]
    [SettingsUIShowGroupName(kMainGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";
        public const string kMainGroup = "Settings";

        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        [SettingsUISection(kSection, kMainGroup)]
        public BikePathColorPreset ColorPreset { get; set; }

        [SettingsUISlider(min = 0.0f, max = 1.0f, step = 0.01f)]
        [SettingsUISection(kSection, kMainGroup)]
        public float ColorIntensity { get; set; }

        [SettingsUISection(kSection, kMainGroup)]
        public bool OnlyBikeLanesNotPedestrian { get; set; }

        [SettingsUISection(kSection, kMainGroup)]
        public bool ColorMixedPaths { get; set; }

        public Color GetBikePathColor()
        {
            switch (ColorPreset)
            {
                case BikePathColorPreset.Red:
                    return new Color(1.0f, 0.0f, 0.0f, 1.0f);
                case BikePathColorPreset.Blue:
                    return new Color(0.0f, 0.5f, 1.0f, 1.0f);
                case BikePathColorPreset.Purple:
                    return new Color(0.6f, 0.0f, 0.8f, 1.0f);
                case BikePathColorPreset.Orange:
                    return new Color(1.0f, 0.5f, 0.0f, 1.0f);
                case BikePathColorPreset.Yellow:
                    return new Color(1.0f, 0.9f, 0.0f, 1.0f);
                case BikePathColorPreset.Pink:
                    return new Color(1.0f, 0.4f, 0.7f, 1.0f);
                case BikePathColorPreset.Brown:
                    return new Color(0.6f, 0.4f, 0.2f, 1.0f);
                case BikePathColorPreset.White:
                    return new Color(0.9f, 0.9f, 0.9f, 1.0f);
                case BikePathColorPreset.Black:
                    return new Color(0.1f, 0.1f, 0.1f, 1.0f);
                case BikePathColorPreset.Green:
                default:
                    return new Color(0.0f, 0.8f, 0.2f, 1.0f);
            }
        }

        public override void SetDefaults()
        {
            ColorPreset = BikePathColorPreset.Red;
            ColorIntensity = 1.0f;
            OnlyBikeLanesNotPedestrian = false;
            ColorMixedPaths = false; // Standardmäßig gemischte Wege NICHT färben
        }
    }

    public enum BikePathColorPreset
    {
        Red,
        Blue,
        Green,
        Purple,
        Orange,
        Yellow,
        Pink,
        Brown,
        White,
        Black
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
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorPreset)), "Color Preset" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorPreset)), "Choose a color preset for bike paths" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorIntensity)), "Color Intensity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorIntensity)), "Intensity of the bike path color effect" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Only Bike Lanes (Skip Mixed Paths)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "If enabled, skips mixed/divided bike+pedestrian paths to avoid coloring pedestrian areas. Note: This may also skip some pure bike paths due to naming." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorMixedPaths)), "Color Mixed Bike+Pedestrian Paths" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorMixedPaths)), "If enabled, colors mixed paths where bikes and pedestrians share the same surface. Warning: This will also color the pedestrian areas!" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Red), "Red" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Blue), "Blue" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Green), "Green" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Purple), "Purple" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Orange), "Orange" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Yellow), "Yellow" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Pink), "Pink" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Brown), "Brown" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.White), "White" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Black), "Black" },
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
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorPreset)), "Farbvoreinstellung" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorPreset)), "Wähle eine Farbvoreinstellung für Fahrradwege" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorIntensity)), "Farbintensität" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorIntensity)), "Intensität des Fahrradweg-Farbeffekts" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Nur Fahrradspuren (Gemischte Wege überspringen)" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OnlyBikeLanesNotPedestrian)), "Wenn aktiviert, werden gemischte/geteilte Fahrrad+Fußgänger-Wege übersprungen, um Fußgängerbereiche nicht einzufärben. Hinweis: Dies kann auch einige reine Fahrradwege überspringen." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ColorMixedPaths)), "Gemischte Fahrrad+Fußgänger-Wege färben" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ColorMixedPaths)), "Wenn aktiviert, werden gemischte Wege gefärbt, wo Fahrräder und Fußgänger die gleiche Fläche teilen. Warnung: Dies färbt auch die Fußgängerbereiche!" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Red), "Rot" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Blue), "Blau" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Green), "Grün" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Purple), "Lila" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Orange), "Orange" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Yellow), "Gelb" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Pink), "Rosa" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Brown), "Braun" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.White), "Weiß" },
                { m_Setting.GetEnumValueLocaleID(BikePathColorPreset.Black), "Schwarz" },
            };
        }

        public void Unload()
        {

        }
    }
}
