// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Text, nq}")]
    public class InlineCode : MElement
    {
        public InlineCode(string text)
        {
            Text = text;
        }

        public InlineCode(InlineCode other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Text = other.Text;
        }

        public string Text { get; }

        public override MarkdownKind Kind => MarkdownKind.InlineCode;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendInlineCode(Text);
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteInlineCode(Text);
        }

        internal override MElement Clone()
        {
            return new InlineCode(this);
        }
    }
}
