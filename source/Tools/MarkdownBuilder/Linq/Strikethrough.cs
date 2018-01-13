// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class Strikethrough : MInlineContainer
    {
        public Strikethrough()
        {
        }

        public Strikethrough(object content)
            : base(content)
        {
        }

        public Strikethrough(params object[] content)
            : base(content)
        {
        }

        public Strikethrough(Strikethrough other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Strikethrough;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteStrikethrough(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new Strikethrough(this);
        }
    }
}
