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
    public class AvoidUsageOfUsingAliasDirectiveAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.AvoidUsageOfUsingAliasDirective); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeUsingDirective(f), SyntaxKind.UsingDirective);
        }

        private static void AnalyzeUsingDirective(SyntaxNodeAnalysisContext context)
        {
            var usingDirective = (UsingDirectiveSyntax)context.Node;

            if (usingDirective.Alias == null)
                return;

            if (usingDirective.ContainsDiagnostics)
                return;

            if (usingDirective.SpanContainsDirectives())
                return;

            if (context.SemanticModel
                .GetSymbol(usingDirective.Name, context.CancellationToken)?
                .IsKind(SymbolKind.Namespace, SymbolKind.NamedType) == true)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.AvoidUsageOfUsingAliasDirective, usingDirective);
            }
        }
    }
}
