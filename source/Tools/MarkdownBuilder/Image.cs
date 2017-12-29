// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Url = {Url,nq} Title = {Title,nq}")]
    public struct Image : IMarkdown, IEquatable<Image>
    {
        internal Image(string text, string url, string title = null)
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
            return mb.AppendImage(Text, Url, Title);
        }

        public override bool Equals(object obj)
        {
            return (obj is Image other)
                && Equals(other);
        }

        public bool Equals(Image other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal)
                && string.Equals(Url, other.Url, StringComparison.Ordinal)
                && string.Equals(Title, other.Title, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Combine(Url, Hash.Combine(Title, Hash.OffsetBasis)));
        }

        public static bool operator ==(Image left, Image right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Image left, Image right)
        {
            return !(left == right);
        }
    }
}
