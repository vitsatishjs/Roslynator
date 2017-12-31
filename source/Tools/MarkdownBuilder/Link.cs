// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Url = {Url,nq} Title = {Title,nq}")]
    public struct Link : IMarkdown, IEquatable<Link>
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

        public Link WithText(string text)
        {
            return new Link(text, Url, Title);
        }

        public Link WithUrl(string url)
        {
            return new Link(Text, url, Title);
        }

        public Link WithTitle(string title)
        {
            return new Link(Text, Url, title);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendLink(Text, Url, Title);
        }

        public override bool Equals(object obj)
        {
            return (obj is Link other)
                && Equals(other);
        }

        public bool Equals(Link other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal)
                && string.Equals(Url, other.Url, StringComparison.Ordinal)
                && string.Equals(Title, other.Title, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Combine(Url, Hash.Create(Title)));
        }

        public static bool operator ==(Link left, Link right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Link left, Link right)
        {
            return !(left == right);
        }
    }
}
