﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp;

namespace Roslynator.Formatting.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class AddNewLineBetweenClosingBraceAndWhileKeywordOrViceVersaAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.AddNewLineBetweenClosingBraceAndWhileKeywordOrViceVersa); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeDoStatement(f), SyntaxKind.DoStatement);
        }

        private static void AnalyzeDoStatement(SyntaxNodeAnalysisContext context)
        {
            var doStatement = (DoStatementSyntax)context.Node;

            StatementSyntax statement = doStatement.Statement;

            if (!statement.IsKind(SyntaxKind.Block))
                return;

            SyntaxTriviaList trailingTrivia = statement.GetTrailingTrivia();

            if (!trailingTrivia.Any()
                || trailingTrivia.SingleOrDefault(shouldThrow: false).IsWhitespaceTrivia())
            {
                if (!doStatement.WhileKeyword.LeadingTrivia.Any()
                    && context.IsAnalyzerSuppressed(AnalyzerOptions.RemoveNewLineBetweenClosingBraceAndWhileKeyword))
                {
                    context.ReportDiagnostic(
                        DiagnosticDescriptors.AddNewLineBetweenClosingBraceAndWhileKeywordOrViceVersa,
                        Location.Create(doStatement.SyntaxTree, new TextSpan(statement.FullSpan.End, 0)));
                }
            }
            else if (SyntaxTriviaAnalysis.IsOptionalWhitespaceThenEndOfLineTrivia(trailingTrivia))
            {
                if (doStatement.WhileKeyword.LeadingTrivia.IsEmptyOrWhitespace()
                    && !context.IsAnalyzerSuppressed(AnalyzerOptions.RemoveNewLineBetweenClosingBraceAndWhileKeyword))
                {
                    context.ReportDiagnostic(
                        DiagnosticDescriptors.ReportOnly.RemoveNewLineBetweenClosingBraceAndWhileKeyword,
                        Location.Create(doStatement.SyntaxTree, new TextSpan(trailingTrivia.Last().SpanStart, 0)),
                        properties: DiagnosticProperties.AnalyzerOption_Invert);
                }
            }
        }
    }
}
