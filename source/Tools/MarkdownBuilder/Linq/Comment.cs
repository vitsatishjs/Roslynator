// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Value,nq}")]
    public class Comment : MElement
    {
        private string _value;

        public Comment(string value)
        {
            Value = value;
        }

        public Comment(Comment other)
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

        public override MarkdownKind Kind => MarkdownKind.Comment;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendComment(Value);
        }

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteComment(Value);
        }

        internal override MElement Clone()
        {
            return new Comment(this);
        }
    }
}
