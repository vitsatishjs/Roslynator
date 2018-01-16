// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MItalic : MInlineContainer
    {
        public MItalic()
        {
        }

        public MItalic(object content)
            : base(content)
        {
        }

        public MItalic(params object[] content)
            : base(content)
        {
        }

        public MItalic(MItalic other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Italic;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            writer.WriteItalicStart();
            WriteContentTo(writer);
            writer.WriteItalicEnd();
            return writer;
        }

        internal override MElement Clone()
        {
            return new MItalic(this);
        }
    }
}
