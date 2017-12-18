// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roslynator.Utilities.Markdown
{
    public class TableRowCollection : Collection<TableRow>
    {
        public TableRowCollection()
        {
        }

        public TableRowCollection(IList<TableRow> list) : base(list)
        {
        }
    }
}