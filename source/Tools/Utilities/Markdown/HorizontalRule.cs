// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Roslynator.Utilities.Markdown
{
    public struct HorizontalRule : IAppendable
    {
        public const HorizontalRuleCharacter DefaultCharacter = HorizontalRuleCharacter.Underscore;

        //TODO: 3
        public const int DefaultLength = 5;

        public static HorizontalRule Default { get; } = new HorizontalRule(DefaultCharacter, DefaultLength);

        internal HorizontalRule(HorizontalRuleCharacter character, int length = DefaultLength)
        {
            if (length < 3)
                throw new ArgumentOutOfRangeException(nameof(length), length, "");

            Character = character;
            Length = length;
        }

        public HorizontalRuleCharacter Character { get; }

        public int Length { get; }

        internal bool IsDefault
        {
            get
            {
                return Character == DefaultCharacter
                    && Length == DefaultLength;
            }
        }

        public StringBuilder Append(StringBuilder sb, MarkdownSettings settings = null)
        {
            return sb.Append(GetChar(), Length);
        }

        public override string ToString()
        {
            if (IsDefault)
                return "_____"; //TODO: 

            return new string(GetChar(), Length);
        }

        //TODO: přejmenovat
        private char GetChar()
        {
            switch (Character)
            {
                case HorizontalRuleCharacter.Asterisk:
                    return '*';
                case HorizontalRuleCharacter.Hyphen:
                    return '-';
                case HorizontalRuleCharacter.Underscore:
                    return '_';
                default:
                    throw new InvalidOperationException(); //TODO: 
            }
        }
    }
}
