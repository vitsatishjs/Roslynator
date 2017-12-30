// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Options = {Options} Escape = {Escape}")]
    public struct MarkdownText : IMarkdown, IEquatable<MarkdownText>
    {
        internal MarkdownText(string text, EmphasisOptions options, bool escape)
        {
            Text = text;
            Options = options;
            Escape = escape;
        }

        public string Text { get; }

        public EmphasisOptions Options { get; }

        public bool Escape { get; }

        public MarkdownText WithText(string text)
        {
            return new MarkdownText(text, Options, Escape);
        }

        public MarkdownText WithOptions(EmphasisOptions options)
        {
            return new MarkdownText(Text, options, Escape);
        }

        public MarkdownText WithEscape(bool escape)
        {
            return new MarkdownText(Text, Options, escape);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.Append(Text, Options, Escape);
        }

        public override bool Equals(object obj)
        {
            return (obj is MarkdownText other)
                && Equals(other);
        }

        public bool Equals(MarkdownText other)
        {
            return Escape == other.Escape
                && Options == other.Options
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Combine((int)Options, Hash.Create(Escape)));
        }

        public static bool operator ==(MarkdownText text1, MarkdownText text2)
        {
            return text1.Equals(text2);
        }

        public static bool operator !=(MarkdownText text1, MarkdownText text2)
        {
            return !(text1 == text2);
        }
    }
}
