// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using static Pihrtsoft.Markdown.Tests.TestHelpers;

#pragma warning disable CS1718

namespace Pihrtsoft.Markdown.Tests
{
    public class TableColumnTests
    {
        [Fact]
        public void TableColumn_Equals()
        {
            TableColumn column = CreateTableColumn();

            Assert.True(column.Equals((object)column));
        }

        [Fact]
        public void TableColumn_NotEquals()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = column.Modify();

            Assert.False(column.Equals((object)column2));
        }

        [Fact]
        public void TableColumn_IEquatableEquals()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = column;
            IEquatable<TableColumn> equatable = column;

            Assert.True(equatable.Equals(column2));
        }

        [Fact]
        public void TableColumn_IEquatableNotEquals()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = CreateTableColumn().Modify();
            IEquatable<TableColumn> equatable = column;

            Assert.False(column.Equals(column2));
        }

        [Fact]
        public void TableColumn_GetHashCode_Equal()
        {
            TableColumn column = CreateTableColumn();

            Assert.Equal(column.GetHashCode(), column.GetHashCode());
        }

        [Fact]
        public void TableColumn_GetHashCode_NotEqual()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = column.Modify();

            Assert.NotEqual(column.GetHashCode(), column2.GetHashCode());
        }

        [Fact]
        public void TableColumn_OperatorEquals()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = column;

            Assert.True(column == column2);
        }

        [Fact]
        public void TableColumn_OperatorNotEquals()
        {
            TableColumn column = CreateTableColumn();
            TableColumn column2 = column.Modify();

            Assert.True(column != column2);
        }

        [Fact]
        public void TableColumn_Constructor_AssignName()
        {
            string name = TableColumnName();
            var column = new TableColumn(name: name, alignment: TableColumnAlignment());

            Assert.Equal(name, column.Name);
        }

        [Fact]
        public void TableColumn_WithName()
        {
            string name = TableColumnName();

            Assert.Equal(name, CreateTableColumn().WithName(name).Name);
        }

        [Fact]
        public void TableColumn_Constructor_AssignAlignment()
        {
            Alignment alignment = TableColumnAlignment();
            var column = new TableColumn(name: TableColumnName(), alignment: alignment);

            Assert.Equal(alignment, column.Alignment);
        }

        [Fact]
        public void TableColumn_WithAlignment()
        {
            TableColumn column = CreateTableColumn();
            Alignment alignment = column.Alignment.Modify();

            Assert.Equal(alignment, column.WithAlignment(alignment).Alignment);
        }
    }
}
