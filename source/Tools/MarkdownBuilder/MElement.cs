// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {GetString(),nq}")]
    public abstract class MElement : MObject
    {
        internal MElement next;

        public abstract MarkdownBuilder AppendTo(MarkdownBuilder builder);

        internal abstract MElement Clone();

        public override string ToString()
        {
            return ToString(null);
        }

        public string ToString(MarkdownFormat format)
        {
            var builder = new MarkdownBuilder(format ?? MarkdownFormat.Default);
            AppendTo(builder);
            return builder.ToString();
        }

        internal string GetString()
        {
            return ToString(MarkdownFormat.DebugFormat);
        }
    }
}