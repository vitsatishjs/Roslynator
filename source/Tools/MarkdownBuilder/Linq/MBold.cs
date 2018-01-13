// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MBold : MInlineContainer
    {
        public MBold()
        {
        }

        public MBold(object content)
            : base(content)
        {
        }

        public MBold(params object[] content)
            : base(content)
        {
        }

        public MBold(MBold other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Bold;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteBold(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new MBold(this);
        }
    }
}
