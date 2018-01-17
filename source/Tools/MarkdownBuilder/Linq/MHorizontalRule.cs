// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown.Linq
{
    public class MHorizontalRule : MElement
    {
        private int _count;

        internal MHorizontalRule(char value, int count = 3, string separator = " ")
        {
            Value = value;
            Count = count;
            Separator = separator ?? "";
        }

        public MHorizontalRule(HorizontalRuleFormat format)
            : this(format.Value, format.Count, format.Separator)
        {
        }

        public MHorizontalRule(MHorizontalRule other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Value = other.Value;
            Count = other.Count;
            Separator = other.Separator;
        }

        public char Value { get; set; }

        public int Count
        {
            get { return _count; }
            set
            {
                Error.ThrowOnInvalidHorizontalRuleCount(value);
                _count = value;
            }
        }

        public string Separator { get; set; }

        public override MarkdownKind Kind => MarkdownKind.HorizontalRule;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteHorizontalRule(Value, Count, Separator);
        }

        internal override MElement Clone()
        {
            return new MHorizontalRule(this);
        }
    }
}
