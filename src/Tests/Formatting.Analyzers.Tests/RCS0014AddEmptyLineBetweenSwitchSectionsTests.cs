﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.Formatting.CodeFixes.CSharp;
using Xunit;

namespace Roslynator.Formatting.CSharp.Tests
{
    public class RCS0014AddEmptyLineBetweenSwitchSectionsTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.AddEmptyLineBetweenSwitchSections;

        public override DiagnosticAnalyzer Analyzer { get; } = new AddEmptyLineBetweenSwitchSectionsAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new SwitchSectionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenSwitchSections)]
        public async Task Test()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    {
        string s = null;

        switch (s)
        {
            case ""a"":
                return A();[||]
            case ""b"":
                return B();[||]
            default:
                return null;
        }
    }

    public string A() => null;
    public string B() => null;
}
", @"
class C
{
    string M()
    {
        string s = null;

        switch (s)
        {
            case ""a"":
                return A();

            case ""b"":
                return B();

            default:
                return null;
        }
    }

    public string A() => null;
    public string B() => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenSwitchSections)]
        public async Task Test_Comment()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    {
        string s = null;

        switch (s)
        {
            case ""a"":
                return A(); //x[||]
            default:
                return null;
        }
    }

    public string A() => null;
}
", @"
class C
{
    string M()
    {
        string s = null;

        switch (s)
        {
            case ""a"":
                return A(); //x

            default:
                return null;
        }
    }

    public string A() => null;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineBetweenSwitchSections)]
        public async Task TestNoDiagnostic_SingleSection()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M()
    {
        string s = null;

        switch (s)
        {
            case ""a"":
                return A();
        }

        return null;
    }

    public string A() => null;
}
");
        }
    }
}
