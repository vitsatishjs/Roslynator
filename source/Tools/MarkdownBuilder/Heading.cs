// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Level = {Level}")]
    public class Heading : MElement, IEquatable<Heading>, IMarkdown
    {
        internal Heading(string text, int level = 1)
        {
            if (level < 1
                || level > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, ErrorMessages.HeadingLevelMustBeInRangeFromOneToSix);
            }

            Text = text;
            Level = level;
        }

        public string Text { get; }

        public int Level { get; }

        public override MarkdownKind Kind => MarkdownKind.Heading;

        public Heading WithText(string text)
        {
            return new Heading(text, Level);
        }

        public Heading WithLevel(int level)
        {
            return new Heading(Text, level);
        }

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendHeading(Level, Text);
        }

        public override bool Equals(object obj)
        {
            return (obj is Heading other)
                && Equals(other);
        }

        public bool Equals(Heading other)
        {
            return string.Equals(Text, other.Text, StringComparison.Ordinal)
                && Level == other.Level;
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Level, Hash.Create(Text));
        }

        public static bool operator ==(Heading left, Heading right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Heading left, Heading right)
        {
            return !(left == right);
        }
    }
}
