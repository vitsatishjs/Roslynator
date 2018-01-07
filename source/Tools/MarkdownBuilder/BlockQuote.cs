// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class BlockQuote : MBlockContainer
    {
        public BlockQuote()
        {
        }

        public BlockQuote(object content)
            : base(content)
        {
        }

        public BlockQuote(params object[] content)
            : base(content)
        {
        }

        public BlockQuote(BlockQuote other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.BlockQuote;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendBlockQuote(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new BlockQuote(this);
        }
    }
}
