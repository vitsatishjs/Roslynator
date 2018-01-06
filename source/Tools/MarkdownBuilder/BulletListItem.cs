// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class BulletListItem : ListItem
    {
        public BulletListItem(object content)
            : base(content)
        {
        }

        public BulletListItem(params object[] content)
            : base(content)
        {
        }

        public BulletListItem(BulletListItem other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.BulletListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendListItem(Elements());
        }

        internal override MElement Clone()
        {
            return new BulletListItem(this);
        }
    }
}
