using SkiaSharp;

namespace ChicShop.Chic
{
    public class ChicTypefaces
    {
        public static SKTypeface BurbankBigCondensedBold { get; } = SKTypeface.FromFile(Program.Root + "Resources/BurbankBigCondensed-Bold.ttf");
        public static SKTypeface BurbankBigRegularBlack { get; } = SKTypeface.FromFile(Program.Root + "Resources/BurbankBigRegular-Black.otf");
    }
}
