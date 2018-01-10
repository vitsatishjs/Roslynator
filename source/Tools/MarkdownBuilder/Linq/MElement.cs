// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {GetString(),nq}")]
    public abstract class MElement : MObject
    {
        internal MElement next;

        public abstract MarkdownBuilder AppendTo(MarkdownBuilder builder);

        public abstract MarkdownWriter WriteTo(MarkdownWriter writer);

        internal abstract MElement Clone();

        public override string ToString()
        {
            return ToString(default(MarkdownWriterSettings));
        }

        public string ToString(MarkdownFormat format)
        {
            return ToString((format != null) ? new MarkdownWriterSettings(format) : null);
        }

        public string ToString(MarkdownWriterSettings settings)
        {
            using (var sw = new StringWriter())
            {
                using (MarkdownWriter mw = MarkdownWriter.Create(sw, settings))
                {
                    WriteTo(mw);
                }

                return sw.ToString();
            }
        }

        internal string GetString()
        {
            return ToString(MarkdownWriterSettings.Debugging);
        }

        public MElement NextElement
        {
            get
            {
                return (parent != null && parent.content != this)
                    ? next
                    : null;
            }
        }

        public MElement PreviousElement
        {
            get
            {
                if (parent == null)
                    return null;

                MElement e = ((MElement)parent.content).next;

                MElement p = null;

                while (e != this)
                {
                    p = e;

                    e = e.next;
                }

                return p;
            }
        }

        public IEnumerable<MContainer> Ancestors()
        {
            return GetAncestors(false);
        }

        internal IEnumerable<MContainer> GetAncestors(bool self)
        {
            var c = ((self) ? this : parent) as MContainer;

            while (c != null)
            {
                yield return c;

                c = c.parent;
            }
        }

        public IEnumerable<MElement> ElementsAfterSelf()
        {
            var e = this;

            while (e.parent != null
                && e.parent.content != e)
            {
                e = e.next;

                yield return e;
            }
        }

        public IEnumerable<MElement> ElementsBeforeSelf()
        {
            if (parent != null)
            {
                var e = (MElement)parent.content;

                do
                {
                    e = e.next;

                    if (e == this)
                        break;

                    yield return e;

                } while (parent != null && parent == e.parent);
            }
        }

        public void Remove()
        {
            if (parent == null)
                throw new InvalidOperationException("Element has no parent.");

            parent.RemoveElement(this);
        }
    }
}