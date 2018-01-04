// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind}")]
    public class ListItem : MContainer, IMarkdown
    {
        internal ListItem(object content)
            : base(content)
        {
        }

        public ListItem(params object[] content)
            : base(content)
        {
        }

        public override MarkdownKind Kind =>  MarkdownKind.ListItem;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendListItem(Elements);
        }

        //TODO: 
        //public ListItem WithText(string text)
        //{
        //    return new ListItem(text);
        //}

        //public override bool Equals(object obj)
        //{
        //    return (obj is ListItem other)
        //        && Equals(other);
        //}

        //public bool Equals(ListItem other)
        //{
        //    return string.Equals(Text, other.Text, StringComparison.Ordinal);
        //}

        //public override int GetHashCode()
        //{
        //    return EqualityComparer<string>.Default.GetHashCode(Text);
        //}

        //public static bool operator ==(ListItem left, ListItem right)
        //{
        //    return left.Equals(right);
        //}

        //public static bool operator !=(ListItem left, ListItem right)
        //{
        //    return !(left == right);
        //}
    }
}
