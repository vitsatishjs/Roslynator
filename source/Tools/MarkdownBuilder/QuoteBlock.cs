// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq}")]
    public struct QuoteBlock : IMarkdown, IEquatable<QuoteBlock>
    {
        internal QuoteBlock(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendQuoteBlock(Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is QuoteBlock other)
                && Equals(other);
        }

        public bool Equals(QuoteBlock other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Text);
        }

        public static bool operator ==(QuoteBlock block1, QuoteBlock block2)
        {
            return block1.Equals(block2);
        }

        public static bool operator !=(QuoteBlock block1, QuoteBlock block2)
        {
            return !(block1 == block2);
        }
    }
}
