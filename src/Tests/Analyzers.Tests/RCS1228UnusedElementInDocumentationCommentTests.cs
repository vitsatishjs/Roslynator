﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1228UnusedElementInDocumentationCommentTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.UnusedElementInDocumentationComment;

        public override DiagnosticAnalyzer Analyzer { get; } = new SingleLineDocumentationCommentTriviaAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new XmlNodeCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_FirstElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// [|<returns></returns>|]
    /// <summary>
    /// </summary>
    void M()
    {
    }
}
", @"
class C
{
    /// <summary>
    /// </summary>
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_LastElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary>
    /// </summary>
    /// [|<returns></returns>|]
    void M()
    {
    }
}
", @"
class C
{
    /// <summary>
    /// </summary>
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_EmptyElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary>
    /// </summary>
    /// [|<returns />|]
    void M()
    {
    }
}
", @"
class C
{
    /// <summary>
    /// </summary>
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ReturnsIsOnlyElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// [|<returns></returns>|]
    void M()
    {
    }
}
", @"
class C
{
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ExampleElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<example></example>|]
    void M() => M();
}
", @"
class C
{
    /// <summary></summary>
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_RemarksElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<remarks></remarks>|]
    void M() => M();
}
", @"
class C
{
    /// <summary></summary>
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ValueElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<value></value>|]
    void M() => M();
}
", @"
class C
{
    /// <summary></summary>
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ParamElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<param name=""p""></param>|]
    void M(object p1, object p2) => M(p1, p2);
}
", @"
class C
{
    /// <summary></summary>
    void M(object p1, object p2) => M(p1, p2);
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ParamElement2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<param name=""p""></param>|]
    void M() => M();
}
", @"
class C
{
    /// <summary></summary>
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_ParamElement_Empty()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<param name=""p"" />|]
    void M(object p1, object p2) => M(p1, p2);
}
", @"
class C
{
    /// <summary></summary>
    void M(object p1, object p2) => M(p1, p2);
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_TypeParamElement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<typeparam name=""T""></param>|]
    void M<T1, T2>() => M<T1, T2>();
}
", @"
class C
{
    /// <summary></summary>
    void M<T1, T2>() => M<T1, T2>();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_TypeParamElement2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<typeparam name=""T""></param>|]
    void M() => M();
}
", @"
class C
{
    /// <summary></summary>
    void M() => M();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task Test_TypeParamElement_Empty()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    /// <summary></summary>
    /// [|<typeparam name=""T"" />|]
    void M<T1, T2>() => M<T1, T2>();
}
", @"
class C
{
    /// <summary></summary>
    void M<T1, T2>() => M<T1, T2>();
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task TestNoDiagnostic_NoReturnsElement()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    /// <summary>
    /// </summary>
    void M()
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.UnusedElementInDocumentationComment)]
        public async Task TestNoDiagnostic_NonEmpty()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    /// <summary>
    /// </summary>
    /// <returns>x</returns>
    void M()
    {
    }
}
");
        }
    }
}
