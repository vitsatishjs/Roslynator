﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1145RemoveRedundantAsOperatorTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.RemoveRedundantAsOperator;

        public override DiagnosticAnalyzer Analyzer { get; } = new RemoveRedundantAsOperatorAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new BinaryExpressionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.RemoveRedundantAsOperator)]
        public async Task TestNoDiagnostic_Dynamic()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        dynamic d = null;

        object o = null;

        d = o as dynamic;

        o = d as object;
    }
}
");
        }
    }
}
