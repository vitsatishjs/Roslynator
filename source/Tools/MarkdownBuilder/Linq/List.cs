// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public abstract class List : MContainer
    {
        protected List()
        {
        }

        protected List(object content)
            : base(content)
        {
        }

        protected List(params object[] content)
            : base(content)
        {
        }

        protected List(List other)
            : base(other)
        {
        }

        internal override bool AllowStringConcatenation => false;

        internal override void ValidateElement(MElement element)
        {
            switch (element.Kind)
            {
                case MarkdownKind.BulletItem:
                case MarkdownKind.OrderedItem:
                case MarkdownKind.TaskItem:
                    return;
            }

            base.ValidateElement(element);
        }
    }
}
