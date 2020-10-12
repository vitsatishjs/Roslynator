﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseEmptyStringLiteralInsteadOfStringEmptyOrViceVersaAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.UseEmptyStringLiteralInsteadOfStringEmptyOrViceVersa); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                if (startContext.IsAnalyzerSuppressed(AnalyzerOptions.UseStringEmptyInsteadOfEmptyStringLiteral))
                {
                    startContext.RegisterSyntaxNodeAction(f => AnalyzeSimpleMemberAccessExpression(f), SyntaxKind.SimpleMemberAccessExpression);
                }
                else
                {
                    startContext.RegisterSyntaxNodeAction(f => AnalyzeStringLiteralExpression(f), SyntaxKind.StringLiteralExpression);
                    startContext.RegisterSyntaxNodeAction(f => AnalyzeInterpolatedStringExpression(f), SyntaxKind.InterpolatedStringExpression);
                }
            });
        }

        private static void AnalyzeSimpleMemberAccessExpression(SyntaxNodeAnalysisContext context)
        {
            var memberAccess = (MemberAccessExpressionSyntax)context.Node;

            if (memberAccess.IsParentKind(SyntaxKind.SimpleMemberAccessExpression))
                return;

            if (memberAccess.Expression == null)
                return;

            if (memberAccess.Name?.Identifier.ValueText != "Empty")
                return;

            var fieldSymbol = context.SemanticModel.GetSymbol(memberAccess.Name, context.CancellationToken) as IFieldSymbol;

            if (!SymbolUtility.IsPublicStaticReadOnly(fieldSymbol))
                return;

            if (fieldSymbol.ContainingType?.SpecialType != SpecialType.System_String)
                return;

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.UseEmptyStringLiteralInsteadOfStringEmptyOrViceVersa, memberAccess);
        }

        private static void AnalyzeStringLiteralExpression(SyntaxNodeAnalysisContext context)
        {
            var literalExpression = (LiteralExpressionSyntax)context.Node;

            if (literalExpression.Token.ValueText.Length > 0)
                return;

            if (CSharpUtility.IsPartOfExpressionThatMustBeConstant(literalExpression))
                return;

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.ReportOnly.UseStringEmptyInsteadOfEmptyStringLiteral,
                literalExpression);
        }

        private static void AnalyzeInterpolatedStringExpression(SyntaxNodeAnalysisContext context)
        {
            var interpolatedString = (InterpolatedStringExpressionSyntax)context.Node;

            if (interpolatedString.Contents.Any())
                return;

            if (CSharpUtility.IsPartOfExpressionThatMustBeConstant(interpolatedString))
                return;

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.ReportOnly.UseStringEmptyInsteadOfEmptyStringLiteral,
                interpolatedString);
        }
    }
}
