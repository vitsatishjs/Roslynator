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

        public Image WithText(string text)
        {
            return new Image(text, Url, Title);
        }

        public Image WithUrl(string url)
        {
            return new Image(Text, url, Title);
        }

        public Image WithTitle(string title)
        {
            return new Image(Text, Url, title);
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
