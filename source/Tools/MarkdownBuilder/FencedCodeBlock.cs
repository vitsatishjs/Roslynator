// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Info = {InfoDebuggerDisplay}")]
    public struct FencedCodeBlock : IMarkdown, IEquatable<FencedCodeBlock>
    {
        internal FencedCodeBlock(string text, string info = null)
        {
            Text = text;
            Info = info;
        }

        public string Text { get; }

        public string Info { get; }

        private string InfoDebuggerDisplay => Info ?? "None";

        public FencedCodeBlock WithText(string text)
        {
            return new FencedCodeBlock(text, Info);
        }

        public FencedCodeBlock WithInfo(string info)
        {
            return new FencedCodeBlock(Text, info);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendFencedCodeBlock(Text, Info);
        }

        public override bool Equals(object obj)
        {
            return (obj is FencedCodeBlock other)
                && Equals(other);
        }

        public bool Equals(FencedCodeBlock other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal)
                && string.Equals(Info, other.Info, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Create(Info));
        }

        public static bool operator ==(FencedCodeBlock left, FencedCodeBlock right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FencedCodeBlock left, FencedCodeBlock right)
        {
            return !(left == right);
        }
    }
}
