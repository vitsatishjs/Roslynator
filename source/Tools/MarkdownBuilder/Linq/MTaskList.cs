// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MTaskList : MList
    {
        public MTaskList()
        {
        }

        public MTaskList(object content)
            : base(content)
        {
        }

        public MTaskList(params object[] content)
            : base(content)
        {
        }

        public MTaskList(MTaskList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.TaskList;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            if (content is string s)
            {
                return writer.WriteTaskItem(s).WriteLine();
            }
            else
            {
                return writer.WriteTaskItems(Elements());
            }
        }

        internal override MElement Clone()
        {
            return new MTaskList(this);
        }
    }
}
