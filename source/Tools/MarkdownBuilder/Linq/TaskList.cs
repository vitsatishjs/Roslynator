// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class TaskList : List
    {
        public TaskList()
        {
        }

        public TaskList(object content)
            : base(content)
        {
        }

        public TaskList(params object[] content)
            : base(content)
        {
        }

        public TaskList(TaskList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.TaskList;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (content is string s)
            {
                return builder.AppendTaskItem(s).AppendLine();
            }
            else
            {
                return builder.AppendTaskItems(Elements());
            }
        }

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
            return new TaskList(this);
        }
    }
}
