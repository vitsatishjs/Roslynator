// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MBulletItem : MBlockContainer
    {
        public MBulletItem()
        {
        }

        public MBulletItem(object content)
            : base(content)
        {
        }

        public MBulletItem(params object[] content)
            : base(content)
        {
        }

        public MBulletItem(MBulletItem other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.BulletItem;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            writer.WriteStartBulletItem();
            WriteContentTo(writer);
            writer.WriteEndBulletItem();
            return writer;
        }

        internal override MElement Clone()
        {
            return new MBulletItem(this);
        }
    }
}
