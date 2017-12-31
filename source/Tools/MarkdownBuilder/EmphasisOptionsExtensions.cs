// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    internal static class EmphasisOptionsExtensions
    {
        public static State ToState(this EmphasisOptions options)
        {
            if (options == EmphasisOptions.None)
                return State.None;

            var state = State.None;

            if ((options & EmphasisOptions.Bold) != 0)
                state |= State.Bold;

            if ((options & EmphasisOptions.Italic) != 0)
                state |= State.Italic;

            if ((options & EmphasisOptions.Strikethrough) != 0)
                state |= State.Strikethrough;

            if ((options & EmphasisOptions.Code) != 0)
                state |= State.Code;

            return state;
        }
    }
}
