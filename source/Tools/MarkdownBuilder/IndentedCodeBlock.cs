// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq}")]
    public struct IndentedCodeBlock : IMarkdown, IEquatable<IndentedCodeBlock>
    {
        internal IndentedCodeBlock(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public IndentedCodeBlock WithText(string text)
        {
            return new IndentedCodeBlock(text);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendIndentedCodeBlock(Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is IndentedCodeBlock other)
                && Equals(other);
        }

        public bool Equals(IndentedCodeBlock other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Text);
        }

        public static bool operator ==(IndentedCodeBlock left, IndentedCodeBlock right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IndentedCodeBlock left, IndentedCodeBlock right)
        {
            return !(left == right);
        }
    }
}
