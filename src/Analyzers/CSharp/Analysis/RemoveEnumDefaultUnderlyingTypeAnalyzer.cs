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
    public class RemoveEnumDefaultUnderlyingTypeAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.RemoveEnumDefaultUnderlyingType); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeEnumDeclaration(f), SyntaxKind.EnumDeclaration);
        }

        private static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            TypeSyntax type = enumDeclaration
                .BaseList?
                .Types
                .SingleOrDefault(shouldThrow: false)?
                .Type;

            if (type?.IsMissing == false
                && context.SemanticModel.GetTypeSymbol(type, context.CancellationToken)?.SpecialType == SpecialType.System_Int32)
            {
                DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.RemoveEnumDefaultUnderlyingType, type);
            }
        }
    }
}
