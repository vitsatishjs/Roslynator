﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1021ConvertLambdaExpressionBodyToExpressionBodyTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.ConvertLambdaExpressionBodyToExpressionBody;

        public override DiagnosticAnalyzer Analyzer { get; } = new LambdaExpressionAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new ConvertLambdaExpressionBodyToExpressionBodyCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertLambdaExpressionBodyToExpressionBody)]
        public async Task Test()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class C
{
    void M()
    {
        var list = new List<Func<Task>>()
        {
            new Func<Task>(() => [|{ return Task.CompletedTask; }|]),
        };
    }
}
", @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class C
{
    void M()
    {
        var list = new List<Func<Task>>()
        {
            new Func<Task>(() => Task.CompletedTask),
        };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertLambdaExpressionBodyToExpressionBody)]
        public async Task Test2()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    void M()
    {
        var actions = new Action[] {
            new Action(() => [|{ throw new InvalidOperationException(); }|])
        };
    }
}
", @"
using System;

class C
{
    void M()
    {
        var actions = new Action[] {
            new Action(() => throw new InvalidOperationException())
        };
    }
}
");
        }
    }
}
