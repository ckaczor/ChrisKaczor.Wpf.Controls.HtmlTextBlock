using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace ChrisKaczor.Wpf.Controls
{
    public class TextParser
    {
        private HtmlTextBlock _parentControl = null!;

        public Collection<TextLine> Parse(HtmlTextBlock parentControl, string? text)
        {
            _parentControl = parentControl;

            text = text?.Replace("&", "&amp;");

            // Add a root tag so the parser is happy
            text = string.Format(CultureInfo.InvariantCulture, "<body>{0}</body>", text);

            // Create an XML document and load it with the text
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = false
            };
            xmlDocument.LoadXml(text);

            // Create a list of text lines
            var lines = new Collection<TextLine>();

            // Walk over the nodes and build up the fragment list
            WalkNodes(xmlDocument.ChildNodes, lines);

            return lines;
        }

        private readonly Stack<TextFragmentStyle> _attributeStack = new();

        private void WalkNodes(XmlNodeList xmlNodeList, Collection<TextLine> textLines)
        {
            if (textLines.Count == 0)
                textLines.Add(new TextLine());

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                TextFragmentStyle style;

                switch (xmlNode.Name.ToUpperInvariant())
                {
                    case "#WHITESPACE":
                    case "#TEXT":
                        {
                            var line = xmlNode.Value!;

                            var textLine = textLines[^1];

                            // Create a new fragment and fill the style information
                            var textFragment = new TextFragment(_parentControl)
                            {
                                Text = line
                            };

                            foreach (var s in _attributeStack)
                            {
                                s.Apply(textFragment);
                            }

                            // Add the fragment to the list
                            textLine.FragmentList.Add(textFragment);

                            break;
                        }

                    case "BR":
                        {
                            var textLine = new TextLine();

                            textLine.FragmentList.Add(new TextFragment(_parentControl) { Text = Environment.NewLine });

                            textLines.Add(textLine);

                            break;
                        }

                    case "EM":
                    case "B":

                        style = new TextFragmentStyle { Weight = FontWeights.Bold };
                        _attributeStack.Push(style);

                        break;

                    case "U":

                        style = new TextFragmentStyle { Underline = true };
                        _attributeStack.Push(style);

                        break;

                    case "CITE":

                        style = new TextFragmentStyle { Style = FontStyles.Italic };
                        _attributeStack.Push(style);

                        break;

                    case "FONT":
                        style = new TextFragmentStyle();

                        if (xmlNode.Attributes == null)
                            break;

                        foreach (XmlAttribute attribute in xmlNode.Attributes)
                        {
                            switch (attribute.Name.ToUpperInvariant())
                            {
                                case "SIZE":
                                    style.Size = Convert.ToDouble(attribute.Value, CultureInfo.InvariantCulture);
                                    break;

                                case "COLOR":
                                    style.Color = (Brush?)new BrushConverter().ConvertFromString(attribute.Value);
                                    break;
                            }
                        }

                        _attributeStack.Push(style);
                        break;
                }

                if (xmlNode.ChildNodes.Count > 0)
                    WalkNodes(xmlNode.ChildNodes, textLines);

                if (_attributeStack.Count > 0)
                    _attributeStack.Pop();
            }
        }
    }
}