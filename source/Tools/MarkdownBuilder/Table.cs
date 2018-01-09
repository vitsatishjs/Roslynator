// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class Table : MContainer
    {
        internal Table()
        {
        }

        internal Table(object content)
            : base(content)
        {
        }

        internal Table(params object[] content)
            : base(content)
        {
        }

        internal Table(MContainer other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Table;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendTable(Elements());
        }

        internal override MElement Clone()
        {
            return new Table(this);
        }
    }
}