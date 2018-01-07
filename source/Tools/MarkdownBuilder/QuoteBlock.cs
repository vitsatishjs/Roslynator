// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    //TODO: BlockQuote
    public class QuoteBlock : MBlockContainer
    {
        public QuoteBlock()
        {
        }

        public QuoteBlock(object content)
            : base(content)
        {
        }

        public QuoteBlock(params object[] content)
            : base(content)
        {
        }

        public QuoteBlock(QuoteBlock other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.QuoteBlock;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendQuoteBlock(Elements());
        }

        internal override MElement Clone()
        {
            return new QuoteBlock(this);
        }
    }
}
