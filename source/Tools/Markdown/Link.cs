// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Markdown
{
    public struct Link : IMarkdown
    {
        internal Link(string text, string url, string title = null)
        {
            Text = text;
            Url = url;
            Title = title;
        }

        public string Text { get; }

        public string Url { get; }

        public string Title { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendLink(Text, Url, Title);
        }
    }
}
