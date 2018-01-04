// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    //TODO: LiteralText
    [DebuggerDisplay("{Text,nq}")]
    public struct RawText : IMarkdown, IEquatable<RawText>
    {
        internal RawText(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public RawText WithText(string text)
        {
            return new RawText(text);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendRaw(Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is RawText other)
                && Equals(other);
        }

        public bool Equals(RawText other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Text);
        }

        public static bool operator ==(RawText text1, RawText text2)
        {
            return text1.Equals(text2);
        }

        public static bool operator !=(RawText text1, RawText text2)
        {
            return !(text1 == text2);
        }
    }
}
