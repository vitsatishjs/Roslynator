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
    public class UseShortCircuitingOperatorAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.UseShortCircuitingOperator); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => Analyze(f), SyntaxKind.BitwiseAndExpression);
            context.RegisterSyntaxNodeAction(f => Analyze(f), SyntaxKind.BitwiseOrExpression);
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;

            if (binaryExpression.ContainsDiagnostics)
                return;

            ISymbol symbol = context.SemanticModel.GetSymbol(binaryExpression, context.CancellationToken);

            if (symbol?.Kind != SymbolKind.Method)
                return;

            if (symbol.ContainingType?.SpecialType != SpecialType.System_Boolean)
                return;

            switch (symbol.MetadataName)
            {
                case WellKnownMemberNames.BitwiseAndOperatorName:
                case WellKnownMemberNames.BitwiseOrOperatorName:
                    break;
                default:
                    return;
            }

            DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.UseShortCircuitingOperator, binaryExpression.OperatorToken);
        }
    }
}
