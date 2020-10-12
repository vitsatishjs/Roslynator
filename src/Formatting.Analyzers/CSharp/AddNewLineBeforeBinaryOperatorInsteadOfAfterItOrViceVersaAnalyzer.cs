﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class AddNewLineBeforeBinaryOperatorInsteadOfAfterItOrViceVersaAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.AddNewLineBeforeBinaryOperatorInsteadOfAfterItOrViceVersa); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(
                f => AnalyzeBinaryExpression(f),
                SyntaxKind.AddExpression,
                SyntaxKind.SubtractExpression,
                SyntaxKind.MultiplyExpression,
                SyntaxKind.DivideExpression,
                SyntaxKind.ModuloExpression,
                SyntaxKind.LeftShiftExpression,
                SyntaxKind.RightShiftExpression,
                SyntaxKind.LogicalOrExpression,
                SyntaxKind.LogicalAndExpression,
                SyntaxKind.BitwiseOrExpression,
                SyntaxKind.BitwiseAndExpression,
                SyntaxKind.ExclusiveOrExpression,
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression,
                SyntaxKind.LessThanExpression,
                SyntaxKind.LessThanOrEqualExpression,
                SyntaxKind.GreaterThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression,
                SyntaxKind.IsExpression,
                SyntaxKind.AsExpression);
        }

        private static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;

            ExpressionSyntax left = binaryExpression.Left;

            if (left.IsMissing)
                return;

            ExpressionSyntax right = binaryExpression.Right;

            if (right.IsMissing)
                return;

            if (SyntaxTriviaAnalysis.IsTokenFollowedWithNewLineAndNotPrecededWithNewLine(left, binaryExpression.OperatorToken, right))
            {
                if (context.IsAnalyzerSuppressed(AnalyzerOptions.AddNewLineAfterBinaryOperatorInsteadOfBeforeIt))
                    ReportDiagnostic(DiagnosticDescriptors.AddNewLineBeforeBinaryOperatorInsteadOfAfterItOrViceVersa, ImmutableDictionary<string, string>.Empty);
            }
            else if (SyntaxTriviaAnalysis.IsTokenPrecededWithNewLineAndNotFollowedWithNewLine(left, binaryExpression.OperatorToken, right)
                && !context.IsAnalyzerSuppressed(AnalyzerOptions.AddNewLineAfterBinaryOperatorInsteadOfBeforeIt))
            {
                ReportDiagnostic(DiagnosticDescriptors.ReportOnly.AddNewLineAfterBinaryOperatorInsteadOfBeforeIt, DiagnosticProperties.AnalyzerOption_Invert);
            }

            void ReportDiagnostic(DiagnosticDescriptor descriptor, ImmutableDictionary<string, string> properties)
            {
                if (CSharpUtility.IsStringConcatenation(binaryExpression, context.SemanticModel, context.CancellationToken))
                    return;

                DiagnosticHelpers.ReportDiagnostic(
                    context,
                    descriptor,
                    Location.Create(binaryExpression.SyntaxTree, binaryExpression.OperatorToken.Span.WithLength(0)),
                    properties: properties);
            }
        }
    }
}
