// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MStrikethrough : MInlineContainer
    {
        public MStrikethrough()
        {
        }

        public MStrikethrough(object content)
            : base(content)
        {
        }

        public MStrikethrough(params object[] content)
            : base(content)
        {
        }

        public MStrikethrough(MStrikethrough other)
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
            return new MStrikethrough(this);
        }
    }
}
