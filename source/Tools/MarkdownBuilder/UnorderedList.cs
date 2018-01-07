// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class UnorderedList : MContainer
    {
        public UnorderedList()
        {
        }

        public UnorderedList(object content)
            : base(content)
        {
        }

        public UnorderedList(params object[] content)
            : base(content)
        {
        }

        public UnorderedList(UnorderedList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.List;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (content is string s)
            {
                return builder.AppendListItem(s);
            }
            else
            {
                return builder.AppendListItems(Elements());
            }
        }

        internal override MElement Clone()
        {
            return new UnorderedList(this);
        }
    }
}
