// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pihrtsoft.Markdown
{
    [Flags]
    public enum HeadingOptions
    {
        None = 0,
        EmptyLineBefore = 1,
        EmptyLineAfter = 2,
        EmptyLineBeforeAndAfter = EmptyLineBefore | EmptyLineAfter,
        UnderlineH1 = 4,
        UnderlineH2 = 8,
        Underline = UnderlineH1 | UnderlineH2,
        Close = 16
    }
}
