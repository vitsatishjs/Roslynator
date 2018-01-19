// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind}")]
    public abstract class MObject
    {
        //TODO: protected (internal) property
        internal MContainer parent;

        public abstract MarkdownKind Kind { get; }

        public MDocument Document
        {
            get { return TopmostParentOrSelf() as MDocument; }
        }

        public MContainer Parent
        {
            get { return parent; }
        }

        //TODO: zrušit
        internal MObject TopmostParentOrSelf()
        {
            var x = this;

            while (x.parent != null)
                x = x.parent;

            return x;
        }
    }
}