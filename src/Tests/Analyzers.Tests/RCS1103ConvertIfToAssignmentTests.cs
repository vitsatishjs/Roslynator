﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1103ConvertIfToAssignmentTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.ConvertIfToAssignment;

        public override DiagnosticAnalyzer Analyzer { get; } = new IfStatementAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new IfStatementCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertIfToAssignment)]
        public async Task Test_InvertCondition()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        bool a = false;
        int x = 0;

        //x
        [|if (x >= 1)
        {
            a = false;
        }
        else
        {
            a = true;
        }|]
    }
}
", @"
class C
{
    void M()
    {
        bool a = false;
        int x = 0;

        //x
        a = x < 1;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertIfToAssignment)]
        public async Task TestNoDiagnostic_ContainsComment()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        bool a = false, b = false;

        if (a)
        {
            b = true;
        }
        else
        {
            //x
            b = false;
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertIfToAssignment)]
        public async Task TestNoDiagnostic_ContainsDirective()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        bool a = false, b = false;

        if (a)
        {
            b = true;
        }
        else
        {
#if DEBUG
            b = false;
#endif
        }
    }
}
");
        }
    }
}
