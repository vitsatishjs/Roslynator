// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} IsCompleted = {IsCompleted}")]
    public class TaskListItem : MContainer, IMarkdown
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

        public bool IsCompleted { get; set; }

        public override MarkdownKind Kind => MarkdownKind.TaskListItem;

        //TODO: 
        //public TaskListItem WithText(string text)
        //{
        //    return new TaskListItem(text, IsCompleted);
        //}

        //public TaskListItem WithIsCompleted(bool isCompleted)
        //{
        //    return new TaskListItem(Text, isCompleted);
        //}

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            if (IsCompleted)
            {
                return builder.AppendCompletedTaskListItem(Elements);
            }
            else
            {
                return builder.AppendTaskListItem(Elements);
            }
        }

        //public override bool Equals(object obj)
        //{
        //    return (obj is TaskListItem other)
        //        && Equals(other);
        //}

        //public bool Equals(TaskListItem other)
        //{
        //    return IsCompleted == other.IsCompleted
        //           && Text == other.Text;
        //}

        //public override int GetHashCode()
        //{
        //    return Hash.Combine(Text, Hash.Create(IsCompleted));
        //}

        //public static bool operator ==(TaskListItem item1, TaskListItem item2)
        //{
        //    return item1.Equals(item2);
        //}

        //public static bool operator !=(TaskListItem item1, TaskListItem item2)
        //{
        //    return !(item1 == item2);
        //}
    }
}
