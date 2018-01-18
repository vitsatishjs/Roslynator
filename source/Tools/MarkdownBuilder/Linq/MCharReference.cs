// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Value} {ValueAsHexadecimalString}")]
    public class MCharReference : MElement
    {
        public MCharReference(char value)
        {
            Value = value;
        }

        public MCharReference(MCharReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Value = other.Value;
        }

        public char Value { get; }

        internal string ValueAsHexadecimalString => NumberAsString(CharReferenceFormat.Hexadecimal);

        public override MarkdownKind Kind => MarkdownKind.CharReference;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteCharReference(Value);
        }

        internal override MElement Clone()
        {
            return new MCharReference(this);
        }

        internal string NumberAsString(CharReferenceFormat format)
        {
            switch (format)
            {
                case CharReferenceFormat.Hexadecimal:
                    return ((int)Value).ToString("x", CultureInfo.InvariantCulture);
                case CharReferenceFormat.Decimal:
                    return ((int)Value).ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(format), nameof(format));
            }
        }
    }
}
