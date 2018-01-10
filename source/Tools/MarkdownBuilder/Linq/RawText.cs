// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Value,nq}")]
    public class RawText : MElement
    {
        private string _value;

        public RawText(string value)
        {
            Value = value;
        }

        public RawText(RawText other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Value = other.Value;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public override MarkdownKind Kind => MarkdownKind.RawText;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.Append(Value, escape: false);
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.Write(Value, escape: false);
        }

        internal override MElement Clone()
        {
            return new RawText(this);
        }
    }
}
