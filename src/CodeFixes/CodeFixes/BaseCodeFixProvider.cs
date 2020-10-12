﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CodeFixes
{
    public abstract class BaseCodeFixProvider : AbstractCodeFixProvider
    {
        protected virtual CodeFixSettings Settings
        {
            get { return CodeFixSettings.Current; }
        }
    }
}
