﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SimplifyConditionalExpressionAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.SimplifyConditionalExpression); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeConditionalExpression(f), SyntaxKind.ConditionalExpression);
        }

        private static void AnalyzeConditionalExpression(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.ContainsDiagnostics)
                return;

            if (context.Node.SpanContainsDirectives())
                return;

            var conditionalExpression = (ConditionalExpressionSyntax)context.Node;

            ConditionalExpressionInfo info = SyntaxInfo.ConditionalExpressionInfo(conditionalExpression);

            if (!info.Success)
                return;

            SyntaxKind trueKind = info.WhenTrue.Kind();
            SyntaxKind falseKind = info.WhenFalse.Kind();

            if (trueKind == SyntaxKind.TrueLiteralExpression)
            {
                // a ? true : false >>> a
                // a ? true : b >>> a || b
                if (falseKind == SyntaxKind.FalseLiteralExpression
                    || context.SemanticModel.GetTypeInfo(info.WhenFalse, context.CancellationToken).ConvertedType?.SpecialType == SpecialType.System_Boolean)
                {
                    ReportDiagnostic();
                }
            }
            else if (trueKind == SyntaxKind.FalseLiteralExpression)
            {
                /// a ? false : true >>> !a
                if (falseKind == SyntaxKind.TrueLiteralExpression)
                {
                    ReportDiagnostic();
                }
                /// a ? false : b >>> !a && b
                else if (!context.IsAnalyzerSuppressed(AnalyzerOptions.SimplifyConditionalExpressionWhenItIncludesNegationOfCondition)
                    && context.SemanticModel.GetTypeInfo(info.WhenFalse, context.CancellationToken).ConvertedType?.SpecialType == SpecialType.System_Boolean)
                {
                    ReportDiagnostic();
                }
            }
            else if (falseKind == SyntaxKind.TrueLiteralExpression)
            {
                // a ? b : true >>> !a || b
                if (!context.IsAnalyzerSuppressed(AnalyzerOptions.SimplifyConditionalExpressionWhenItIncludesNegationOfCondition)
                    && context.SemanticModel.GetTypeInfo(info.WhenTrue, context.CancellationToken).ConvertedType?.SpecialType == SpecialType.System_Boolean)
                {
                    ReportDiagnostic();
                }
            }
            else if (falseKind == SyntaxKind.FalseLiteralExpression)
            {
                // a ? b : false >>> a && b
                if (context.SemanticModel.GetTypeInfo(info.WhenTrue, context.CancellationToken).ConvertedType?.SpecialType == SpecialType.System_Boolean)
                {
                    ReportDiagnostic();
                }
            }

            void ReportDiagnostic()
            {
                DiagnosticHelpers.ReportDiagnosticIfNotSuppressed(context, DiagnosticDescriptors.SimplifyConditionalExpression, conditionalExpression);
            }
        }
    }
}
