using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SkinFramework.DefaultSkins.VS2017.VSTheme;

namespace SkinFramework.DefaultSkins.VS2017
{
    public class VS2017SkinPalette
    {
        public static VS2017SkinPalette Blue { get; }
        public static VS2017SkinPalette Light { get; }
        public static VS2017SkinPalette Dark { get; }

        public SkinEnvironmentPalette Environment { get; set; }

    }
}
