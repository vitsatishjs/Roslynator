// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class StrikethroughText : MContainer
    {
        public StrikethroughText()
        {
        }

        public StrikethroughText(object content)
            : base(content)
        {
        }

        public StrikethroughText(params object[] content)
            : base(content)
        {
        }

        public StrikethroughText(StrikethroughText other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Strikethrough;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendStrikethrough(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new StrikethroughText(this);
        }
    }
}
