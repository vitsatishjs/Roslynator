// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static Pihrtsoft.Markdown.MarkdownFactory;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Columns = {Columns.Count} Rows = {Rows.Count}")]
    public class Table : IMarkdown
    {
        internal Table()
        {
        }

        public TableColumnCollection Columns { get; } = new TableColumnCollection();

        public Collection<IList<object>> Rows { get; } = new Collection<IList<object>>();

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendTable(Columns, Rows);
        }

        public Table AddColumns(params TableColumn[] columns)
        {
            foreach (TableColumn column in columns)
                Columns.Add(column);

            return this;
        }

        public Table AddColumn(TableColumn column)
        {
            Columns.Add(column);
            return this;
        }

        public Table AddColumns(TableColumn column1, TableColumn column2)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            return this;
        }

        public Table AddColumns(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            return this;
        }

        public Table AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            return this;
        }

        public Table AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            Columns.Add(column5);
            return this;
        }

        public Table AddRowIf(bool condition, params object[] values)
        {
            return (condition) ? AddRow(values) : this;
        }

        public Table AddRow(params object[] values)
        {
            Rows.Add(values);
            return this;
        }

        public Table AddRow(object value)
        {
            Rows.Add(TableRow(value));
            return this;
        }

        public Table AddRow(object value1, object value2)
        {
            Rows.Add(TableRow(value1, value2));
            return this;
        }

        public Table AddRow(object value1, object value2, object value3)
        {
            Rows.Add(TableRow(value1, value2, value3));
            return this;
        }

        public Table AddRow(object value1, object value2, object value3, object value4)
        {
            Rows.Add(TableRow(value1, value2, value3, value4));
            return this;
        }

        public Table AddRow(object value1, object value2, object value3, object value4, object value5)
        {
            Rows.Add(TableRow(value1, value2, value3, value4, value5));
            return this;
        }
    }
}