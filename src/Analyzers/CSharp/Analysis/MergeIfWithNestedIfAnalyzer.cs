﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MergeIfWithNestedIfAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.MergeIfWithNestedIf,
                    DiagnosticDescriptors.MergeIfWithNestedIfFadeOut);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterCompilationStartAction(startContext =>
            {
                if (startContext.IsAnalyzerSuppressed(DiagnosticDescriptors.MergeIfWithNestedIf))
                    return;

                startContext.RegisterSyntaxNodeAction(f => AnalyzeIfStatement(f), SyntaxKind.IfStatement);
            });
        }

        private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.SpanContainsDirectives())
                return;

            SimpleIfStatementInfo simpleIf = SyntaxInfo.SimpleIfStatementInfo(ifStatement);

            if (!simpleIf.Success)
                return;

            if (simpleIf.Condition.Kind() == SyntaxKind.LogicalOrExpression)
                return;

            SimpleIfStatementInfo nestedIf = SyntaxInfo.SimpleIfStatementInfo(GetNestedIfStatement(ifStatement));

            if (!nestedIf.Success)
                return;

            if (nestedIf.Condition.Kind() == SyntaxKind.LogicalOrExpression)
                return;

            if (!CheckTrivia(ifStatement, nestedIf.IfStatement))
                return;

            ReportDiagnostic(context, ifStatement, nestedIf.IfStatement);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, IfStatementSyntax ifStatement, IfStatementSyntax nestedIf)
        {
            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.MergeIfWithNestedIf, ifStatement);

            DiagnosticHelpers.ReportToken(context, DiagnosticDescriptors.MergeIfWithNestedIfFadeOut, nestedIf.IfKeyword);
            DiagnosticHelpers.ReportToken(context, DiagnosticDescriptors.MergeIfWithNestedIfFadeOut, nestedIf.OpenParenToken);
            DiagnosticHelpers.ReportToken(context, DiagnosticDescriptors.MergeIfWithNestedIfFadeOut, nestedIf.CloseParenToken);

            if (ifStatement.Statement.IsKind(SyntaxKind.Block)
                && nestedIf.Statement.IsKind(SyntaxKind.Block))
            {
                CSharpDiagnosticHelpers.ReportBraces(context, DiagnosticDescriptors.MergeIfWithNestedIfFadeOut, (BlockSyntax)nestedIf.Statement);
            }
        }

        private static bool CheckTrivia(IfStatementSyntax ifStatement, IfStatementSyntax nestedIf)
        {
            TextSpan span = TextSpan.FromBounds(
                nestedIf.FullSpan.Start,
                nestedIf.CloseParenToken.FullSpan.End);

            if (nestedIf.DescendantTrivia(span).All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            {
                if (ifStatement.Statement.IsKind(SyntaxKind.Block)
                    && nestedIf.Statement.IsKind(SyntaxKind.Block))
                {
                    var block = (BlockSyntax)nestedIf.Statement;

                    return block.OpenBraceToken.LeadingTrivia.IsEmptyOrWhitespace()
                        && block.OpenBraceToken.TrailingTrivia.IsEmptyOrWhitespace()
                        && block.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace()
                        && block.CloseBraceToken.TrailingTrivia.IsEmptyOrWhitespace();
                }

                return true;
            }

            return false;
        }

        internal static IfStatementSyntax GetNestedIfStatement(IfStatementSyntax ifStatement)
        {
            StatementSyntax statement = ifStatement.Statement;

            switch (statement?.Kind())
            {
                case SyntaxKind.Block:
                    return ((BlockSyntax)statement).Statements.SingleOrDefault(shouldThrow: false) as IfStatementSyntax;
                case SyntaxKind.IfStatement:
                    return (IfStatementSyntax)statement;
            }

            return null;
        }
    }
}
