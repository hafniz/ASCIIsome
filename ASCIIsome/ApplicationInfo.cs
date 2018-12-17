using System;
using System.Windows.Media;

namespace ASCIIsome
{
    public static class ApplicationInfo
    {
        // TODO: [HV] Have an icon for the application

        public static string ApplicationName { get; } = "ASCIIsome";
        public static Version ApplicationVersion { get; } = new Version(0, 0, 40, 60); // TODO: [HV] Automatically generate (and format) version info in ApplicationInfo and AssemblyInfo class on building/publishing 
        public static string VersionPrefix { get; } = "a";
        public static string VersionSuffix { get; } = "_181218-0010";
        public static SolidColorBrush ApplicationTitleBrush { get; } = new SolidColorBrush(Color.FromRgb(43, 87, 151));
    }
}
