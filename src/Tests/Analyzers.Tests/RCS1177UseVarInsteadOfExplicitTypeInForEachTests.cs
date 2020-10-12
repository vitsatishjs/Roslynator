﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1177UseVarInsteadOfExplicitTypeInForEachTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseVarInsteadOfExplicitTypeInForEach;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseVarInsteadOfExplicitTypeInForEachAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UseVarInsteadOfExplicitTypeCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseVarInsteadOfExplicitTypeInForEach)]
        public async Task Test()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        foreach ([|string|] item in items)
        {
        }
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        var items = new List<string>();

        foreach (var item in items)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseVarInsteadOfExplicitTypeInForEach)]
        public async Task Test_TupleExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, object y)> M()
    {
        foreach ([|(object x, object y)|] in M())
        {
        }

        return default;
    }
}
", @"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, object y)> M()
    {
        foreach (var (x, y) in M())
        {
        }

        return default;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseVarInsteadOfExplicitTypeInForEach)]
        public async Task Test_TupleExpression_Var()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, object y)> M()
    {
        foreach ([|(object x, var y)|] in M())
        {
        }

        return default;
    }
}
", @"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, object y)> M()
    {
        foreach (var (x, y) in M())
        {
        }

        return default;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseVarInsteadOfExplicitTypeInForEach)]
        public async Task TestNoDiagnostic_TupleExpression()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    (object x, object y) M()
    {
        (object x, object y) = M();

        return default;
    }
}
");
        }
    }
}
