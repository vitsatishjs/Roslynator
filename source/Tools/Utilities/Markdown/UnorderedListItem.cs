// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using static Roslynator.Utilities.Markdown.MarkdownFactory;

namespace Roslynator.Utilities.Markdown
{
    //TODO: TaskListItem

    //TODO: UnorderedListItem
    public struct UnorderedListItem : IAppendable
    {
        internal UnorderedListItem(string text, byte level = 1)
        {
            OriginalText = text;
            Level = level;
        }

        public string OriginalText { get; }

        public byte Level { get; }

        public StringBuilder Append(StringBuilder sb, MarkdownSettings settings = null)
        {
            return sb
                .AppendMany(settings.Indentation, Level)
                .Append(MarkdownSettings.UnorderedListItemText)
                .Append(" ")
                .AppendLineIf(!string.IsNullOrEmpty(OriginalText), OriginalText, escape: true);
        }

        //TODO: 
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
