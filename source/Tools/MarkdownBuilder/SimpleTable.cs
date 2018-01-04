// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using static Pihrtsoft.Markdown.MarkdownFactory;

namespace Pihrtsoft.Markdown
{
    public class SimpleTable : BaseTable<TableRow>
    {
        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendTable(Columns, Rows);
        }

        public SimpleTable AddColumns(params TableColumn[] columns)
        {
            return AddColumns((IEnumerable<TableColumn>)columns);
        }

        public SimpleTable AddColumns(IEnumerable<TableColumn> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            foreach (TableColumn column in columns)
                Columns.Add(column);

            return this;
        }

        public SimpleTable AddColumn(TableColumn column)
        {
            Columns.Add(column);
            return this;
        }

        public SimpleTable AddColumns(TableColumn column1, TableColumn column2)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            return this;
        }

        public SimpleTable AddColumns(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            return this;
        }

        public SimpleTable AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            return this;
        }

        public SimpleTable AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            Columns.Add(column5);
            return this;
        }

        public SimpleTable AddRowIf(bool condition, params object[] values)
        {
            return (condition) ? AddRow(values) : this;
        }

        public SimpleTable AddRow(params object[] values)
        {
            Rows.Add(TableRow(values));
            return this;
        }

        public SimpleTable AddRow(object value)
        {
            Rows.Add(TableRow(value));
            return this;
        }

        public SimpleTable AddRow(object value1, object value2)
        {
            Rows.Add(TableRow(value1, value2));
            return this;
        }

        public SimpleTable AddRow(object value1, object value2, object value3)
        {
            Rows.Add(TableRow(value1, value2, value3));
            return this;
        }

        public SimpleTable AddRow(object value1, object value2, object value3, object value4)
        {
            Rows.Add(TableRow(value1, value2, value3, value4));
            return this;
        }

        public SimpleTable AddRow(object value1, object value2, object value3, object value4, object value5)
        {
            Rows.Add(TableRow(value1, value2, value3, value4, value5));
            return this;
        }
    }
}