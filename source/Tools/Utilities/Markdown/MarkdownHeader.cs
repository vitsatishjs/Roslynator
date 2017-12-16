// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public struct MarkdownHeader : IMarkdown
    {
        internal MarkdownHeader(string text, int level = 1)
        {
            Text = text;
            Level = level;
        }

        public string Text { get; }

        public int Level { get; }

        public void WriteTo(MarkdownWriter mw)
        {
            mw.Write(MarkdownFactory.HeaderStart(Level));
            mw.WriteLineMarkdownIf(!string.IsNullOrEmpty(Text), Text);
        }
    }
}
