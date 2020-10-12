﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1198AvoidBoxingOfValueTypeTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.AvoidBoxingOfValueType;

        public override DiagnosticAnalyzer Analyzer { get; } = new AvoidBoxingOfValueTypeAnalyzer();

        public override ImmutableArray<DiagnosticAnalyzer> AdditionalAnalyzers => ImmutableArray.Create<DiagnosticAnalyzer>(new InvocationExpressionAnalyzer());

        public override CodeFixProvider FixProvider { get; } = new AvoidBoxingOfValueTypeCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task Test_Interpolation()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    public TimeSpan P { get; }

    void M()
    {
        var c = new C();

        var x = $""{[|c?.P.TotalMilliseconds|]}"";
    }
}
", @"
using System;

class C
{
    public TimeSpan P { get; }

    void M()
    {
        var c = new C();

        var x = $""{(c?.P.TotalMilliseconds).ToString()}"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task Test_Interpolation_NullableType()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
        int? i = null;

        string s = $""{[|i|]}"";
    }
}
", @"
class C
{
    void M()
    {
        int? i = null;

        string s = $""{i?.ToString()}"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task Test_StringBuilder_Append()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Text;

class C
{
    void M()
    {
        StringSplitOptions o = StringSplitOptions.None;
        var sb = new StringBuilder();

        sb.Append([|o|]);
    }
}
", @"
using System;
using System.Text;

class C
{
    void M()
    {
        StringSplitOptions o = StringSplitOptions.None;
        var sb = new StringBuilder();

        sb.Append(o.ToString());
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task Test_StringBuilder_Insert()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;
using System.Text;

class C
{
    void M()
    {
        StringSplitOptions o = StringSplitOptions.None;
        var sb = new StringBuilder();

        sb.Insert(1, [|o|]);
    }
}
", @"
using System;
using System.Text;

class C
{
    void M()
    {
        StringSplitOptions o = StringSplitOptions.None;
        var sb = new StringBuilder();

        sb.Insert(1, o.ToString());
    }
}
");
        }

        // https://github.com/dotnet/roslyn/pull/35006
        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task TestNoDiagnostic_StringConcatenation()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        int i = 0;
        string s = """" + i;
    }
}
");
        }

        // https://github.com/dotnet/roslyn/pull/35006
        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task TestNoDiagnostic_InterpolatedString()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        int i = 0;
        string s = """";

        s = $""{i,1}"";
        s = $""{i:f}"";
        s = $""{i,1:f}"";
    }
}
");
        }

        // https://github.com/dotnet/roslyn/pull/35006
        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AvoidBoxingOfValueType)]
        public async Task TestNoDiagnostic_AppendFormat()
        {
            await VerifyNoDiagnosticAsync(@"
using System.Text;

class C
{
    void M()
    {
        int i = 0;
        var sb = new StringBuilder();

        sb.AppendFormat(""f"", i);
        sb.AppendFormat(""f"", i, i);
        sb.AppendFormat(""f"", i, i, i);
        sb.AppendFormat(""f"", i, i, i, i);
    }
}
");
        }
    }
}
