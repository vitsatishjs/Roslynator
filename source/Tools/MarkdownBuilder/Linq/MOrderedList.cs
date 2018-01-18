// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MOrderedList : MList
    {
        public MOrderedList()
        {
        }

        public MOrderedList(object content)
            : base(content)
        {
        }

        public MOrderedList(params object[] content)
            : base(content)
        {
        }

        public MOrderedList(MOrderedList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.OrderedList;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            if (content is string s)
            {
                return writer.WriteOrderedItem(1, s);
            }
            else
            {
                int number = 1;
                foreach (MElement element in Elements())
                {
                    writer.WriteStartOrderedItem(number);

                    if (element is MOrderedItem item)
                    {
                        item.WriteContentTo(writer);
                    }
                    else
                    {
                        writer.Write(element);
                    }

                    writer.WriteEndOrderedItem();
                    number++;
                }
            }

            writer.WriteLine();
            return writer;
        }

        internal override void ValidateElement(MElement element)
        {
            if (element.Kind == MarkdownKind.OrderedItem)
                return;

            base.ValidateElement(element);
        }

        internal override MElement Clone()
        {
            return new MOrderedList(this);
        }
    }
}
