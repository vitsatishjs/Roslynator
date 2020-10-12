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
    public class AddOrRemoveRegionNameAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.AddOrRemoveRegionName); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeEndRegionDirectiveTrivia(f), SyntaxKind.EndRegionDirectiveTrivia);
        }

        private static void AnalyzeEndRegionDirectiveTrivia(SyntaxNodeAnalysisContext context)
        {
            var endRegionDirective = (EndRegionDirectiveTriviaSyntax)context.Node;

            RegionDirectiveTriviaSyntax regionDirective = endRegionDirective.GetRegionDirective();

            if (regionDirective == null)
                return;

            SyntaxTrivia trivia = regionDirective.GetPreprocessingMessageTrivia();

            SyntaxTrivia endTrivia = endRegionDirective.GetPreprocessingMessageTrivia();

            if (trivia.Kind() == SyntaxKind.PreprocessingMessageTrivia)
            {
                if (endTrivia.Kind() != SyntaxKind.PreprocessingMessageTrivia
                    || !string.Equals(trivia.ToString(), endTrivia.ToString(), StringComparison.Ordinal))
                {
                    DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.AddOrRemoveRegionName, endRegionDirective, "Add", "to");
                }
            }
            else if (endTrivia.Kind() == SyntaxKind.PreprocessingMessageTrivia)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.AddOrRemoveRegionName, endRegionDirective, "Remove", "from");
            }
        }
    }
}
