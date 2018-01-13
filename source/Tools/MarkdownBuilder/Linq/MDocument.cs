// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MDocument : MBlockContainer
    {
        public MDocument()
        {
        }

        public MDocument(object content)
            : base(content)
        {
        }

        public MDocument(params object[] content)
            : base(content)
        {
        }

        public MDocument(MDocument other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Document;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.Write(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new MDocument(this);
        }
    }
}