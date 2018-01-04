// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Pihrtsoft.Markdown
{
    //TODO: HtmlEntity
    [DebuggerDisplay("{Number}")]
    public struct HtmlEntity : IMarkdown, IEquatable<HtmlEntity>
    {
        internal HtmlEntity(int number)
        {
            Number = number;
        }

        public int Number { get; }

        public HtmlEntity WithNumber(int number)
        {
            return new HtmlEntity(number);
        }

        public MarkdownBuilder AppendTo(MarkdownBuilder mb)
        {
            return mb.AppendHtmlEntity(Number);
        }

        public override bool Equals(object obj)
        {
            return (obj is HtmlEntity other)
                && Equals(other);
        }

        public bool Equals(HtmlEntity other)
        {
            return Number == other.Number;
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        internal string NumberAsString(HtmlEntityFormat format)
        {
            switch (format)
            {
                case HtmlEntityFormat.Hexadecimal:
                    return Number.ToString("x", CultureInfo .InvariantCulture);
                case HtmlEntityFormat.Decimal:
                    return Number.ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(format), nameof(format));
            }
        }

        public static bool operator ==(HtmlEntity htmlEntity1, HtmlEntity htmlEntity2)
        {
            return htmlEntity1.Equals(htmlEntity2);
        }

        public static bool operator !=(HtmlEntity htmlEntity1, HtmlEntity htmlEntity2)
        {
            return !(htmlEntity1 == htmlEntity2);
        }
    }
}
