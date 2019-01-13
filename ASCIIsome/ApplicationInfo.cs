using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace ASCIIsome
{
    public static class ApplicationInfo
    {
        // TODO: [HV] Have an icon for the application
        public static string ApplicationName { get; } = "ASCIIsome";
        public static string AppDataFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
        public static Version ApplicationVersion { get; } = new Version(0, 0, 45, 20); // TODO: [HV] Automatically generate (and format) version info in ApplicationInfo and AssemblyInfo class on building/publishing 
        public static string VersionPrefix { get; } = "a";
        public static string VersionSuffix { get; } = "_190113-1805";
        public static SolidColorBrush ApplicationTitleBrush { get; } = new SolidColorBrush(GetTitleColor());

        private static MColor GetTitleColor() // TODO: [HV] Change to switch expression in C# 8.0
        {
            switch (VersionSuffix)
            {
                case string s when string.IsNullOrWhiteSpace(s): // [HV] Formal releases on branch master
                    return MColor.FromRgb(43, 87, 151);
                case string s when s.Trim().StartsWith("RC", StringComparison.InvariantCulture): // [HV] Release candidates on branch RC
                    return GetRandomKnownColor();
                default: // [HV] Internal development versions on branch canary
                    return MColor.FromRgb(192, 192, 192);
            }
        }

        private static MColor GetRandomKnownColor()
        {
            KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor knownColor = knownColors[new Random().Next(knownColors.Length)];
            DColor drawingColor = DColor.FromKnownColor(knownColors[new Random().Next(knownColors.Length)]);
            return MColor.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
        }
    }
}
