using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

#nullable enable
namespace ASCIIsome
{
    public static class ApplicationInfo
    {
        // TODO: [HV] Have an icon for the application. 
        // TODO: [HV] 'Check for update' feature. 
        public static string ApplicationName { get; } = "ASCIIsome";
        public static string AppDataFolder { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
        public static Version ApplicationVersion { get; } = new Version(0, 0, 47, 28); // TODO: [HV] Automatically generate (and format) version info in ApplicationInfo and AssemblyInfo class on building/publishing. 
        public static string VersionPrefix { get; } = "a";
        public static string VersionSuffix { get; } = "_190130-1335";
        public static SolidColorBrush ApplicationTitleBrush { get; } = new SolidColorBrush(GetTitleColor());

        private static MColor GetTitleColor() => VersionSuffix switch
        {
            string s when string.IsNullOrWhiteSpace(s) => MColor.FromRgb(43, 87, 151), // [HV] Formal releases on branch master. 
            string s when s.Trim().StartsWith("RC", StringComparison.InvariantCulture) => GetRandomKnownColor(), // [HV] Release candidates on branch RC. 
            _ => MColor.FromRgb(192, 192, 192) // [HV] Internal development versions on branch canary. 
        };

        private static MColor GetRandomKnownColor()
        {
            KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            DColor drawingColor = DColor.FromKnownColor(knownColors[new Random().Next(knownColors.Length)]);
            return MColor.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
        }
    }
}
