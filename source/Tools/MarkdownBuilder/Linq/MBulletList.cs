// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MBulletList : MList
    {
        public MBulletList()
        {
        }

        public MBulletList(object content)
            : base(content)
        {
        }

        public MBulletList(params object[] content)
            : base(content)
        {
        }

        public MBulletList(MBulletList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.List;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            if (content is string s)
            {
                return writer.WriteBulletItem(s).WriteLine();
            }
            else
            {
                return writer.WriteBulletItems(Elements());
            }
        }

        internal override MElement Clone()
        {
            return new MBulletList(this);
        }
    }
}
