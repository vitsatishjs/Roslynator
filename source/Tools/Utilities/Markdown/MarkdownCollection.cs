// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roslynator.Utilities.Markdown
{
    public class MarkdownCollection<T> : Collection<T>
    {
        internal MarkdownCollection()
        {
        }

        internal MarkdownCollection(IList<T> list)
            : base(list)
        {
        }

        internal MarkdownCollection(IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (T value in values)
                Add(value);
        }

        public void AddRange(IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (T value in values)
                Add(value);
        }
    }
}