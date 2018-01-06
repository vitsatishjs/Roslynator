// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    //TODO: MarkdownTable
    [DebuggerDisplay("{Kind} Columns = {Columns.Count} Rows = {Rows.Count}")]
    public abstract class BaseTable<T> : MElement
    {
        internal BaseTable()
        {
        }

        public TableColumnCollection Columns { get; } = new TableColumnCollection();

        public Collection<T> Rows { get; } = new Collection<T>();

        public override MarkdownKind Kind => MarkdownKind.Table;

        internal override MElement Clone()
        {
            throw new NotImplementedException();
        }
    }
}