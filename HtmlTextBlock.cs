using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChrisKaczor.Wpf.Controls
{
    public class HtmlTextBlock : TextBlock
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register(nameof(Html), typeof(string), typeof(HtmlTextBlock), new UIPropertyMetadata("Html", OnHtmlChanged));

        private static void OnHtmlChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var htmlTextBlock = (HtmlTextBlock) s;

            Parse(htmlTextBlock, e.NewValue as string);
        }

        private static void Parse(HtmlTextBlock control, string? value)
        {
            try
            {
                control.Inlines.Clear();

                var parser = new TextParser();
                var lines = parser.Parse(control, value);

                foreach (var line in lines)
                {
                    foreach (var fragment in line.FragmentList)
                    {
                        var run = new Run(fragment.Text)
                        {
                            FontStyle = fragment.Style,
                            FontWeight = fragment.Weight,
                            FontSize = fragment.Size,
                            Foreground = fragment.Color
                        };

                        control.Inlines.Add(run);
                    }
                }
            }
            catch (Exception)
            {
                control.Inlines.Clear();

                control.Text = value;
            }
        }

        public string Html
        {
            get => (string) GetValue(HtmlProperty);
            set => SetValue(HtmlProperty, value);
        }
    }
}
