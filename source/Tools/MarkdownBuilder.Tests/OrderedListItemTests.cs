// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Pihrtsoft.Markdown.Tests.TestHelpers;
using Xunit;
using Pihrtsoft.Markdown.Linq;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class OrderedOrderedListItemTests
    {
        [Fact]
        public void OrderedListItem_Equals()
        {
            MOrderedItem item = CreateOrderedListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void OrderedListItem_GetHashCode_Equal()
        {
            MOrderedItem item = CreateOrderedListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void OrderedListItem_OperatorEquals()
        {
            MOrderedItem item = CreateOrderedListItem();
            MOrderedItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void OrderedListItem_Constructor_AssignNumber()
        {
            int number = OrderedListItemNumber();
            var item = new MOrderedItem(number: number, content: StringValue());

            Assert.Equal(number, item.Number);
        }
    }
}
