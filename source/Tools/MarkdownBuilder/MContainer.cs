// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.ObjectModel;

namespace Pihrtsoft.Markdown
{
    public abstract class MContainer : MElement
    {
        protected MContainer(object content)
        {
            Add(content);
        }

        protected MContainer(params object[] content)
        {
            Add(content);
        }

        public Collection<MElement> Elements { get; } = new Collection<MElement>();

        public void Add(object content)
        {
            if (content == null)
                return;

            if (content is string s)
            {
                Elements.Add(MarkdownFactory.Text(s));
                return;
            }

            if (content is MElement element)
            {
                Elements.Add(element);
                return;
            }

            if (content is object[] arr)
            {
                foreach (object item in arr)
                    Add(item);

                return;
            }

            if (content is IEnumerable enumerable)
            {
                foreach (object item in enumerable)
                    Add(item);

                return;
            }

            Elements.Add(MarkdownFactory.Text(content.ToString()));
        }

        public void Add(params object[] content)
        {
            Add((object)content);
        }
    }
}