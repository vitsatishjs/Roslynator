// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {Number}")]
    public class HtmlEntity : MElement
    {
        internal HtmlEntity(int number)
        {
            Number = number;
        }

        public HtmlEntity(HtmlEntity other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Number = other.Number;
        }

        public int Number { get; }

        public override MarkdownKind Kind => MarkdownKind.HtmlEntity;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendHtmlEntity(Number);
        }

        internal override MElement Clone()
        {
            return new HtmlEntity(this);
        }

        internal string NumberAsString(HtmlEntityFormat format)
        {
            switch (format)
            {
                case HtmlEntityFormat.Hexadecimal:
                    return Number.ToString("x", CultureInfo.InvariantCulture);
                case HtmlEntityFormat.Decimal:
                    return Number.ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(format), nameof(format));
            }
        }
    }
}
