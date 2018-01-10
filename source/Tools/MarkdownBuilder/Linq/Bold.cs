// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class Bold : MInlineContainer
    {
        public Bold()
        {
        }

        public Bold(object content)
            : base(content)
        {
        }

        public Bold(params object[] content)
            : base(content)
        {
        }

        public Bold(Bold other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Bold;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendBold(TextOrElements());
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteBold(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new Bold(this);
        }
    }
}
