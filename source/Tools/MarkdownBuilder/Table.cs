// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Markdown
{
    public class Table<T> : BaseTable<T>
    {
        internal Table()
        {
        }

        public Collection<Func<T, object>> Selectors { get; } = new Collection<Func<T, object>>();

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendTable(Columns, Rows, Selectors);
        }

        public Table<T> AddColumns(params TableColumn[] columns)
        {
            return AddColumns((IEnumerable<TableColumn>)columns);
        }

        public Table<T> AddColumns(IEnumerable<TableColumn> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            foreach (TableColumn column in columns)
                Columns.Add(column);

            return this;
        }

        public Table<T> AddColumn(TableColumn column)
        {
            Columns.Add(column);
            return this;
        }

        public Table<T> AddColumns(TableColumn column1, TableColumn column2)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            return this;
        }

        public Table<T> AddColumns(TableColumn column1, TableColumn column2, TableColumn column3)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            return this;
        }

        public Table<T> AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            return this;
        }

        public Table<T> AddColumns(TableColumn column1, TableColumn column2, TableColumn column3, TableColumn column4, TableColumn column5)
        {
            Columns.Add(column1);
            Columns.Add(column2);
            Columns.Add(column3);
            Columns.Add(column4);
            Columns.Add(column5);
            return this;
        }
    }
}