// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {Text, nq}")]
    public class CodeText : MElement
    {
        public CodeText(string text)
        {
            Text = text;
        }

        public CodeText(CodeText other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Text = other.Text;
        }

        public string Text { get; }

        public override MarkdownKind Kind => MarkdownKind.Code;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendCode(Text);
        }

        internal override MElement Clone()
        {
            return new CodeText(this);
        }
    }
}
