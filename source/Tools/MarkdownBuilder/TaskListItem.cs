// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} IsCompleted = {IsCompleted}")]
    public class TaskListItem : ListItem
    {
        internal TaskListItem(bool isCompleted, object content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        internal TaskListItem(bool isCompleted, params object[] content)
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
                return builder.AppendCompletedTaskListItem(Elements());
            }
            else
            {
                return builder.AppendTaskListItem(Elements());
            }
        }

        internal override MElement Clone()
        {
            return new TaskListItem(this);
        }
    }
}
