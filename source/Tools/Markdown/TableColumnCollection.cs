// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roslynator.Markdown
{
    //TODO: TableHeader
    public class TableColumnCollection : Collection<TableColumn>
    {
        internal TableColumnCollection()
        {
        }

        internal TableColumnCollection(IList<TableColumn> list)
            : base(list)
        {
        }

        internal TableColumnCollection(IEnumerable<TableColumn> columns)
        {
            AddRange(columns);
        }

        public void AddRange(IEnumerable<TableColumn> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            foreach (TableColumn column in columns)
                Add(column);
        }
    }
}