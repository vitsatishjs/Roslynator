// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class Italic : MInlineContainer
    {
        public Italic()
        {
        }

        public Italic(object content)
            : base(content)
        {
        }

        public Italic(params object[] content)
            : base(content)
        {
        }

        public Italic(Italic other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Italic;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteItalic(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new Italic(this);
        }
    }
}
