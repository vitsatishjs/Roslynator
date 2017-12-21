// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Markdown
{
    public struct MarkdownText : IMarkdown
    {
        internal MarkdownText(string text, bool escape)
        {
            Text = text;
            Escape = escape;
        }

        public string Text { get; }

        public bool Escape { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.Append(Text, Escape);
        }
    }
}
