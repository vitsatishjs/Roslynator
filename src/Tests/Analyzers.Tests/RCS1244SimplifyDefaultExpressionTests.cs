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
    public class RCS1244SimplifyDefaultExpressionTests : AbstractCSharpFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.SimplifyDefaultExpression;

        public override DiagnosticAnalyzer Analyzer { get; } = new DefaultExpressionAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new DefaultExpressionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task Test_ParameterDefaultValue()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s = default[|(string)|])
    {
    }
}
", @"
class C
{
    void M(string s = default)
    {
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task Test_ExpressionBody()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M() => default[|(string)|];
}
", @"
class C
{
    string M() => default;
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task Test_ReturnStatement()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    string M()
    {
        return default[|(string)|];
    }
}
", @"
class C
{
    string M()
    {
        return default;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task Test_YieldReturnStatement()
        {
            await VerifyDiagnosticAndFixAsync(@"
using System.Collections.Generic;

class C
{
    IEnumerable<string> M()
    {
        yield return default[|(string)|];
    }
}
", @"
using System.Collections.Generic;

class C
{
    IEnumerable<string> M()
    {
        yield return default;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_NonObjectValueAssignedToObject()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        object x = null;

        x = default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_NonNullableValueAssignedToNullable()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        int? x = null;

        x = default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ValueAssignedToDynamic()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        dynamic x = null;

        x = default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ConditionalExpression()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        bool condition = false;

        object x = null;

        x = (condition) ? x : default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ConditionalExpression_Nullable()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        bool condition = false;

        int? x = null;

        x = (condition) ? 1 : default(int?);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ReturnStatement()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    object M()
    {
        return default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_CoalesceExpression()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
        object x = null;

        x = x ?? default(int);
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ExpressionBody()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    object M() => default(int);
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ObjectInitializer()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    string P { get; set; }

    void M()
    {
        var x = new C() { P = default(string) };
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_ConstantPattern()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
		object x = default;

		if (x is default(object))
		{
		}
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_CaseSwitchLabel()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
		bool x = false;

		switch (x)
		{
			case default(bool):
				break;
		}
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_Argument()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        M(default(string));
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_CSharp7()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s = default(string))
    {
    }
}
", options: CSharpCodeVerificationOptions.Default_CSharp7);
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.SimplifyDefaultExpression)]
        public async Task TestNoDiagnostic_Nullable()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
            int? x = null;

            if (x == default(int)) { }
    }
}
");
        }
    }
}
