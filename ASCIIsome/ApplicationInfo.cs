using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIsome
{
    public static class ApplicationInfo
    {
        public static string ApplicationName { get; } = "ASCIIsome";
        public static Version ApplicationVersion { get; } = new Version(0, 0, 24, 16);
        public static string VersionPrefix { get; } = "a";
        public static string VersionSuffix { get; } = "_181016-1414";
        public static SolidColorBrush ApplicationTitleBrush { get; } = new SolidColorBrush(Color.FromRgb(43, 87, 151));
    }
}
