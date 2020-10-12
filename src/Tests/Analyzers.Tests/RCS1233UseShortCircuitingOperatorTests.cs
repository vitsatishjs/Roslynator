﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1233UseShortCircuitingOperatorTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseShortCircuitingOperator;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseShortCircuitingOperatorAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new BinaryExpressionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseShortCircuitingOperator)]
        public async Task Test_Or()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        bool f = false;
        bool f2 = false;

        if (f [|||] f2)
        {
        }
    }
}
", @"
class C
{
    void M()
    {
        bool f = false;
        bool f2 = false;

        if (f || f2)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseShortCircuitingOperator)]
        public async Task Test_And()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        bool f = false;
        bool f2 = false;

        if (f [|&|] f2)
        {
        }
    }
}
", @"
class C
{
    void M()
    {
        bool f = false;
        bool f2 = false;

        if (f && f2)
        {
        }
    }
}
");
        }
    }
}
