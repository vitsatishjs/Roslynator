﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1098ConstantValuesShouldBePlacedOnRightSideOfComparisonsTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.ConstantValuesShouldBePlacedOnRightSideOfComparisons;

        public override DiagnosticAnalyzer Analyzer { get; } = new ConstantValuesShouldBePlacedOnRightSideOfComparisonsAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new BinaryExpressionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_NullLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        if ([|null|] == s)
        {
        }
    }
}
", @"
class C
{
    void M(string s)
    {
        if (s == null)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_DefaultLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        if ([|default|] == s)
        {
        }
    }
}
", @"
class C
{
    void M(string s)
    {
        if (s == default)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_DefaultExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        if ([|default(string)|] == s)
        {
        }
    }
}
", @"
class C
{
    void M(string s)
    {
        if (s == default(string))
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_StringLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        if ([|""a""|] == s)
        {
        }
    }
}
", @"
class C
{
    void M(string s)
    {
        if (s == ""a"")
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_CharacterLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(char ch)
    {
        if ([|'a'|] == ch)
        {
        }
    }
}
", @"
class C
{
    void M(char ch)
    {
        if (ch == 'a')
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_TrueLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(bool x)
    {
        if ([|true|] == x)
        {
        }
    }
}
", @"
class C
{
    void M(bool x)
    {
        if (x == true)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_FalseLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(bool x)
    {
        if ([|false|] == x)
        {
        }
    }
}
", @"
class C
{
    void M(bool x)
    {
        if (x == false)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestDiagnostic_NumericLiteral()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(int i)
    {
        if ([|0|] == i)
        {
        }
    }
}
", @"
class C
{
    void M(int i)
    {
        if (i == 0)
        {
        }
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConstantValuesShouldBePlacedOnRightSideOfComparisons)]
        public async Task TestNoDiagnostic()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        if (null
        #region
            == s)
        {
        }
        #endregion
    }
}
");
        }
    }
}
