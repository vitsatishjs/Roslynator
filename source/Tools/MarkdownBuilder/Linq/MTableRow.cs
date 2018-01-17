﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    //TODO: DebuggerDisplay error
    public class MTableRow : MContainer
    {
        public MTableRow()
        {
        }

        public MTableRow(object content)
            : base(content)
        {
        }

        public MTableRow(params object[] content)
            : base(content)
        {
        }

        public MTableRow(MTableRow other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.TableRow;

        internal override bool AllowStringConcatenation => false;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteTableRow(this);
        }

        internal override MElement Clone()
        {
            return new MTableRow(this);
        }

        internal override void ValidateElement(MElement element)
        {
            switch (element.Kind)
            {
                case MarkdownKind.TableColumn:
                    return;
            }

            base.ValidateElement(element);
        }
    }
}
