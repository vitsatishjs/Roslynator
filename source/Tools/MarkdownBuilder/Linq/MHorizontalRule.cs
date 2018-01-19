// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown.Linq
{
    public class MHorizontalRule : MElement
    {
        private string _text;
        private int _count;
        private string _separator;

        public MHorizontalRule(string text, int count, string separator)
        {
            Text = text;
            Count = count;
            Separator = separator;
        }

        public MHorizontalRule(HorizontalRuleFormat format)
            : this(format.Text, format.Count, format.Separator)
        {
        }

        public MHorizontalRule(MHorizontalRule other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            _text = other.Text;
            _count = other.Count;
            _separator = other.Separator;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                Error.ThrowOnInvalidHorizontalRuleText(value);
                _text = value;
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                Error.ThrowOnInvalidHorizontalRuleCount(value);
                _count = value;
            }
        }

        public string Separator
        {
            get { return _separator; }
            set
            {
                Error.ThrowOnInvalidHorizontalRuleSeparator(value);
                _separator = value;
            }
        }

        public override MarkdownKind Kind => MarkdownKind.HorizontalRule;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteHorizontalRule(Text, Count, Separator);
        }

        internal override MElement Clone()
        {
            return new MHorizontalRule(this);
        }
    }
}
