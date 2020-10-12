﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1012UseExplicitTypeInsteadOfVarWhenTypeIsObviousTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseExplicitTypeInsteadOfVarWhenTypeIsObvious;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseExplicitTypeInsteadOfVarWhenTypeIsObviousAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UseExplicitTypeInsteadOfVarCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task Test()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    void M()
    {
        [|var|] x = new List<string>();
    }
}
", @"
using System.Collections.Generic;

class C
{
    void M()
    {
        List<string> x = new List<string>();
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task Test_Tuple_DeclarationExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        [|var|] (x, y) = default((object, System.DateTime));
    }
}
", @"
class C
{
    void M()
    {
        (object x, System.DateTime y) = default((object, System.DateTime));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task Test_TupleExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        (object x, [|var|] y) = default((object, System.DateTime));
    }
}
", @"
class C
{
    void M()
    {
        (object x, System.DateTime y) = default((object, System.DateTime));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task Test_TupleExpression_AllVar()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        ([|var|] x, [|var|] y) = default((object, System.DateTime));
    }
}
", @"
class C
{
    void M()
    {
        (object x, System.DateTime y) = default((object, System.DateTime));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task TestNoDiagnostic_ForEach_DeclarationExpression()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, string y)> M()
    {
        foreach (var (x, y) in M())
        {
        }

        return default;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseExplicitTypeInsteadOfVarWhenTypeIsObvious)]
        public async Task TestNoDiagnostic_ForEach_TupleExpression()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Collections.Generic;

class C
{
    IEnumerable<(object x, string y)> M()
    {
        foreach ((object x, string y) in M())
        {
        }

        return default;
    }
}
");
        }
    }
}
