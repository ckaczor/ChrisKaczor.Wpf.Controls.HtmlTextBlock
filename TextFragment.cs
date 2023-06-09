﻿using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ChrisKaczor.Wpf.Controls
{
    public class TextFragment
    {
        private readonly HtmlTextBlock _parent;

        public Brush Color { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public double Size { get; set; }
        public string Text { get; set; }
        public bool Underline { get; set; }

        private Typeface? _typeface;

        private Typeface Typeface
        {
            get { return _typeface ??= new Typeface(_parent.FontFamily, Style, Weight, _parent.FontStretch); }
        }

        private FormattedText? _formattedText;
        public FormattedText FormattedText
        {
            get
            {
                if (_formattedText != null) 
                    return _formattedText;

                var measureText = string.IsNullOrEmpty(Text) ? " " : Text;

                _formattedText = new FormattedText(measureText, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, Typeface, Size, Color, null, TextFormattingMode.Display, 1);

                var textDecorationCollection = new TextDecorationCollection();

                if (Underline)
                {
                    var underlineDecoration = new TextDecoration { PenThicknessUnit = TextDecorationUnit.FontRecommended };

                    textDecorationCollection.Add(underlineDecoration);
                }

                _formattedText.SetTextDecorations(textDecorationCollection);

                return _formattedText;
            }
        }

        public TextFragment(HtmlTextBlock parent)
        {
            _parent = parent;

            Color = _parent.Foreground;
            Style = _parent.FontStyle;
            Weight = _parent.FontWeight;
            Size = _parent.FontSize;
            Text = string.Empty;
            Underline = false;
        }
    }
}