﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Analysis.MarkLocalVariableAsConst;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1118MarkLocalVariableAsConstTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.MarkLocalVariableAsConst;

        public override DiagnosticAnalyzer Analyzer { get; } = new LocalDeclarationStatementAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new MarkLocalVariableAsConstCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.MarkLocalVariableAsConst)]
        public async Task Test_ConstantValue()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        [|string|] s = ""a"";
        string s2 = s + ""b"";
    }
}
", @"
class C
{
    void M()
    {
        const string s = ""a"";
        string s2 = s + ""b"";
    }
}
");
        }
    }
}