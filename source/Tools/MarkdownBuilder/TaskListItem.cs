// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} IsCompleted = {IsCompleted}")]
    public struct TaskListItem : IMarkdown, IEquatable<TaskListItem>
    {
        internal TaskListItem(string text, bool isCompleted = false)
        {
            Text = text;
            IsCompleted = isCompleted;
        }

        public string Text { get; }

        public bool IsCompleted { get; }

        public TaskListItem WithText(string text)
        {
            return new TaskListItem(text, IsCompleted);
        }

        public TaskListItem WithIsCompleted(bool isCompleted)
        {
            return new TaskListItem(Text, isCompleted);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            if (IsCompleted)
            {
                return mb.AppendCompletedTaskListItem(Text);
            }
            else
            {
                return mb.AppendTaskListItem(Text);
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is TaskListItem other)
                && Equals(other);
        }

        public bool Equals(TaskListItem other)
        {
            return IsCompleted == other.IsCompleted
                   && Text == other.Text;
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Text, Hash.Create(IsCompleted));
        }

        public static bool operator ==(TaskListItem item1, TaskListItem item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(TaskListItem item1, TaskListItem item2)
        {
            return !(item1 == item2);
        }
    }
}
