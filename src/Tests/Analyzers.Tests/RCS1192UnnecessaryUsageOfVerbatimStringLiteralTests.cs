﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1192UnnecessaryUsageOfVerbatimStringLiteralTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UnnecessaryUsageOfVerbatimStringLiteral;

        public override DiagnosticAnalyzer Analyzer { get; } = new UnnecessaryUsageOfVerbatimStringLiteralAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new UnnecessaryUsageOfVerbatimStringLiteralCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task Test_EmptyStringLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = [|@|]"""";
    }
}
", @"
class C
{
    void M()
    {
        string s = """";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task Test_NonEmptyStringLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = [|@|]"""";
    }
}
", @"
class C
{
    void M()
    {
        string s = """";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task Test_EmptyInterpolatedString()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = $[|@|]"""";
    }
}
", @"
class C
{
    void M()
    {
        string s = $"""";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task Test_NonEmptyInterpolatedString()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        string s = $[|@|]""x{""""}x"";
    }
}
", @"
class C
{
    void M()
    {
        string s = $""x{""""}x"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task TestNoDiagnostic_StringLiteral()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        string s = null;

        s = @"" \ "";
        s = @"" """" "";
        s = @""
"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task TestNoDiagnostic_InterpolatedString()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        string s = null;

        s = $@"" {s} \ "";
        s = $@"" {s} """" "";
        s = $@"" {s}
"";

        s = $@""s{
s}s"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnnecessaryUsageOfVerbatimStringLiteral)]
        public async Task TestNoDiagnostic_InterpolatedString_FormatClauseContainsBackslash()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    void M()
    {
        string s = $@""{DateTime.UtcNow:yyyy\/MM\/dd\/HH\/mm}"";
    }
}
");
        }
    }
}
