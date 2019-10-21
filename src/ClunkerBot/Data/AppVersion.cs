using System;

namespace ClunkerBot.Data
{
    class AppVersion
    {
        public static readonly int Major = 19;
        public static readonly int Minor = 14;
        public static readonly int Patch = 1;
        public static readonly string Release = "DeLorean";

        public static readonly string FullVersion = $"{Major}.{Minor}.{Patch}";
    }
}
