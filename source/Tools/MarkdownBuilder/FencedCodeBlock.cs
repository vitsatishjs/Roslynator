// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} Info = {InfoDebuggerDisplay} {Text,nq}")]
    public class FencedCodeBlock : MElement
    {
        internal FencedCodeBlock(string text, string info = null)
        {
            Text = text;

            //TODO: validate info
            Info = info;
        }

        public FencedCodeBlock(FencedCodeBlock other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Text = other.Text;
            Info = other.Info;
        }

        public string Text { get; set; }

        public string Info { get; set; }

        public override MarkdownKind Kind => MarkdownKind.FencedCodeBlock;

        private string InfoDebuggerDisplay => Info ?? "None";

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendFencedCodeBlock(Text, Info);
        }

        internal override MElement Clone()
        {
            return new FencedCodeBlock(this);
        }
    }
}
