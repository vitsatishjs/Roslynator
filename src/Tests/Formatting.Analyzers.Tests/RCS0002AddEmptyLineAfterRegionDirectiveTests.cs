﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.Formatting.CodeFixes.CSharp;
using Xunit;

namespace Roslynator.Formatting.CSharp.Tests
{
    public class RCS0002AddEmptyLineAfterRegionDirectiveTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.AddEmptyLineAfterRegionDirective;

        public override DiagnosticAnalyzer Analyzer { get; } = new AddEmptyLineAfterRegionDirectiveAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new DirectiveTriviaCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task Test()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    #region Methods[||]
    void M()
    {
    }

    #endregion
}
", @"
class C
{
    #region Methods

    void M()
    {
    }

    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task Test_NoIndentation()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    #region Methods[||]
    void M()
    {
    }

    #endregion
}
", @"
class C
{
    #region Methods

    void M()
    {
    }

    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task Test_Comment()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    #region Methods[||]
    // x
    void M()
    {
    }

    #endregion
}
", @"
class C
{
    #region Methods

    // x
    void M()
    {
    }

    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task Test_DocumentationComment()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    #region Methods[||]
    /// <summary>
    /// 
    /// </summary>
    void M()
    {
    }

    #endregion
}
", @"
class C
{
    #region Methods

    /// <summary>
    /// 
    /// </summary>
    void M()
    {
    }

    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task TestNoDiagnostic()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    #region Methods

    void M()
    {
    }
    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task TestNoDiagnostic_EmptyRegion()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    #region Methods
    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task TestNoDiagnostic_Comment()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    #region Methods

    // x
    void M()
    {
    }
    // x
    #endregion
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.AddEmptyLineAfterRegionDirective)]
        public async Task TestNoDiagnostic_DocumentationComment()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    #region Methods

    /// <summary>
    /// 
    /// </summary>
    void M()
    {
    }
    #endregion
}
");
        }
    }
}
