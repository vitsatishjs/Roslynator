﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseExplicitTypeInsteadOfVarWhenTypeIsObviousAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.UseExplicitTypeInsteadOfVarWhenTypeIsObvious); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeVariableDeclaration(f), SyntaxKind.VariableDeclaration);
            context.RegisterSyntaxNodeAction(f => AnalyzeDeclarationExpression(f), SyntaxKind.DeclarationExpression);
        }

        private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclarationSyntax)context.Node;

            if (CSharpTypeAnalysis.IsImplicitThatCanBeExplicit(variableDeclaration, context.SemanticModel, TypeAppearance.Obvious, context.CancellationToken))
                ReportDiagnostic(context, variableDeclaration.Type);
        }

        private static void AnalyzeDeclarationExpression(SyntaxNodeAnalysisContext context)
        {
            var declarationExpression = (DeclarationExpressionSyntax)context.Node;

            if (declarationExpression.IsParentKind(SyntaxKind.ForEachVariableStatement))
                return;

            if (CSharpTypeAnalysis.IsImplicitThatCanBeExplicit(declarationExpression, context.SemanticModel, context.CancellationToken))
                ReportDiagnostic(context, declarationExpression.Type);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, TypeSyntax type)
        {
            DiagnosticHelpers.ReportDiagnostic(
                context,
                DiagnosticDescriptors.UseExplicitTypeInsteadOfVarWhenTypeIsObvious,
                type);
        }
    }
}
