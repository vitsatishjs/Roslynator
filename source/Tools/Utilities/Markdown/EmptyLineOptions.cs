// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.Utilities.Markdown
{
    [Flags]
    public enum EmptyLineOptions
    {
        None = 0,
        AddEmptyLineBefore = 1,
        AddEmptyLineAfter = 2,
        AddEmptyLineBeforeAndAfter = AddEmptyLineBefore | AddEmptyLineAfter
    }
}
