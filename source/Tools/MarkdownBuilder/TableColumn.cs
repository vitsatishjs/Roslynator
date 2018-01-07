// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} Alignment = {Alignment} {ToString(),nq}")]
    public class TableColumn : MContainer
    {
        public TableColumn(Alignment alignment)
        {
            Alignment = alignment;
        }

        public TableColumn(Alignment alignment, object content)
            : base(content)
        {
            Alignment = alignment;
        }

        public TableColumn(Alignment alignment, params object[] content)
            : base(content)
        {
            Alignment = alignment;
        }

        public TableColumn(TableColumn other)
            : base(other)
        {
            Alignment = other.Alignment;
        }

        public Alignment Alignment { get; set; }

        public override MarkdownKind Kind => MarkdownKind.TableColumn;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.Append(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new TableColumn(this);
        }
    }
}
