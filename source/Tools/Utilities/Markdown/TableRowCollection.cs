// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator.Utilities.Markdown
{
    public class TableRowCollection : MarkdownCollection<TableRow>
    {
        internal TableRowCollection()
        {
        }

        internal TableRowCollection(IList<TableRow> list)
            : base(list)
        {
        }

        internal TableRowCollection(IEnumerable<TableRow> values)
            : base(values)
        {
        }
    }
}