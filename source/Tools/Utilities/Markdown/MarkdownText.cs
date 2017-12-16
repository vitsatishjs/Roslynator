// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public struct MarkdownText : IMarkdown
    {
        public static MarkdownText Empty { get; } = new MarkdownText("", escape: false);

        internal MarkdownText(string text, bool escape)
        {
            Text = text;
            Escape = escape;
        }

        public string Text { get; }

        public bool Escape { get; }

        public static implicit operator MarkdownText(string value)
        {
            return new MarkdownText(value, escape: true);
        }

        public void WriteTo(MarkdownWriter mw)
        {
            mw.WriteMarkdown(Text, Escape);
        }
    }
}
