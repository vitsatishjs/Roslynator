// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    public abstract class MContainer : MElement
    {
        internal object content;

        internal MContainer()
        {
        }

        internal MContainer(object content)
        {
            Add(content);
        }

        internal MContainer(params object[] content)
        {
            Add(content);
        }

        internal MContainer(MContainer other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.content is string)
            {
                content = other.content;
            }
            else
            {
                var e = (MElement)other.content;
                if (e != null)
                {
                    do
                    {
                        e = e.next;
                        AppendElement(e.Clone());

                    } while (e != other.content);
                }
            }
        }

        public MElement FirstElement
        {
            get { return LastElement?.next; }
        }

        public MElement LastElement
        {
            get
            {
                if (content == null)
                    return null;

                if (content is MElement element)
                    return element;

                if (content is string s)
                {
                    if (s.Length == 0)
                        return null;

                    var text = new MText(s) { parent = this };
                    text.next = text;

                    Interlocked.CompareExchange<object>(ref content, text, s);
                }

                return (MElement)content;
            }
        }

        public IEnumerable<MElement> Elements()
        {
            MElement e = LastElement;

            if (e != null)
            {
                do
                {
                    e = e.next;
                    yield return e;

                } while (e.parent == this && e != content);
            }
        }

        public IEnumerable<MElement> Descendants()
        {
            return GetDescendants(false);
        }

        public IEnumerable<MElement> DescendantsAndSelf()
        {
            return GetDescendants(true);
        }

        internal IEnumerable<MElement> GetDescendants(bool self)
        {
            MElement e = this;

            if (self)
                yield return e;

            var c = this;

            while (true)
            {
                MElement first = c?.FirstElement;

                if (first != null)
                {
                    e = first;
                }
                else
                {
                    while (e != this
                        && e == e.parent.content)
                    {
                        e = e.parent;
                    }

                    if (e == this)
                        break;

                    e = e.next;
                }

                if (e != null)
                    yield return e;

                c = e as MContainer;
            }
        }

        public void Add(object content)
        {
            if (content == null)
                return;

            if (content is MElement element)
            {
                AddElement(element);
                return;
            }

            if (content is string s)
            {
                AddString(s);
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

            AddString(content.ToString());
        }

        public void Add(params object[] content)
        {
            Add((object)content);
        }

        internal void AddElement(MElement e)
        {
            ValidateElement(e, this);

            if (e.parent != null
                || e == TopmostParentOrSelf())
            {
                e = e.Clone();
            }

            ConvertTextToElement();
            AppendElement(e);
        }

        internal void AppendElement(MElement element)
        {
            element.parent = this;

            if (content == null
                || content is string)
            {
                element.next = element;
            }
            else
            {
                var e = (MElement)content;
                element.next = e.next;
                e.next = element;
            }

            content = element;
        }

        internal void AddString(string s)
        {
            ValidateString(s);

            if (content == null)
            {
                content = s;
            }
            else if (s.Length > 0)
            {
                if (content is string stringContent)
                {
                    content = stringContent + s;
                }
                else if (content is MText text)
                {
                    text.Value += s;
                }
                else
                {
                    AppendElement(new MText(s));
                }
            }
        }

        internal virtual void ValidateElement(MElement element, MElement previous)
        {
        }

        internal virtual void ValidateString(string s)
        {
        }

        internal void ConvertTextToElement()
        {
            var s = content as string;

            if (!string.IsNullOrEmpty(s))
            {
                var text = new MText(s) { parent = this };

                text.next = text;
                content = text;
            }
        }
    }
}