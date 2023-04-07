using System.Windows;
using System.Windows.Media;

namespace ChrisKaczor.Wpf.Controls
{
    public class TextFragmentStyle
    {
        public Brush? Color { get; set; }
        public FontStyle? Style { get; init; }
        public FontWeight? Weight { get; init; }
        public double? Size { get; set; }
        public bool? Underline { get; init; }

        public void Apply(TextFragment fragment)
        {
            if (Color != null)
                fragment.Color = Color;

            if (Style.HasValue)
                fragment.Style = Style.Value;

            if (Weight.HasValue)
                fragment.Weight = Weight.Value;

            if (Size.HasValue)
                fragment.Size = Size.Value;

            if (Underline.HasValue)
                fragment.Underline = Underline.Value;
        }
    }
}