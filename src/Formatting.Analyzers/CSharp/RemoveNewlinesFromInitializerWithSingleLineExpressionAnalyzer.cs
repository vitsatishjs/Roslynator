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
    internal class RemoveNewlinesFromInitializerWithSingleLineExpressionAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.RemoveNewlinesFromInitializerWithSingleLineExpression); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(
                f => AnalyzeInitializerExpression(f),
                SyntaxKind.ArrayInitializerExpression,
                SyntaxKind.ObjectInitializerExpression,
                SyntaxKind.CollectionInitializerExpression);
        }

        private static void AnalyzeInitializerExpression(SyntaxNodeAnalysisContext context)
        {
            var initializer = (InitializerExpressionSyntax)context.Node;

            SeparatedSyntaxList<ExpressionSyntax> expressions = initializer.Expressions;

            ExpressionSyntax expression = expressions.SingleOrDefault(shouldThrow: false);

            if (expression == null)
                return;

            if (initializer.SpanContainsDirectives())
                return;

            if (initializer.IsSingleLine(includeExteriorTrivia: false))
                return;

            if (!expression.IsSingleLine())
                return;

            if (!SyntaxTriviaAnalysis.IsExteriorTriviaEmptyOrWhitespace(expression))
                return;

            if (!SyntaxTriviaAnalysis.IsExteriorTriviaEmptyOrWhitespace(initializer.OpenBraceToken))
                return;

            if (!initializer.CloseBraceToken.LeadingTrivia.IsEmptyOrWhitespace())
                return;

            if (expressions.SeparatorCount == 1
                && !SyntaxTriviaAnalysis.IsExteriorTriviaEmptyOrWhitespace(expressions.GetSeparator(0)))
            {
                return;
            }

            if (!initializer.OpenBraceToken.GetPreviousToken().TrailingTrivia.IsEmptyOrWhitespace())
                return;

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.RemoveNewlinesFromInitializerWithSingleLineExpression, initializer);
        }
    }
}
