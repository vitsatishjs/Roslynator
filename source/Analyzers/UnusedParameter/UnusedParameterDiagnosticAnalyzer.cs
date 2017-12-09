// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.DiagnosticAnalyzers;
using static Roslynator.CSharp.Analyzers.UnusedParameter.UnusedParameterRefactoring;

namespace Roslynator.CSharp.Analyzers.UnusedParameter
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnusedParameterDiagnosticAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DiagnosticDescriptors.UnusedParameter,
                    DiagnosticDescriptors.UnusedThisParameter,
                    DiagnosticDescriptors.UnusedTypeParameter);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            base.Initialize(context);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeConstructorDeclaration, SyntaxKind.ConstructorDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeIndexerDeclaration, SyntaxKind.IndexerDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeLocalFunctionStatement, SyntaxKind.LocalFunctionStatement);
            context.RegisterSyntaxNodeAction(AnalyzeSimpleLambdaExpression, SyntaxKind.SimpleLambdaExpression);
            context.RegisterSyntaxNodeAction(AnalyzeParenthesizedLambdaExpression, SyntaxKind.ParenthesizedLambdaExpression);
        }
    }
}
