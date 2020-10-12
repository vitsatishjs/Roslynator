﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseUnaryOperatorInsteadOfAssignmentAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignment,
                    DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                if (startContext.IsAnalyzerSuppressed(DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignment))
                    return;

                startContext.RegisterSyntaxNodeAction(f => AnalyzeSimpleAssignmentExpression(f), SyntaxKind.SimpleAssignmentExpression);
                startContext.RegisterSyntaxNodeAction(f => AnalyzeAddAssignmentExpression(f), SyntaxKind.AddAssignmentExpression);
                startContext.RegisterSyntaxNodeAction(f => AnalyzeSubtractAssignmentExpression(f), SyntaxKind.SubtractAssignmentExpression);
            });
        }

        private static void AnalyzeSimpleAssignmentExpression(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.SpanContainsDirectives())
                return;

            var assignment = (AssignmentExpressionSyntax)context.Node;

            if (assignment.IsParentKind(SyntaxKind.ObjectInitializerExpression))
                return;

            ExpressionSyntax left = assignment.Left;
            ExpressionSyntax right = assignment.Right;

            if (left?.IsMissing != false)
                return;

            if (right?.IsKind(SyntaxKind.AddExpression, SyntaxKind.SubtractExpression) != true)
                return;

            var binaryExpression = (BinaryExpressionSyntax)right;

            ExpressionSyntax binaryLeft = binaryExpression.Left;
            ExpressionSyntax binaryRight = binaryExpression.Right;

            if (binaryLeft?.IsMissing != false)
                return;

            if (binaryRight?.IsNumericLiteralExpression("1") != true)
                return;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(left, context.CancellationToken);

            if (typeSymbol?.SupportsPrefixOrPostfixUnaryOperator() != true)
                return;

            if (!CSharpFactory.AreEquivalent(left, binaryLeft))
                return;

            string operatorText = GetOperatorText(assignment);

            ReportDiagnostic(context, assignment, operatorText);

            DiagnosticHelpers.ReportToken(context, DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut, assignment.OperatorToken, operatorText);
            DiagnosticHelpers.ReportNode(context, DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut, binaryLeft, operatorText);
            DiagnosticHelpers.ReportNode(context, DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut, binaryRight, operatorText);
        }

        private static void AnalyzeAddAssignmentExpression(SyntaxNodeAnalysisContext context)
        {
            AnalyzeAddOrSubtractAssignmentExpression(context);
        }

        private static void AnalyzeSubtractAssignmentExpression(SyntaxNodeAnalysisContext context)
        {
            AnalyzeAddOrSubtractAssignmentExpression(context);
        }

        private static void AnalyzeAddOrSubtractAssignmentExpression(SyntaxNodeAnalysisContext context)
        {
            if (context.Node.SpanContainsDirectives())
                return;

            var assignment = (AssignmentExpressionSyntax)context.Node;

            ExpressionSyntax left = assignment.Left;
            ExpressionSyntax right = assignment.Right;

            if (left?.IsMissing != false)
                return;

            if (right?.IsNumericLiteralExpression("1") != true)
                return;

            ITypeSymbol typeSymbol = context.SemanticModel.GetTypeSymbol(left, context.CancellationToken);

            if (typeSymbol?.SupportsPrefixOrPostfixUnaryOperator() != true)
                return;

            string operatorText = GetOperatorText(assignment);

            ReportDiagnostic(context, assignment, operatorText);

            SyntaxToken operatorToken = assignment.OperatorToken;

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut, Location.Create(assignment.SyntaxTree, new TextSpan(operatorToken.SpanStart, 1)), operatorText);
            DiagnosticHelpers.ReportNode(context, DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignmentFadeOut, assignment.Right, operatorText);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, AssignmentExpressionSyntax assignment, string operatorText)
        {
            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.UseUnaryOperatorInsteadOfAssignment,
                assignment,
                operatorText);
        }

        public static bool UseIncrementOperator(AssignmentExpressionSyntax assignment)
        {
            return (assignment.Kind()) switch
            {
                SyntaxKind.AddAssignmentExpression => true,
                SyntaxKind.SubtractAssignmentExpression => false,
                _ => (assignment.Right?.Kind()) switch
                {
                    SyntaxKind.AddExpression => true,
                    SyntaxKind.SubtractExpression => false,
                    _ => throw new InvalidOperationException(),
                },
            };
        }

        public static string GetOperatorText(AssignmentExpressionSyntax assignment)
        {
            return (UseIncrementOperator(assignment)) ? "++" : "--";
        }
    }
}
