// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;

namespace Pihrtsoft.Markdown
{
    //TODO: CharReference
    [DebuggerDisplay("{Kind} {Number} {NumberAsHexadecimalString}")]
    public class CharacterReference : MElement
    {
        public CharacterReference(int number)
        {
            Number = number;
        }

        public CharacterReference(CharacterReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Number = other.Number;
        }

        public int Number { get; }

        internal string NumberAsHexadecimalString => Number.ToString("x", CultureInfo.InvariantCulture);

        public override MarkdownKind Kind => MarkdownKind.CharacterReference;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendCharacterReference(Number);
        }

        internal override MElement Clone()
        {
            return new CharacterReference(this);
        }

        internal string NumberAsString(CharacterReferenceFormat format)
        {
            switch (format)
            {
                case CharacterReferenceFormat.Hexadecimal:
                    return Number.ToString("x", CultureInfo.InvariantCulture);
                case CharacterReferenceFormat.Decimal:
                    return Number.ToString(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(format), nameof(format));
            }
        }
    }
}
