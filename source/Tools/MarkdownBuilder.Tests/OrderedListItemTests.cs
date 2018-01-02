// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using static Pihrtsoft.Markdown.Tests.TestHelpers;
using Xunit;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class OrderedOrderedListItemTests
    {
        [Fact]
        public void OrderedListItem_Equals()
        {
            OrderedListItem item = CreateOrderedListItem();

            Assert.True(item.Equals((object)item));
        }

        [Fact]
        public void OrderedListItem_NotEquals()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = item.Modify();

            Assert.False(item.Equals((object)item2));
        }

        [Fact]
        public void OrderedListItem_IEquatableEquals()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = item;
            IEquatable<OrderedListItem> equatable = item;

            Assert.True(equatable.Equals(item2));
        }

        [Fact]
        public void OrderedListItem_IEquatableNotEquals()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = CreateOrderedListItem().Modify();
            IEquatable<OrderedListItem> equatable = item;

            Assert.False(item.Equals(item2));
        }

        [Fact]
        public void OrderedListItem_GetHashCode_Equal()
        {
            OrderedListItem item = CreateOrderedListItem();

            Assert.Equal(item.GetHashCode(), item.GetHashCode());
        }

        [Fact]
        public void OrderedListItem_GetHashCode_NotEqual()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = item.Modify();

            Assert.NotEqual(item.GetHashCode(), item2.GetHashCode());
        }

        [Fact]
        public void OrderedListItem_OperatorEquals()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = item;

            Assert.True(item == item2);
        }

        [Fact]
        public void OrderedListItem_OperatorNotEquals()
        {
            OrderedListItem item = CreateOrderedListItem();
            OrderedListItem item2 = item.Modify();

            Assert.True(item != item2);
        }

        [Fact]
        public void OrderedListItem_Constructor_AssignNumber()
        {
            int number = OrderedListItemNumber();
            var item = new OrderedListItem(number: number, text: ListItemText());

            Assert.Equal(number, item.Number);
        }

        [Fact]
        public void OrderedListItem_WithNumber()
        {
            int number = OrderedListItemNumber();

            Assert.Equal(number, CreateOrderedListItem().WithNumber(number).Number);
        }

        [Fact]
        public void OrderedListItem_Constructor_AssignText()
        {
            string text = ListItemText();
            var item = new OrderedListItem(number: OrderedListItemNumber(), text: text);

            Assert.Equal(text, item.Text);
        }

        [Fact]
        public void OrderedListItem_WithText()
        {
            string text = ListItemText();

            Assert.Equal(text, CreateOrderedListItem().WithText(text).Text);
        }
    }
}
