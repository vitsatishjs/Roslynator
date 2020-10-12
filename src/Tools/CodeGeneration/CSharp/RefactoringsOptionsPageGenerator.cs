﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp;
using Roslynator.Metadata;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CodeGeneration.CSharp
{
    public static class RefactoringsOptionsPageGenerator
    {
        public static CompilationUnitSyntax Generate(IEnumerable<RefactoringMetadata> refactorings, IComparer<string> comparer)
        {
            return CompilationUnit(
                UsingDirectives(
                    "System.Collections.Generic",
                    "Roslynator.CSharp.Refactorings"),
                NamespaceDeclaration(
                    "Roslynator.VisualStudio",
                    ClassDeclaration(
                        Modifiers.Public_Partial(),
                        "RefactoringsOptionsPage",
                        CreateMembers(refactorings, comparer).ToSyntaxList())));
        }

        private static IEnumerable<MemberDeclarationSyntax> CreateMembers(IEnumerable<RefactoringMetadata> refactorings, IComparer<string> comparer)
        {
            yield return PropertyDeclaration(
                Modifiers.Protected_Override(),
                PredefinedStringType(),
                Identifier("MaxId"),
                AccessorList(AutoGetAccessorDeclaration()),
                ParseExpression($"RefactoringIdentifiers.{refactorings.OrderBy(f => f.Id, comparer).Last().Identifier}"));

            yield return MethodDeclaration(
                Modifiers.Internal_Static(),
                VoidType(),
                Identifier("SetRefactoringsDisabledByDefault"),
                ParameterList(Parameter(IdentifierName("RefactoringSettings"), Identifier("settings"))),
                Block(refactorings
                    .Where(f => !f.IsEnabledByDefault)
                    .OrderBy(f => f.Identifier, comparer)
                    .Select(refactoring =>
                    {
                        return ExpressionStatement(
                            ParseExpression($"settings.Disable(RefactoringIdentifiers.{refactoring.Identifier})"));
                    })));

            yield return MethodDeclaration(
                Modifiers.Protected_Override(),
                VoidType(),
                Identifier("Fill"),
                ParameterList(Parameter(ParseTypeName("ICollection<BaseModel>"), Identifier("refactorings"))),
                Block(
                    SingletonList(ExpressionStatement(ParseExpression("refactorings.Clear()")))
                        .AddRange(refactorings
                            .OrderBy(f => f.Id, comparer)
                            .Select(refactoring =>
                            {
                                return ExpressionStatement(
                                    ParseExpression($"refactorings.Add(new BaseModel(RefactoringIdentifiers.{refactoring.Identifier}, \"{StringUtility.EscapeQuote(refactoring.Title)}\", IsEnabled(RefactoringIdentifiers.{refactoring.Identifier})))"));
                            }))));
        }
    }
}
