// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator.Utilities.Markdown
{
    public class TableHeaderCollection : MarkdownCollection<TableHeader>
    {
        internal TableHeaderCollection()
        {
        }

        internal TableHeaderCollection(IList<TableHeader> list)
            : base(list)
        {
        }

        internal TableHeaderCollection(IEnumerable<TableHeader> values)
            : base(values)
        {
        }
    }
}