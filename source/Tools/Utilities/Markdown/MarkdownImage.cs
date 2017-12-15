// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct MarkdownImage : IAppendable
    {
        internal MarkdownImage(string text, string url)
        {
            Text = text;
            Url = url;
        }

        //TODO: Text vs. OriginalText
        public string Text { get; }

        public string Url { get; }

        public StringBuilder Append(StringBuilder sb, MarkdownSettings settings = null)
        {
            //TODO: ?
            if (string.IsNullOrEmpty(Url))
                return sb.AppendEscape(Text);

            return sb
                .Append("![")
                .AppendEscape(Text)
                .Append("](")
                .Append(Url) //TODO: escape
                .Append(")");
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Url))
                return Text?.EscapeMarkdown();

            return $"![{Text?.EscapeMarkdown()}]({Url})";
        }
    }
}
