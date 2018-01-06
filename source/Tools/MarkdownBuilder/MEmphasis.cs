// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} Options = {Options}")]
    public class MEmphasis : MContainer
    {
        internal MEmphasis(EmphasisOptions options, object content)
            : base(content)
        {
            Options = options;
        }

        internal MEmphasis(EmphasisOptions options, params object[] content)
            : base(content)
        {
            Options = options;
        }

        public MEmphasis(MEmphasis other)
            : base(other)
        {
            Options = other.Options;
        }

        public EmphasisOptions Options { get; set; }

        public override MarkdownKind Kind => MarkdownKind.Emphasis;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.Append(Options, Elements());
        }

        internal override MElement Clone()
        {
            return new MEmphasis(this);
        }
    }
}
