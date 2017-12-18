// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public class Table : IMarkdown
    {
        public TableHeaderCollection Headers { get; } = new TableHeaderCollection();

        public TableRowCollection Rows { get; } = new TableRowCollection();

        public void WriteTo(MarkdownWriter mw)
        {
            int columnCount = Headers.Count;

            if (columnCount == 0)
                return;

            MarkdownSettings settings = mw.Settings;

            if (!settings.FormatTableContent)
            {
                mw.WriteTableHeader(Headers, columnCount);
                mw.WriteTableRows(Rows, columnCount);
            }
            else
            {
                bool formatHeader = settings.FormatTableHeader;

                List<int> widths = CalculateWidths((formatHeader) ? Headers : null, columnCount, Rows, mw.FormatProvider, settings);

                mw.WriteTableHeader(Headers, columnCount, (formatHeader) ? widths : null);
                mw.WriteTableRows(Rows, columnCount, widths);
            }
        }

        private List<int> CalculateWidths(TableHeaderCollection headers, int columnCount, TableRowCollection rows, IFormatProvider formatProvider, MarkdownSettings settings)
        {
            var widths = new List<int>();

            var sb = new StringBuilder();

            int index = 0;

            using (var mw = new MarkdownWriter(sb, formatProvider, settings))
            {
                if (headers != null)
                {
                    foreach (TableHeader header in headers)
                    {
                        mw.WriteMarkdown(header.Name);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                foreach (TableRow row in rows)
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        mw.Write(row[i]);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                int count = widths.Count;

                var maxWidths = new List<int>();

                for (int i = 0; i < columnCount; i++)
                {
                    maxWidths.Add(0);

                    for (int j = i; j < count; j += columnCount)
                    {
                        maxWidths[i] = Math.Max(maxWidths[i], widths[j]);
                    }
                }

                return maxWidths;
            }
        }

        public void AddHeaders(params TableHeader[] headers)
        {
            foreach (TableHeader header in headers)
                Headers.Add(header);
        }

        public void AddHeaders(TableHeader header1, TableHeader header2)
        {
            Headers.Add(header1);
            Headers.Add(header2);
        }

        public void AddHeaders(TableHeader header1, TableHeader header2, TableHeader header3)
        {
            Headers.Add(header1);
            Headers.Add(header2);
            Headers.Add(header3);
        }

        public void AddHeaders(TableHeader header1, TableHeader header2, TableHeader header3, TableHeader header4)
        {
            Headers.Add(header1);
            Headers.Add(header2);
            Headers.Add(header3);
            Headers.Add(header4);
        }

        public void AddHeaders(TableHeader header1, TableHeader header2, TableHeader header3, TableHeader header4, TableHeader header5)
        {
            Headers.Add(header1);
            Headers.Add(header2);
            Headers.Add(header3);
            Headers.Add(header4);
            Headers.Add(header5);
        }

        public void AddHeaders(TableHeader header1, TableHeader header2, TableHeader header3, TableHeader header4, TableHeader header5, TableHeader header6)
        {
            Headers.Add(header1);
            Headers.Add(header2);
            Headers.Add(header3);
            Headers.Add(header4);
            Headers.Add(header5);
            Headers.Add(header6);
        }

        public void AddRow(params object[] values)
        {
            var rowValues = new TableRow();

            foreach (object value in values)
                rowValues.Add(value);

            Rows.Add(rowValues);
        }

        public void AddRow(object value)
        {
            Rows.Add(new TableRow() { value });
        }

        public void AddRow(object value1, object value2)
        {
            Rows.Add(new TableRow() { value1, value2 });
        }

        public void AddRow(object value1, object value2, object value3)
        {
            Rows.Add(new TableRow() { value1, value2, value3 });
        }

        public void AddRow(object value1, object value2, object value3, object value4)
        {
            Rows.Add(new TableRow() { value1, value2, value3, value4 });
        }

        public void AddRow(object value1, object value2, object value3, object value4, object value5)
        {
            Rows.Add(new TableRow() { value1, value2, value3, value4, value5 });
        }

        public void AddRow(object value1, object value2, object value3, object value4, object value5, object value6)
        {
            Rows.Add(new TableRow() { value1, value2, value3, value4, value5, value6 });
        }
    }
}