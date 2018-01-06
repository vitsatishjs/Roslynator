// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} Number = {Number}")]
    public class OrderedListItem : ListItem
    {
        internal OrderedListItem(int number, object content)
            : base(content)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), number, ErrorMessages.OrderedListItemNumberCannotBeNegative);

            Number = number;
        }

        internal OrderedListItem(int number, params object[] content)
            : base(content)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), number, ErrorMessages.OrderedListItemNumberCannotBeNegative);

            Number = number;
        }

        public OrderedListItem(OrderedListItem other)
            : base(other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Number = other.Number;
        }

        public int Number { get; set; }

        public override MarkdownKind Kind => MarkdownKind.OrderedListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendOrderedListItem(Number, Elements());
        }

        internal override MElement Clone()
        {
            return new OrderedListItem(this);
        }
    }
}
