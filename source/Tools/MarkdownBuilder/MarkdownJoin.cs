// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Pihrtsoft.Markdown
{
    public class MarkdownJoin : IMarkdown
    {
        internal MarkdownJoin(object separator, IEnumerable<object> values)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
            Separator = separator;
        }

        public object Separator { get; }

        public IEnumerable<object> Values { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendJoin(Separator, Values);
        }
    }
}
