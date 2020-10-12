﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.Formatting.CodeFixes.CSharp;
using Xunit;

namespace Roslynator.Formatting.CSharp.Tests
{
    public class RCS0038RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespaceTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace;

        public override DiagnosticAnalyzer Analyzer { get; } = new AddOrRemoveEmptyLineBetweenUsingDirectiveAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new SyntaxTriviaCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace)]
        public async Task Test_EmptyLine()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
[||]
using System.Linq;
[||]
using System.Threading;

class C
{
}
", @"
using System;
using System.Linq;
using System.Threading;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace)]
        public async Task Test_EmptyLines()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
[||]    

using System.Linq;

class C
{
}
", @"
using System;
using System.Linq;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace)]
        public async Task TestNoDiagnostic_DifferentRootNamespace()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

using Microsoft.CodeAnalysis;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace)]
        public async Task TestNoDiagnostic_UsingStatic()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

using static System.IO.Path;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveEmptyLineBetweenUsingDirectivesWithSameRootNamespace)]
        public async Task TestNoDiagnostic_Alias()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

using I = System.Int32;

class C
{
}
");
        }
    }
}
