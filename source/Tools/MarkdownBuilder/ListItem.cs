// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq}")]
    public struct ListItem : IMarkdown, IEquatable<ListItem>
    {
        internal ListItem(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendListItem(Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is ListItem other)
                && Equals(other);
        }

        public bool Equals(ListItem other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Text);
        }

        public static bool operator ==(ListItem left, ListItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ListItem left, ListItem right)
        {
            return !(left == right);
        }
    }
}
