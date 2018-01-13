// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {GetString(),nq}")]
    public class TaskItem : MBlockContainer
    {
        public TaskItem(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

        public TaskItem(bool isCompleted, object content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public TaskItem(bool isCompleted, params object[] content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public TaskItem(TaskItem other)
            : base(other)
        {
            IsCompleted = other.IsCompleted;
        }

        public bool IsCompleted { get; set; }

        public override MarkdownKind Kind => MarkdownKind.TaskItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (IsCompleted)
            {
                return builder.AppendCompletedTaskItem(TextOrElements());
            }
            else
            {
                return builder.AppendTaskItem(TextOrElements());
            }
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            if (IsCompleted)
            {
                return writer.WriteCompletedTaskItem(TextOrElements());
            }
            else
            {
                return writer.WriteTaskItem(TextOrElements());
            }
        }

        internal override MElement Clone()
        {
            return new TaskItem(this);
        }
    }
}
