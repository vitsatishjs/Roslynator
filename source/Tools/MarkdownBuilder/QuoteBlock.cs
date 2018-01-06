// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind}")]
    public class QuoteBlock : MBlockContainer
    {
        internal QuoteBlock(object content)
            : base(content)
        {
        }

        internal QuoteBlock(params object[] content)
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
