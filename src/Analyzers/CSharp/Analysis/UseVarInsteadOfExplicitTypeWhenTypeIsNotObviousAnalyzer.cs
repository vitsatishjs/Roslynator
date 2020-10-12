﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseVarInsteadOfExplicitTypeWhenTypeIsNotObviousAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.UseVarInsteadOfExplicitTypeWhenTypeIsNotObvious); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeVariableDeclaration(f), SyntaxKind.VariableDeclaration);
            context.RegisterSyntaxNodeAction(f => AnalyzeDeclarationExpression(f), SyntaxKind.DeclarationExpression);
            context.RegisterSyntaxNodeAction(f => AnalyzeTupleExpression(f), SyntaxKind.TupleExpression);
        }

        private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclarationSyntax)context.Node;

            if (CSharpTypeAnalysis.IsExplicitThatCanBeImplicit(variableDeclaration, context.SemanticModel, TypeAppearance.NotObvious, context.CancellationToken))
            {
                DiagnosticHelpers.ReportDiagnostic(
                    context,
                    DiagnosticDescriptors.UseVarInsteadOfExplicitTypeWhenTypeIsNotObvious,
                    variableDeclaration.Type);
            }
        }

        private static void AnalyzeDeclarationExpression(SyntaxNodeAnalysisContext context)
        {
            var declarationExpression = (DeclarationExpressionSyntax)context.Node;

            if (declarationExpression.IsParentKind(SyntaxKind.Argument)
                && declarationExpression.Parent.IsParentKind(SyntaxKind.TupleExpression))
            {
                return;
            }

            if (CSharpTypeAnalysis.IsExplicitThatCanBeImplicit(declarationExpression, context.SemanticModel, context.CancellationToken))
                ReportDiagnostic(context, declarationExpression.Type);
        }

        private static void AnalyzeTupleExpression(SyntaxNodeAnalysisContext context)
        {
            var tupleExpression = (TupleExpressionSyntax)context.Node;

            if (tupleExpression.IsParentKind(SyntaxKind.ForEachVariableStatement))
                return;

            if (CSharpTypeAnalysis.IsExplicitThatCanBeImplicit(tupleExpression, context.SemanticModel, context.CancellationToken))
                ReportDiagnostic(context, tupleExpression);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node)
        {
            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.UseVarInsteadOfExplicitTypeWhenTypeIsNotObvious,
                node);
        }
    }
}
