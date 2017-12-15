// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct BoldText : IAppendable
    {
        internal BoldText(string text)
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
                .Append(settings.BoldDelimiterText)
                .AppendEscape(OriginalText)
                .Append(settings.BoldDelimiterText);
        }

        public override string ToString()
        {
            return ToString(MarkdownSettings.Default);
        }

        public string ToString(MarkdownSettings settings)
        {
            return settings.BoldDelimiterText + OriginalText?.EscapeMarkdown() + settings.BoldDelimiterText;
        }
    }
}
