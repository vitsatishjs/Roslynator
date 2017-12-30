// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Text,nq} Level = {Level}")]
    public struct Heading : IMarkdown, IEquatable<Heading>
    {
        internal Heading(string text, int level = 1)
        {
            if (level < 1
                || level > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, "Heading level must be between 1-6.");
            }

            Text = text;
            Level = level;
        }

        public string Text { get; }

        public int Level { get; }

        public Heading WithText(string text)
        {
            return new Heading(text, Level);
        }

        public Heading WithLevel(int level)
        {
            return new Heading(Text, level);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendHeading(Level, Text);
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
