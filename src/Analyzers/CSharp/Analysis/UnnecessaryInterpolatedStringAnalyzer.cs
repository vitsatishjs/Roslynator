﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using static Roslynator.DiagnosticHelpers;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnnecessaryInterpolatedStringAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.UnnecessaryInterpolatedString,
                    DiagnosticDescriptors.UnnecessaryInterpolatedStringFadeOut);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                if (startContext.IsAnalyzerSuppressed(DiagnosticDescriptors.UnnecessaryInterpolatedString))
                    return;

                startContext.RegisterSyntaxNodeAction(f => AnalyzeInterpolatedStringExpression(f), SyntaxKind.InterpolatedStringExpression);
            });
        }

        private static void AnalyzeInterpolatedStringExpression(SyntaxNodeAnalysisContext context)
        {
            SyntaxNode node = context.Node;

            if (node.ContainsDiagnostics)
                return;

            if (node.ContainsDirectives)
                return;

            var interpolatedString = (InterpolatedStringExpressionSyntax)node;

            SyntaxList<InterpolatedStringContentSyntax> contents = interpolatedString.Contents;

            if (ConvertInterpolatedStringToStringLiteralAnalysis.IsFixable(contents))
            {
                ReportDiagnostic(
                    context,
                    DiagnosticDescriptors.UnnecessaryInterpolatedString,
                    Location.Create(interpolatedString.SyntaxTree, GetDollarSpan(interpolatedString)));
            }
            else
            {
                if (!(contents.SingleOrDefault(shouldThrow: false) is InterpolationSyntax interpolation))
                    return;

                if (interpolation.AlignmentClause != null)
                    return;

                if (interpolation.FormatClause != null)
                    return;

                ExpressionSyntax expression = interpolation.Expression?.WalkDownParentheses();

                if (expression == null)
                    return;

                if (!IsNonNullStringExpression(expression))
                    return;

                ReportDiagnostic(context, DiagnosticDescriptors.UnnecessaryInterpolatedString, interpolatedString);

                ReportToken(context, DiagnosticDescriptors.UnnecessaryInterpolatedStringFadeOut, interpolatedString.StringStartToken);
                ReportToken(context, DiagnosticDescriptors.UnnecessaryInterpolatedStringFadeOut, interpolation.OpenBraceToken);
                ReportToken(context, DiagnosticDescriptors.UnnecessaryInterpolatedStringFadeOut, interpolation.CloseBraceToken);
                ReportToken(context, DiagnosticDescriptors.UnnecessaryInterpolatedStringFadeOut, interpolatedString.StringEndToken);
            }

            bool IsNonNullStringExpression(ExpressionSyntax expression)
            {
                if (expression.IsKind(SyntaxKind.StringLiteralExpression, SyntaxKind.InterpolatedStringExpression))
                    return true;

                Optional<object> constantValue = context.SemanticModel.GetConstantValue(expression, context.CancellationToken);

                return constantValue.HasValue
                    && constantValue.Value is string value
                    && value != null;
            }
        }

        private static TextSpan GetDollarSpan(InterpolatedStringExpressionSyntax interpolatedString)
        {
            SyntaxToken token = interpolatedString.StringStartToken;

            return new TextSpan(token.SpanStart + token.Text.IndexOf('$'), 1);
        }
    }
}
