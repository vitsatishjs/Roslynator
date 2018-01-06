// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class EmphasisOptionsExtensions
    {
        public static State ToState(this EmphasisOption option)
        {
            switch (option)
            {
                case EmphasisOption.Bold:
                    return State.Bold;
                case EmphasisOption.Italic:
                    return State.Italic;
                case EmphasisOption.Strikethrough:
                    return State.Strikethrough;
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(option), nameof(option));
            }
        }
    }
}
