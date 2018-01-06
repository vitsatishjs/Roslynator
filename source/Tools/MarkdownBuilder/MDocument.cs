// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.IO;

namespace Pihrtsoft.Markdown
{
    public class MDocument : MContainer
    {
        public MDocument()
        {
        }

        public MDocument(object content)
            : base(content)
        {
        }

        public MDocument(params object[] content) : base(content)
        {
        }

        public MDocument(MDocument other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Document;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendRange(Elements());
        }

        internal override MElement Clone()
        {
            return new MDocument(this);
        }
    }
}