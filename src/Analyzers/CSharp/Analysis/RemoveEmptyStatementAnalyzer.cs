﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RemoveEmptyStatementAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.RemoveEmptyStatement); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeEmptyStatement(f), SyntaxKind.EmptyStatement);
        }

        private static void AnalyzeEmptyStatement(SyntaxNodeAnalysisContext context)
        {
            SyntaxNode emptyStatement = context.Node;

            SyntaxNode parent = emptyStatement.Parent;

            if (parent == null)
                return;

            SyntaxKind kind = parent.Kind();

            if (kind == SyntaxKind.LabeledStatement)
                return;

            if (CSharpFacts.CanHaveEmbeddedStatement(kind))
                return;

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.RemoveEmptyStatement, emptyStatement);
        }
    }
}
