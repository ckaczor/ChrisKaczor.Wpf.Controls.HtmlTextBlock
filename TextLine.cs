using System.Collections.ObjectModel;

namespace ChrisKaczor.Wpf.Controls
{
    public class TextLine
    {
        public Collection<TextFragment> FragmentList { get; }

        public TextLine()
        {
            FragmentList = new Collection<TextFragment>();
        }
    }
}
