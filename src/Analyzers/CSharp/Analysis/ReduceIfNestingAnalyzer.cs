﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.Analysis.ReduceIfNesting;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReduceIfNestingAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.ReduceIfNesting); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeIfStatement(f), SyntaxKind.IfStatement);
        }

        private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
        {
            var ifStatement = (IfStatementSyntax)context.Node;

            ReduceIfNestingAnalysisResult analysis = ReduceIfNestingAnalysis.Analyze(
                ifStatement,
                context.SemanticModel,
                options: ReduceIfNestingOptions.None,
                cancellationToken: context.CancellationToken);

            if (!analysis.Success)
                return;

            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.ReduceIfNesting,
                ifStatement.IfKeyword.GetLocation(),
                ImmutableDictionary.CreateRange(new[] { new KeyValuePair<string, string>("JumpKind", analysis.JumpKind.ToString()) }));
        }
    }
}
