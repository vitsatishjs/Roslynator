// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    public class HorizontalRule : MElement, IEquatable<HorizontalRule>, IMarkdown
    {
        internal HorizontalRule(HorizontalRuleStyle style, int count = 3, string space = " ")
        {
            if (count < 3)
                throw new ArgumentOutOfRangeException(nameof(count), count, ErrorMessages.NumberOfCharactersInHorizontalRuleCannotBeLessThanThree);

            Style = style;
            Count = count;
            Space = space ?? "";
        }

        public static HorizontalRule Default { get; } = new HorizontalRule(HorizontalRuleStyle.Hyphen, count: 3, space: " ");

        public HorizontalRuleStyle Style { get; }

        public int Count { get; }

        public string Space { get; }

        public override MarkdownKind Kind => MarkdownKind.HorizontalRule;

        public HorizontalRule WithStyle(HorizontalRuleStyle style)
        {
            return new HorizontalRule(style, Count, Space);
        }

        public HorizontalRule WithCount(int count)
        {
            return new HorizontalRule(Style, count, Space);
        }

        public HorizontalRule WithSpace(string space)
        {
            return new HorizontalRule(Style, Count, space);
        }

        public override MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendHorizontalRule(Style, Count, Space);
        }

        public override bool Equals(object obj)
        {
            return (obj is HorizontalRule other)
                && Equals(other);
        }

        public bool Equals(HorizontalRule other)
        {
            return Style == other.Style
                && Count == other.Count
                && Space == other.Space;
        }

        public override int GetHashCode()
        {
            return Hash.Combine((int)Style, Hash.Combine(Count, Hash.Create(Space)));
        }

        public static bool operator ==(HorizontalRule left, HorizontalRule right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HorizontalRule left, HorizontalRule right)
        {
            return !(left == right);
        }
    }
}
