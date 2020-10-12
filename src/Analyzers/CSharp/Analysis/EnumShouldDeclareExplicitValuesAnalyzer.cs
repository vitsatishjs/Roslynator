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
    public class EnumShouldDeclareExplicitValuesAnalyzer : BaseDiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(DiagnosticDescriptors.EnumShouldDeclareExplicitValues); }
        }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(f => AnalyzeEnumDeclaration(f), SyntaxKind.EnumDeclaration);
        }

        private static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            foreach (EnumMemberDeclarationSyntax enumMember in enumDeclaration.Members)
            {
                if (enumMember.EqualsValue == null
                    && context.SemanticModel.GetDeclaredSymbol(enumMember, context.CancellationToken)?.HasConstantValue == true)
                {
                    DiagnosticHelpers.ReportDiagnostic(context, DiagnosticDescriptors.EnumShouldDeclareExplicitValues, enumDeclaration.Identifier);
                    break;
                }
            }
        }
    }
}
