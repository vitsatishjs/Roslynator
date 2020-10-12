﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.Formatting.CodeFixes.CSharp;
using Xunit;

namespace Roslynator.Formatting.CSharp.Tests
{
    public class RCS0015AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersaTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa;

        public override DiagnosticAnalyzer Analyzer { get; } = new AddOrRemoveEmptyLineBetweenUsingDirectiveAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new SyntaxTriviaCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task Test_AddEmptyLine_EmptyLine()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;[||]
using Microsoft.CodeAnalysis;[||]
using System.Threading;

class C
{
}
", @"
using System;

using Microsoft.CodeAnalysis;

using System.Threading;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task Test_RemoveEmptyLine()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
[||]
using Microsoft.CodeAnalysis;
[||]
using System.Threading;

class C
{
}
", @"
using System;
using Microsoft.CodeAnalysis;
using System.Threading;

class C
{
}
", options: Options.WithEnabled(AnalyzerOptions.RemoveEmptyLineBetweenUsingDirectivesWithDifferentRootNamespace));
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task Test_RemoveEmptyLines()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
[||]    

using Microsoft.CodeAnalysis;

class C
{
}
", @"
using System;
using Microsoft.CodeAnalysis;

class C
{
}
", options: Options.WithEnabled(AnalyzerOptions.RemoveEmptyLineBetweenUsingDirectivesWithDifferentRootNamespace));
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_AddEmptyLine_SameRootNamespace()
        {
            await VerifyNoDiagnosticAsync(@"
using System;
using System.Linq;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_AddEmptyLine_UsingStatic()
        {
            await VerifyNoDiagnosticAsync(@"
using Microsoft.CodeAnalysis;
using static System.IO.Path;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_AddEmptyLine_Alias()
        {
            await VerifyNoDiagnosticAsync(@"
using Microsoft.CodeAnalysis;
using I = System.Int32;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_RemoveEmptyLine_SameRootNamespace()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

using System.Linq;

class C
{
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_RemoveEmptyLine_UsingStatic()
        {
            await VerifyNoDiagnosticAsync(@"
using Microsoft.CodeAnalysis;

using static System.IO.Path;

class C
{
}
", options: Options.WithEnabled(AnalyzerOptions.RemoveEmptyLineBetweenUsingDirectivesWithDifferentRootNamespace));
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenUsingDirectivesWithDifferentRootNamespaceOrViceVersa)]
        public async Task TestNoDiagnostic_RemoveEmptyLine_Alias()
        {
            await VerifyNoDiagnosticAsync(@"
using Microsoft.CodeAnalysis;

using I = System.Int32;

class C
{
}
", options: Options.WithEnabled(AnalyzerOptions.RemoveEmptyLineBetweenUsingDirectivesWithDifferentRootNamespace));
        }
    }
}
