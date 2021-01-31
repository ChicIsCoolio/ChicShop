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
        public static SKTypeface BurbankBigCondensedBold { get; } = SKTypeface.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\BurbankBigCondensed-Bold.ttf"));
        public static SKTypeface BurbankBigRegularBlack { get; } = SKTypeface.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\BurbankBigRegular-Black.otf"));
    }
}
