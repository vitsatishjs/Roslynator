﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Number} {NumberAsHexadecimalString}")]
    public class MCharReference : MElement
    {
        public MCharReference(int number)
        {
            Number = number;
        }

        public MCharReference(MCharReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Number = other.Number;
        }

        public int Number { get; }

        internal string NumberAsHexadecimalString => Number.ToString("x", CultureInfo.InvariantCulture);

        public override MarkdownKind Kind => MarkdownKind.CharReference;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteCharReference(Number);
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
                    return Number.ToString("x", CultureInfo.InvariantCulture);
                case CharReferenceFormat.Decimal:
                    return Number.ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(format), nameof(format));
            }
        }
    }
}