// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Number} {GetString(),nq}")]
    public class OrderedListItem : ListItem
    {
        private int _number;

        public OrderedListItem(int number)
        {
            Number = number;
        }

        public OrderedListItem(int number, object content)
            : base(content)
        {
            Number = number;
        }

        public OrderedListItem(int number, params object[] content)
            : base(content)
        {
            Number = number;
        }

        public OrderedListItem(OrderedListItem other)
            : base(other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Number = other.Number;
        }

        public int Number
        {
            get { return _number; }
            set
            {
                Error.ThrowOnInvalidItemNumber(value);
                _number = value;
            }
        }

        public override MarkdownKind Kind => MarkdownKind.OrderedListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendOrderedListItem(Number, TextOrElements());
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteOrderedListItem(Number, TextOrElements());
        }

        internal override MElement Clone()
        {
            return new OrderedListItem(this);
        }
    }
}
