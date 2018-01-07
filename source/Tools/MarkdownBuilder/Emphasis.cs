// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Option} {ToString(),nq}")]
    public class Emphasis : MContainer
    {
        public Emphasis(EmphasisOption option)
        {
            Option = option;
        }

        public Emphasis(EmphasisOption option, object content)
            : base(content)
        {
            Option = option;
        }

        public Emphasis(EmphasisOption option, params object[] content)
            : base(content)
        {
            Option = option;
        }

        public Emphasis(Emphasis other)
            : base(other)
        {
            Option = other.Option;
        }

        public EmphasisOption Option { get; set; }

        public override MarkdownKind Kind => MarkdownKind.Emphasis;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.Append(Option, TextOrElements());
        }

        internal override MElement Clone()
        {
            return new Emphasis(this);
        }
    }
}
