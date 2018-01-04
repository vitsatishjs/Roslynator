// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    public class MDocument : MContainer
    {
        public MDocument()
        {
        }

        public MDocument(object content) : base(content)
        {
        }

        public MDocument(params object[] content) : base(content)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.Document;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            foreach (MElement element in Elements)
            {
                element.AppendTo(builder);
            }

            return builder;
        }

        //TODO: LinkReferences
    }
}