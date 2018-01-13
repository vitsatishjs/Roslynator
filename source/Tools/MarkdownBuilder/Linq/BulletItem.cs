// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class BulletItem : MBlockContainer
    {
        public BulletItem()
        {
        }

        public BulletItem(object content)
            : base(content)
        {
        }

        public BulletItem(params object[] content)
            : base(content)
        {
        }

        public BulletItem(BulletItem other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.BulletItem;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteBulletItem(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new BulletItem(this);
        }
    }
}
