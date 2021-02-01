using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop.Chic
{
    public class ChicTypefaces
    {
        public static SKTypeface BurbankBigCondensedBold { get; } = SKTypeface.FromFile(Program.Root + "Resources\\BurbankBigCondensed-Bold.ttf");
        public static SKTypeface BurbankBigRegularBlack { get; } = SKTypeface.FromFile(Program.Root + "Resources\\BurbankBigRegular-Black.otf");
    }
}
