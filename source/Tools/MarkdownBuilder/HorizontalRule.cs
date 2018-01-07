// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    public class HorizontalRule : MElement
    {
        internal HorizontalRule(HorizontalRuleStyle style, int count = 3, string space = " ")
        {
            ThrowOnInvalidCount(count);

            Style = style;
            Count = count;
            Space = space ?? "";
        }

        public HorizontalRule(HorizontalRuleFormat format)
            : this(format.Style, format.Count, format.Space)
        {

        }

        public HorizontalRule(HorizontalRule other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Style = other.Style;
            Count = other.Count;
            Space = other.Space;
        }

        public HorizontalRuleStyle Style { get; set; }

        public int Count { get; set; }

        public string Space { get; set; }

        public override MarkdownKind Kind => MarkdownKind.HorizontalRule;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendHorizontalRule(Style, Count, Space);
        }

        internal override MElement Clone()
        {
            return new HorizontalRule(this);
        }

        internal static void ThrowOnInvalidCount(int count)
        {
            if (count < 3)
                throw new ArgumentOutOfRangeException(nameof(count), count, ErrorMessages.NumberOfCharactersInHorizontalRuleCannotBeLessThanThree);
        }
    }
}
