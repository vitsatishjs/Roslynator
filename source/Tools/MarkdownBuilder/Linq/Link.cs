// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Text,nq} Url = {Url,nq} Title = {Title,nq}")]
    public class Link : MElement
    {
        private string _url;

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

        public string Url
        {
            get { return _url; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                Error.ThrowIfContainsWhitespace(value, nameof(value));

                _url = value;
            }
        }

        public string Title { get; set; }

        public override MarkdownKind Kind => MarkdownKind.Link;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendLink(Text, Url, Title);
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteLink(Text, Url, Title);
        }

        internal override MElement Clone()
        {
            return new Link(this);
        }
    }
}
