// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MInlineContainer : MContainer
    {
        internal MInlineContainer()
        {
        }

        internal MInlineContainer(object content)
            : base(content)
        {
        }

        internal MInlineContainer(params object[] content)
            : base(content)
        {
        }

        internal MInlineContainer(MContainer other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.InlineContainer;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.Write(TextOrElements());
        }

        internal override MElement Clone()
        {
            return new MInlineContainer(this);
        }

        internal override void ValidateElement(MElement element)
        {
            switch (element.Kind)
            {
                case MarkdownKind.Text:
                case MarkdownKind.RawText:
                case MarkdownKind.Link:
                case MarkdownKind.LinkReference:
                case MarkdownKind.Image:
                case MarkdownKind.ImageReference:
                case MarkdownKind.Autolink:
                case MarkdownKind.InlineCode:
                case MarkdownKind.CharReference:
                case MarkdownKind.EntityReference:
                case MarkdownKind.Comment:
                case MarkdownKind.Bold:
                case MarkdownKind.Italic:
                case MarkdownKind.Strikethrough:
                    return;
            }

            Error.ThrowInvalidContent(this, element);
        }
    }
}