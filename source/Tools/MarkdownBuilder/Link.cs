// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    public class Link : MElement
    {
        public Link(string text, string url, string title = null)
        {
            Text = text;
            Url = url;
            Title = title;
        }

        public Link(Link other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Text = other.Text;
            Url = other.Url;
            Title = other.Title;
        }

        public string Text { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public override MarkdownKind Kind => MarkdownKind.Link;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendLink(Text, Url, Title);
        }

        internal override MElement Clone()
        {
            return new Link(this);
        }
    }
}
