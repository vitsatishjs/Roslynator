// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public abstract class ListItem : MBlockContainer
    {
        protected ListItem()
        {
        }

        protected ListItem(object content)
            : base(content)
        {
        }

        protected ListItem(params object[] content)
            : base(content)
        {
        }

        protected ListItem(ListItem other)
            : base(other)
        {
        }
    }
}
