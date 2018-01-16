// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public class MBulletList : MList
    {
        public MBulletList()
        {
        }

        public MBulletList(object content)
            : base(content)
        {
        }

        public MBulletList(params object[] content)
            : base(content)
        {
        }

        public MBulletList(MBulletList other)
            : base(other)
        {
        }

        public override MarkdownKind Kind => MarkdownKind.BulletList;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            if (content is string s)
            {
                writer.WriteBulletItem(s);
            }
            else
            {
                foreach (MElement element in Elements())
                {
                    writer.WriteBulletItemStart();

                    if (element is MBulletItem item)
                    {
                        item.WriteContentTo(writer);
                    }
                    else
                    {
                        writer.Write(element);
                    }

                    writer.WriteBulletItemEnd();
                }
            }

            writer.WriteLine();

            return writer;
        }

        internal override void ValidateElement(MElement element)
        {
            if (element.Kind == MarkdownKind.BulletItem)
                return;

            base.ValidateElement(element);
        }

        internal override MElement Clone()
        {
            return new MBulletList(this);
        }
    }
}
