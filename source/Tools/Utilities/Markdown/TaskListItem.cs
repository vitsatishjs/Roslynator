// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.Utilities.Markdown
{
    public struct TaskListItem : IMarkdown
    {
        internal TaskListItem(string text, bool isCompleted = false)
        {
            Text = text;
            IsCompleted = isCompleted;
        }

        public string Text { get; }

        public bool IsCompleted { get; }

        public void WriteTo(MarkdownWriter mw)
        {
            mw.WriteTaskListItem(Text, IsCompleted);
        }
    }
}
