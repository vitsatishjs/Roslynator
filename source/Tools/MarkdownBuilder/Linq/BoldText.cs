// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    //TODO: MBold
    public class BoldText : MContainer
    {
        public BoldText()
        {
        }

        public BoldText(object content)
            : base(content)
        {
        }

        public BoldText(params object[] content)
            : base(content)
        {
        }

        public BoldText(BoldText other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Bold;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendBold(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new BoldText(this);
        }
    }
}
