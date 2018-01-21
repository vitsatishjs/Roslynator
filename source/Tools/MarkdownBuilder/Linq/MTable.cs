// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Pihrtsoft.Markdown.Linq
{
    public class MTable : MContainer
    {
        public MTable()
        {
        }

        public MTable(object content)
            : base(content)
        {
        }

        public MTable(params object[] content)
            : base(content)
        {
        }

        public MTable(MContainer other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Table;

        public override void WriteTo(MarkdownWriter writer)
        {
            IEnumerable<MElement> rows = Elements();

            IReadOnlyList<TableColumnInfo> columns = writer.AnalyzeTable(rows);

            if (columns == null)
                return;

            writer.WriteStartTable(columns);

            using (IEnumerator<MElement> en = rows.GetEnumerator())
            {
                if (en.MoveNext())
                {
                    writer.WriteTableRow(en.Current);
                    writer.WriteTableHeaderSeparator();

                    while (en.MoveNext())
                        writer.WriteTableRow(en.Current);
                }
            }

            writer.WriteEndTable();
        }

        internal override MElement Clone()
        {
            return new MTable(this);
        }

        internal override void ValidateElement(MElement element)
        {
            switch (element.Kind)
            {
                case MarkdownKind.TableColumn:
                case MarkdownKind.TableRow:
                    return;
            }

            base.ValidateElement(element);
        }
    }
}