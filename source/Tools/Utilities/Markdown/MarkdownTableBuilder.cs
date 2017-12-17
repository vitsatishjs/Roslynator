// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownTableBuilder : IMarkdown
    {
        public List<MarkdownTableHeader> Headers { get; } = new List<MarkdownTableHeader>();

        public List<IList<MarkdownText>> Rows { get; } = new List<IList<MarkdownText>>();

        public void WriteTo(MarkdownWriter mw)
        {
            IFormatProvider formatProvider = mw.FormatProvider;
            MarkdownSettings settings = mw.Settings;

            int headerCount = Headers.Count;

            if (headerCount == 0)
                return;

            switch (settings.TableFormatting)
            {
                case MarkdownTableFormatting.None:
                case MarkdownTableFormatting.Header:
                    {
                        mw.WriteTableHeader(Headers, headerCount);

                        foreach (IList<MarkdownText> row in Rows)
                            mw.WriteTableRow(row, headerCount);

                        break;
                    }
                case MarkdownTableFormatting.Content:
                    {
                        List<int> widths = CalculateColumnWidths(null, Rows, formatProvider, settings);

                        mw.WriteTableHeader(Headers, headerCount);

                        foreach (IList<MarkdownText> row in Rows)
                            mw.WriteTableRow(row, headerCount, widths);

                        break;
                    }
                case MarkdownTableFormatting.All:
                    {
                        List<int> widths = CalculateColumnWidths(Headers, Rows, formatProvider, settings);

                        mw.WriteTableHeader(Headers, headerCount, widths);

                        foreach (IList<MarkdownText> row in Rows)
                            mw.WriteTableRow(row, headerCount, widths);

                        break;
                    }
                default:
                    {
                        Debug.Fail(settings.TableFormatting.ToString());
                        break;
                    }
            }
        }

        private List<int> CalculateColumnWidths(List<MarkdownTableHeader> headers, List<IList<MarkdownText>> rows, IFormatProvider formatProvider, MarkdownSettings settings)
        {
            var widths = new List<int>();

            var sb = new StringBuilder();

            int index = 0;

            using (var mw = new MarkdownWriter(sb, formatProvider, settings))
            {
                if (headers != null)
                {
                    foreach (MarkdownTableHeader header in headers)
                    {
                        mw.WriteMarkdown(header.Name);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                foreach (MarkdownText[] row in rows)
                {
                    for (int i = 0; i < row.Length; i++)
                    {
                        mw.WriteMarkdown(row[i]);
                        widths.Add(sb.Length - index);
                        index = sb.Length;
                    }
                }

                int headerCount = headers.Count;

                int count = widths.Count;

                var maxWidths = new List<int>();

                for (int i = 0; i < headerCount; i++)
                {
                    maxWidths.Add(0);

                    for (int j = i; j < count; j += headerCount)
                    {
                        maxWidths[i] = Math.Max(maxWidths[i], widths[j]);
                    }
                }

                return maxWidths;
            }
        }

        public void AddHeaders(string name1, string name2)
        {
            Headers.Add(name1);
            Headers.Add(name2);
        }

        public void AddHeaders(string name1, string name2, string name3)
        {
            Headers.Add(name1);
            Headers.Add(name2);
            Headers.Add(name3);
        }

        public void AddHeaders(string name1, string name2, string name3, string name4)
        {
            Headers.Add(name1);
            Headers.Add(name2);
            Headers.Add(name3);
            Headers.Add(name4);
        }

        public void AddHeaders(string name1, string name2, string name3, string name4, string name5)
        {
            Headers.Add(name1);
            Headers.Add(name2);
            Headers.Add(name3);
            Headers.Add(name4);
            Headers.Add(name5);
        }

        public void AddHeaders(string name1, string name2, string name3, string name4, string name5, string name6)
        {
            Headers.Add(name1);
            Headers.Add(name2);
            Headers.Add(name3);
            Headers.Add(name4);
            Headers.Add(name5);
            Headers.Add(name6);
        }

        public void AddRow(MarkdownText value1, MarkdownText value2)
        {
            Rows.Add(new MarkdownText[] { value1, value2 });
        }

        public void AddRow(MarkdownText value1, MarkdownText value2, MarkdownText value3)
        {
            Rows.Add(new MarkdownText[] { value1, value2, value3 });
        }

        public void AddRow(MarkdownText value1, MarkdownText value2, MarkdownText value3, MarkdownText value4)
        {
            Rows.Add(new MarkdownText[] { value1, value2, value3, value4 });
        }

        public void AddRow(MarkdownText value1, MarkdownText value2, MarkdownText value3, MarkdownText value4, MarkdownText value5)
        {
            Rows.Add(new MarkdownText[] { value1, value2, value3, value4, value5 });
        }

        public void AddRow(MarkdownText value1, MarkdownText value2, MarkdownText value3, MarkdownText value4, MarkdownText value5, MarkdownText value6)
        {
            Rows.Add(new MarkdownText[] { value1, value2, value3, value4, value5, value6 });
        }
    }
}