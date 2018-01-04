// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Markdown
{
    //TODO: MTableRow, MRow
    public class TableRow : Collection<object>
    {
        public TableRow()
        {
        }

        public TableRow(IList<object> list)
            : base(list)
        {
        }

        internal TableRow(IEnumerable<object> values)
        {
            AddRange(values);
        }

        public void AddRange(IEnumerable<object> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (object value in values)
                Add(value);
        }
    }
}
