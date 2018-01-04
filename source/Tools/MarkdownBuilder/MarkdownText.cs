// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Options = {Options}")]
    public class MarkdownText : MElement, IEquatable<MarkdownText>, IMarkdown
    {
        internal MarkdownText(string text, EmphasisOptions options = EmphasisOptions.None)
        {
            Text = text;
            Options = options;
        }

        public string Text { get; }

        public EmphasisOptions Options { get; }

        public override MarkdownKind Kind => MarkdownKind.Text;

        public MarkdownText WithText(string text)
        {
            return new MarkdownText(text, Options);
        }

        public MarkdownText WithOptions(EmphasisOptions options)
        {
            return new MarkdownText(Text, options);
        }

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.Append(Text, Options);
        }

        public override bool Equals(object obj)
        {
            return (obj is MarkdownText other)
                && Equals(other);
        }

        public bool Equals(MarkdownText other)
        {
            return Options == other.Options
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Create((int)Options));
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
