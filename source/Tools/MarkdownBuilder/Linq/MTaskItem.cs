// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {GetString(),nq}")]
    public class MTaskItem : MBlockContainer
    {
        public MTaskItem(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

        public MTaskItem(bool isCompleted, object content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public MTaskItem(bool isCompleted, params object[] content)
            : base(content)
        {
            IsCompleted = isCompleted;
        }

        public MTaskItem(MTaskItem other)
            : base(other)
        {
            IsCompleted = other.IsCompleted;
        }

        public bool IsCompleted { get; set; }

        public override MarkdownKind Kind => MarkdownKind.TaskItem;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            writer.WriteStartTaskItem(IsCompleted);
            WriteContentTo(writer);
            writer.WriteEndTaskItem();
            return writer;
        }

        internal override MElement Clone()
        {
            return new MTaskItem(this);
        }
    }
}
