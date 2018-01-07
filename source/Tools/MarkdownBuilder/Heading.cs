// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {Level} {ToString(),nq}")]
    public class Heading : MContainer
    {
        public Heading(int level)
        {
            ThrowOnInvalidLevel(level);

            Level = level;
        }

        public Heading(int level, object content)
            : base(content)
        {
            ThrowOnInvalidLevel(level);

            Level = level;
        }

        public Heading(int level, params object[] content)
            : base(content)
        {
            ThrowOnInvalidLevel(level);

            Level = level;
        }

        public Heading(Heading other)
            : base(other)
        {
            Level = other.Level;
        }

        public int Level { get; set; }

        public override MarkdownKind Kind => MarkdownKind.Heading;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendHeading(Level, TextOrElements());
        }

        internal override MElement Clone()
        {
            return new Heading(this);
        }

        internal static void ThrowOnInvalidLevel(int level)
        {
            if (level < 1
                || level > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(level), level, ErrorMessages.HeadingLevelMustBeInRangeFromOneToSix);
            }
        }
    }
}
