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
    public class AddBracesToIfElseWhenMultiLineAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.AddBracesToIfElseWhenExpressionSpansOverMultipleLines); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeIfStatement(f), SyntaxKind.IfStatement);
        }

        private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            if (ifStatement.IsParentKind(SyntaxKind.ElseClause))
                return;

            if (ifStatement.Else == null)
                return;

            if (ifStatement.Else.ContainsDirectives)
                return;

            if (ifStatement.Statement.ContainsDirectives)
                return;

            BracesAnalysis analysis = BracesAnalysis.AnalyzeBraces(ifStatement);

            if (!analysis.AddBraces)
                return;

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.AddBracesToIfElseWhenExpressionSpansOverMultipleLines, ifStatement);
        }
    }
}
