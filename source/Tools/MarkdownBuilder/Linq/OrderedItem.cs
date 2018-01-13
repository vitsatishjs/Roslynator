// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Number} {GetString(),nq}")]
    public class OrderedItem : MBlockContainer
    {
        private int _number;

        public OrderedItem(int number)
        {
            Number = number;
        }

        public OrderedItem(int number, object content)
            : base(content)
        {
            Number = number;
        }

        public OrderedItem(int number, params object[] content)
            : base(content)
        {
            Number = number;
        }

        public OrderedItem(OrderedItem other)
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

        public override MarkdownKind Kind => MarkdownKind.OrderedItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendOrderedItem(Number, TextOrElements());
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteOrderedItem(Number, TextOrElements());
        }

        internal override MElement Clone()
        {
            return new OrderedItem(this);
        }
    }
}
