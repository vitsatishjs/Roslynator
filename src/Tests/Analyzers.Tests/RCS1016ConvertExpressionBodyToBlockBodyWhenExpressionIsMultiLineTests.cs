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
    public class RCS1016ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLineTests : AbstractCSharpFixVerifier
    {
        private CSharpCodeVerificationOptions _options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine;
        private CSharpCodeVerificationOptions _options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine;

        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.ConvertBlockBodyToExpressionBodyOrViceVersa;

        public override DiagnosticAnalyzer Analyzer { get; } = new ConvertBlockBodyToExpressionBodyOrViceVersaAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new ConvertBlockBodyToExpressionBodyOrViceVersaCodeFixProvider();

        protected override CSharpCodeVerificationOptions UpdateOptions(CSharpCodeVerificationOptions options)
        {
            return base.UpdateOptions(options).WithEnabled(AnalyzerOptions.ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        private CSharpCodeVerificationOptions Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine
            => _options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine ??= Options.WithEnabled(AnalyzerOptions.ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);

        private CSharpCodeVerificationOptions Options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine
            => _options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine ??= Options.WithEnabled(AnalyzerOptions.ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine);

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_Method_MultilineExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(object x, object y) [|=> M(
        x,
        y)|];
}
", @"
class C
{
    string M(object x, object y)
    {
        return M(
            x,
            y);
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_Method_MultilineExpression2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(object x, object y) [|=>
        M(
            x,
            y)|];
}
", @"
class C
{
    string M(object x, object y)
    {
        return M(
            x,
            y);
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_Method_MultilineExpression_NoIndentation()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(object x, object y) [|=>
M(
x,
y)|];
}
", @"
class C
{
    string M(object x, object y)
    {
        return M(
            x,
            y);
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_Method_MultilineExpression_IndentationsDiffer()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(object x, object y, object z) [|=>
        M(
            x,
                y,
                    z)|];
}
", @"
class C
{
    string M(object x, object y, object z)
    {
        return M(
            x,
                y,
                    z);
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_LocalFunction_MultilineExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(object x, object y)
    {
        return null;

        string LF() [|=> M(
            x,
            y)|];
    }
}
", @"
class C
{
    string M(object x, object y)
    {
        return null;

        string LF()
        {
            return M(
                x,
                y);
        }
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_PropertyWithGetter_MultilineExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string P [|=> M(
        null,
        null)|];

    string M(string x, string y) => null;
}
", @"
class C
{
    string P
    {
        get
        {
            return M(
                null,
                null);
        }
    }

    string M(string x, string y) => null;
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_PropertyWithGetter_MultilineDeclaration()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System;

class C
{
    string
        P [|=> M(null, null)|];

    string M(string x, string y) => null;
}
", @"
using System;

class C
{
    string
        P
    {
        get { return M(null, null); }
    }

    string M(string x, string y) => null;
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_PropertyWithGetterAndSetter_MultilineExpression()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string _f;

    public string P
    {
        get [|=> M(
            null,
            null)|];

        set => _f = value;
    }

    string M(string x, string y) => null;
}
", @"
class C
{
    string _f;

    public string P
    {
        get
        {
            return M(
                null,
                null);
        }

        set => _f = value;
    }

    string M(string x, string y) => null;
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task Test_Method_MultilineDeclaration()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M(
        object x,
        object y) [|=> M(x, y)|];
}
", @"
class C
{
    string M(
        object x,
        object y)
    {
        return M(x, y);
    }
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task TestNoDiagnostic_PreprocessorDirective()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string M() =>
#if DEBUG
        null;
#else
        null;
#endif
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenExpressionIsMultiLine);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.ConvertBlockBodyToExpressionBodyOrViceVersa)]
        public async Task TestNoDiagnostic_MethodWithAttributes()
        {
            await VerifyNoDiagnosticAsync(@"
using System;

class C
{
    [Obsolete]
    string M(object x, object y) => M(x, y);
}
", options: Options_ConvertExpressionBodyToBlockBodyWhenDeclarationIsMultiLine);
        }
    }
}
