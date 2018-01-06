// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {ToString(),nq}")]
    public abstract class MObject
    {
        internal MContainer parent;

        public abstract MarkdownKind Kind { get; }

        public MDocument Document
        {
            get { return TopmostParentOrSelf() as MDocument; }
        }

        internal MObject TopmostParentOrSelf()
        {
            var x = this;

            while (x.parent != null)
                x = x.parent;

            return x;
        }

        public MContainer Parent
        {
            get { return parent; }
        }
    }
}