// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Number}. {Text,nq}")]
    public struct OrderedListItem : IMarkdown, IEquatable<OrderedListItem>
    {
        internal OrderedListItem(int number, string text)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), number, ErrorMessages.OrderedListItemNumberCannotBeNegative);

            Number = number;
            Text = text;
        }

        public int Number { get; }

        public string Text { get; }

        public OrderedListItem WithNumber(int number)
        {
            return new OrderedListItem(number, Text);
        }

        public OrderedListItem WithText(string text)
        {
            return new OrderedListItem(Number, text);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendOrderedListItem(Number, Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is OrderedListItem other)
                && Equals(other);
        }

        public bool Equals(OrderedListItem other)
        {
            return Number == other.Number
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Number, Hash.Create(Text));
        }

        public static bool operator ==(OrderedListItem item1, OrderedListItem item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(OrderedListItem item1, OrderedListItem item2)
        {
            return !(item1 == item2);
        }
    }
}
