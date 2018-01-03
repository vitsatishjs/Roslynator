// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("Separator = {Separator,nq} Escape = {Escape}")]
    public class MarkdownJoin : IMarkdown
    {
        internal MarkdownJoin(string separator, IEnumerable<object> values, bool escapeSeparator = true)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
            Separator = separator;
            EscapeSeparator = escapeSeparator;
        }

        public string Separator { get; }

        public IEnumerable<object> Values { get; }

        public bool EscapeSeparator { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            bool isFirst = true;

            foreach (object value in Values)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    mb.Append(Separator, escape: EscapeSeparator);
                }

                mb.Append(value);
            }

            return mb;
        }
    }
}
