// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Text,nq}")]
    public class IndentedCodeBlock : MElement
    {
        public IndentedCodeBlock(string text)
        {
            Text = text;
        }

        public IndentedCodeBlock(IndentedCodeBlock other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Text = other.Text;
        }

        public string Text { get; set; }

        public override MarkdownKind Kind => MarkdownKind.IndentedCodeBlock;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendIndentedCodeBlock(Text);
        }

        internal override MElement Clone()
        {
            return new IndentedCodeBlock(this);
        }
    }
}
