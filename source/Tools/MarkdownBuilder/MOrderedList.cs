// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    //TODO: MUnorderedList
    public class MOrderedList : MContainer
    {
        public MOrderedList()
        {
        }

        public MOrderedList(object content)
            : base(content)
        {
        }

        public MOrderedList(params object[] content)
            : base(content)
        {
        }

        public MOrderedList(MOrderedList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.OrderedList;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendOrderedItems(Elements());
        }

        internal override MElement Clone()
        {
            return new MOrderedList(this);
        }
    }
}
