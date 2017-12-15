// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct ItalicText : IAppendable
    {
        internal ItalicText(string text)
        {
            OriginalText = text;
        }

        public string OriginalText { get; }

        public string Text
        {
            get { return ToString(); }
        }

        public StringBuilder Append(StringBuilder sb, MarkdownSettings settings = null)
        {
            settings = settings ?? MarkdownSettings.Default;

            return sb
                .Append(settings.ItalicDelimiterText)
                .AppendEscape(OriginalText)
                .Append(settings.ItalicDelimiterText);
        }

        public override string ToString()
        {
            return ToString(MarkdownSettings.Default);
        }

        public string ToString(MarkdownSettings settings)
        {
            return settings.ItalicDelimiterText + OriginalText?.EscapeMarkdown() + settings.ItalicDelimiterText;
        }
    }
}
