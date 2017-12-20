// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator.Utilities.Markdown
{
    public class TableRow : MarkdownCollection<object>
    {
        internal TableRow()
        {
        }

        internal TableRow(IList<object> list)
            : base(list)
        {
        }

        internal TableRow(IEnumerable<TableRow> values)
            : base(values)
        {
        }
    }
}