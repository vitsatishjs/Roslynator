// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roslynator.Utilities.Markdown
{
    public class TableRow : Collection<object>
    {
        public TableRow()
        {
        }

        public TableRow(IList<object> list) : base(list)
        {
        }

        public void AddRange(object value1, object value2)
        {
            Add(value1);
            Add(value2);
        }

        public void AddRange(object value1, object value2, object value3)
        {
            Add(value1);
            Add(value2);
            Add(value3);
        }

        public void AddRange(object value1, object value2, object value3, object value4)
        {
            Add(value1);
            Add(value2);
            Add(value3);
            Add(value4);
        }

        public void AddRange(object value1, object value2, object value3, object value4, object value5)
        {
            Add(value1);
            Add(value2);
            Add(value3);
            Add(value4);
            Add(value5);
        }

        public void AddRange(object value1, object value2, object value3, object value4, object value5, object value6)
        {
            Add(value1);
            Add(value2);
            Add(value3);
            Add(value4);
            Add(value5);
            Add(value6);
        }
    }
}