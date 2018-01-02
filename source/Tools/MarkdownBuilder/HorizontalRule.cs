// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    public struct HorizontalRule : IMarkdown, IEquatable<HorizontalRule>
    {
        internal HorizontalRule(HorizontalRuleStyle style, int count = 3, bool addSpaces = true)
        {
            if (count < 3)
                throw new ArgumentOutOfRangeException(nameof(count), count, ErrorMessages.NumberOfCharactersInHorizontalRuleCannotBeLessThanThree);

            Style = style;
            Count = count;
            AddSpaces = addSpaces;
        }

        public static HorizontalRule Default { get; } = new HorizontalRule(HorizontalRuleStyle.Hyphen, count: 3, addSpaces: true);

        public HorizontalRuleStyle Style { get; }

        public int Count { get; }

        //TODO: Spaces
        public bool AddSpaces { get; }

        public HorizontalRule WithStyle(HorizontalRuleStyle style)
        {
            return new HorizontalRule(style, Count, AddSpaces);
        }

        public HorizontalRule WithCount(int count)
        {
            return new HorizontalRule(Style, count, AddSpaces);
        }

        public HorizontalRule WithAddSpaces(bool addSpaces)
        {
            return new HorizontalRule(Style, Count, addSpaces);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendHorizontalRule(Style, Count, AddSpaces);
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
                && AddSpaces == other.AddSpaces;
        }

        public override int GetHashCode()
        {
            return Hash.Combine((int)Style, Hash.Combine(Count, Hash.Create(AddSpaces)));
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
