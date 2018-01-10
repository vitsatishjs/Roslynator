﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class OrderedList : MContainer
    {
        public OrderedList()
        {
        }

        public OrderedList(object content)
            : base(content)
        {
        }

        public OrderedList(params object[] content)
            : base(content)
        {
        }

        public OrderedList(OrderedList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.OrderedList;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (content is string s)
            {
                return builder.AppendOrderedListItem(1, s).AppendLine();
            }
            else
            {
                return builder.AppendOrderedListItems(Elements());
            }
        }

        internal override MElement Clone()
        {
            return new OrderedList(this);
        }
    }
}