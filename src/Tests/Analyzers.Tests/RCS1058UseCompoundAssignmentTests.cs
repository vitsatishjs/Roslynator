﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Roslynator.CSharp.Testing;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1058UseCompoundAssignmentTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UseCompoundAssignment;

        public override DiagnosticAnalyzer Analyzer { get; } = new UseCompoundAssignmentAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UseCompoundAssignmentCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task Test_Property()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        [|P = P + 1|];
    }

    int P { get; set; }
}
", @"
class C
{
    void M()
    {
        P += 1;
    }

    int P { get; set; }
}
");
        }

        [Theory, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        [InlineData("i = i + 1", "i += 1")]
        [InlineData("i = (i + 1)", "i += 1")]
        [InlineData("i = i - 1", "i -= 1")]
        [InlineData("i = i * 1", "i *= 1")]
        [InlineData("i = i / 1", "i /= 1")]
        [InlineData("i = i % 1", "i %= 1")]
        [InlineData("i = i << 1", "i <<= 1")]
        [InlineData("i = i >> 1", "i >>= 1")]
        [InlineData("i = i | 1", "i |= 1")]
        [InlineData("i = i & 1", "i &= 1")]
        [InlineData("i = i ^ 1", "i ^= 1")]
        public async Task Test(string source, string expected)
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(int i)
    {
        [||];
    }
}
", source, expected);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task Test_CoalesceExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        [|s = s ?? """"|];
    }
}
", @"
class C
{
    void M(string s)
    {
        s ??= """";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task Test_LazyInitialization()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    {
        string x = null;

        return [|x ?? (x = M())|]; // x
    }
}
", @"
class C
{
    string M()
    {
        string x = null;

        return x ??= M(); // x
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task TestNoDiagnostic_ObjectInitializer()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        var x = new C() { P = P + 1 };
    }

    int P { get; set; }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task TestNoDiagnostic_CoalesceExpression_CSharp6()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        s = s ?? """";
    }
}
", options: CSharpCodeVerificationOptions.Default_CSharp6);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task TestNoDiagnostic_LazyInitialization_ExpressionsAreNotEquivalent()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M()
    {
        string x = null;
        string x2 = null;

        return x ?? (x2 = M());
    }
}
", options: CSharpCodeVerificationOptions.Default_CSharp7_3);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UseCompoundAssignment)]
        public async Task TestNoDiagnostic_LazyInitialization_CSharp7_3()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M()
    {
        string x = null;

        return x ?? (x = M());
    }
}
", options: CSharpCodeVerificationOptions.Default_CSharp7_3);
        }
    }
}
