﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1187UseConstantInsteadOfFieldTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseConstantInsteadOfField;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseConstantInsteadOfFieldAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new MemberDeclarationCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseConstantInsteadOfField)]
        public async Task TestNoDiagnostic_AssignmentInInStaticConstructor()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    private static readonly int _f = 1;

    static C()
    {
        _f = 1;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseConstantInsteadOfField)]
        public async Task TestNoDiagnostic_RefInStaticConstructor()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    private static readonly int _f = 1;

    static C()
    {
        M(ref _f);
    }

    static void M(ref int value)
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseConstantInsteadOfField)]
        public async Task TestNoDiagnostic_OutInStaticConstructor()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    private static readonly int _f = 1;

    static C()
    {
        M(out _f);
    }

    static void M(out int value)
    {
        value = 0;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseConstantInsteadOfField)]
        public async Task TestNoDiagnostic_InInStaticConstructor()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    private static readonly int _f = 1;

    static C()
    {
        M(in _f);
    }

    static void M(in int value)
    {
    }
}
");
        }
    }
}
