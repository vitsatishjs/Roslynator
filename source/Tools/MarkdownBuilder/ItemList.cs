// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} IsCompleted = {IsCompleted}")]
    public class ItemList : MContainer
    {
        internal ItemList(bool isCompleted, object content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        internal ItemList(bool isCompleted, params object[] content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public bool IsCompleted { get; set; }

        public Collection<BulletListItem> Items { get; } = new Collection<BulletListItem>();

        public override MarkdownKind Kind => MarkdownKind.ItemList;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            //TODO: AppendItemList
            return builder;
        }

        internal override MElement Clone()
        {
            throw new NotImplementedException();
        }
    }
}
