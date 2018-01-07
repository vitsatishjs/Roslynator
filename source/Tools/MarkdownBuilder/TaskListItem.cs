// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {ToString(),nq}")]
    public class TaskListItem : MBlockContainer
    {
        public TaskListItem(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

        public TaskListItem(bool isCompleted, object content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public TaskListItem(bool isCompleted, params object[] content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public TaskListItem(TaskListItem other)
            : base(other)
        {
            IsCompleted = other.IsCompleted;
        }

        public bool IsCompleted { get; set; }

        public override MarkdownKind Kind => MarkdownKind.TaskListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (IsCompleted)
            {
                return builder.AppendCompletedTaskListItem(TextOrElements());
            }
            else
            {
                return builder.AppendTaskListItem(TextOrElements());
            }
        }

        internal override MElement Clone()
        {
            return new TaskListItem(this);
        }
    }
}
