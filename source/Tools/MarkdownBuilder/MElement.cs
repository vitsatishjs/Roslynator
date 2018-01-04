// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public abstract class MElement : MObject
    {
        public abstract MarkdownBuilder AppendTo(MarkdownBuilder builder);

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
    }
}