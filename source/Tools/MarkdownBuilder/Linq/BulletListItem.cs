// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class BulletListItem : ListItem
    {
        public BulletListItem()
        {
        }

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

        public override MarkdownKind Kind => MarkdownKind.ListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendListItem(TextOrElements());
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteListItem(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new BulletListItem(this);
        }
    }
}
