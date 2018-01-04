// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} Number = {Number}")]
    public class OrderedListItem : MContainer, IMarkdown
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

        public int Number { get; set; }

        public override MarkdownKind Kind => MarkdownKind.OrderedListItem;

        //TODO: 
        //public OrderedListItem WithNumber(int number)
        //{
        //    return new OrderedListItem(number, Text);
        //}

        //public OrderedListItem WithText(string text)
        //{
        //    return new OrderedListItem(Number, text);
        //}

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendOrderedListItem(Number, Elements);
        }

        //TODO: 
        //public override bool Equals(object obj)
        //{
        //    return (obj is OrderedListItem other)
        //        && Equals(other);
        //}

        //public bool Equals(OrderedListItem other)
        //{
        //    return Number == other.Number
        //        && string.Equals(Text, other.Text, StringComparison.Ordinal);
        //}

        //public override int GetHashCode()
        //{
        //    return Hash.Combine(Number, Hash.Create(Text));
        //}

        //public static bool operator ==(OrderedListItem item1, OrderedListItem item2)
        //{
        //    return item1.Equals(item2);
        //}

        //public static bool operator !=(OrderedListItem item1, OrderedListItem item2)
        //{
        //    return !(item1 == item2);
        //}
    }
}
