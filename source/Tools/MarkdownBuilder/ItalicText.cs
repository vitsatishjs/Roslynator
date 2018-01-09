// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class ItalicText : MContainer
    {
        public ItalicText()
        {
        }

        public ItalicText(object content)
            : base(content)
        {
        }

        public ItalicText(params object[] content)
            : base(content)
        {
        }

        public ItalicText(ItalicText other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Italic;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendItalic(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new ItalicText(this);
        }
    }
}
