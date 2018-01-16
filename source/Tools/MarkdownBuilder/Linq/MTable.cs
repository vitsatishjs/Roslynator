// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Markdown.Linq
{
    public class MTable : MContainer
    {
        internal MTable()
        {
        }

        internal MTable(object content)
            : base(content)
        {
        }

        internal MTable(params object[] content)
            : base(content)
        {
        }

        internal MTable(MContainer other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Table;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            IReadOnlyList<TableColumnInfo> columns = writer.AnalyzeTable(Elements());

            if (columns == null)
                return writer;

            writer.WriteTableStart(columns);

            using (IEnumerator<MElement> en = Elements().GetEnumerator())
            {
                if (en.MoveNext())
                {
                    writer.WriteTableRow(en.Current);

                    while (en.MoveNext())
                        writer.WriteTableRow(en.Current);
                }
            }

            writer.WriteTableEnd();

            return writer;
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