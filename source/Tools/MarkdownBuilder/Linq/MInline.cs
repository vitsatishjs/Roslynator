// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MInline : MContainer
    {
        public MInline()
        {
        }

        public MInline(object content)
            : base(content)
        {
        }

        public MInline(params object[] content)
            : base(content)
        {
        }

        public MInline(MContainer other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Inline;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            WriteContentTo(writer);
            return writer;
        }

        internal override MElement Clone()
        {
            return new MInline(this);
        }

        internal override void ValidateElement(MElement element)
        {
            switch (element.Kind)
            {
                case MarkdownKind.Text:
                case MarkdownKind.Raw:
                case MarkdownKind.Link:
                case MarkdownKind.LinkReference:
                case MarkdownKind.Image:
                case MarkdownKind.ImageReference:
                case MarkdownKind.Autolink:
                case MarkdownKind.InlineCode:
                case MarkdownKind.CharEntity:
                case MarkdownKind.EntityRef:
                case MarkdownKind.Comment:
                case MarkdownKind.Bold:
                case MarkdownKind.Italic:
                case MarkdownKind.Strikethrough:
                    return;
            }

            Error.InvalidContent(this, element);
        }
    }
}