// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    internal static class EmphasisOptionsExtensions
    {
        public static State ToState(this EmphasisOptions options)
        {
            switch (options)
            {
                case EmphasisOptions.Bold:
                    return State.Bold;
                case EmphasisOptions.Italic:
                    return State.Italic;
                case EmphasisOptions.Strikethrough:
                    return State.Strikethrough;
                default:
                    throw new ArgumentException(ErrorMessages.UnknownEnumValue(options), nameof(options));
            }
        }
    }
}
