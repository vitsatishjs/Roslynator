// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct StrikethroughText : IAppendable
    {
        public const string Delimiter = "~~";

        internal StrikethroughText(string text)
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
            return sb
                .Append(Delimiter)
                .AppendEscape(OriginalText)
                .Append(Delimiter);
        }

        public override string ToString()
        {
            return Delimiter + OriginalText?.EscapeMarkdown() + Delimiter;
        }
    }
}
