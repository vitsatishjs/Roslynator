﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class InitializeLocalWithDefaultValueRefactoring
    {
        public static async Task ComputeRefactoringAsync(RefactoringContext context, LocalDeclarationStatementSyntax localDeclaration)
        {
            VariableDeclarationSyntax declaration = localDeclaration.Declaration;

            if (declaration == null)
                return;

            SeparatedSyntaxList<VariableDeclaratorSyntax> variables = declaration.Variables;

            if (!variables.Any())
                return;

            VariableDeclaratorSyntax declarator = variables.FirstOrDefault(f => f.FullSpan.Contains(context.Span));

            if (declarator?.Identifier.IsMissing != false)
                return;

            EqualsValueClauseSyntax initializer = declarator.Initializer;

            if (initializer?.Value?.IsMissing == false)
                return;

            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(declaration.Type, context.CancellationToken);

            if (typeSymbol?.IsErrorType() != false)
                return;

            context.RegisterRefactoring(
                $"Initialize '{declarator.Identifier.ValueText}' with default value",
                cancellationToken => RefactorAsync(context.Document, localDeclaration, declarator, typeSymbol, cancellationToken),
                RefactoringIdentifiers.InitializeLocalWithDefaultValue);
        }

        public static Task<Document> RefactorAsync(
            Document document,
            LocalDeclarationStatementSyntax localDeclaration,
            VariableDeclaratorSyntax declarator,
            ITypeSymbol typeSymbol,
            CancellationToken cancellationToken = default)
        {
            VariableDeclaratorSyntax newDeclarator = GetNewDeclarator(localDeclaration.Declaration.Type.WithoutTrivia());

            LocalDeclarationStatementSyntax newNode = localDeclaration.ReplaceNode(declarator, newDeclarator);

            if (localDeclaration.SemicolonToken.IsMissing
                && localDeclaration.Declaration.Variables.Last().Equals(declarator))
            {
                newNode = newNode.WithSemicolonToken(SemicolonToken().WithTrailingTrivia(newDeclarator.GetTrailingTrivia()));
            }

            return document.ReplaceNodeAsync(localDeclaration, newNode, cancellationToken);

            VariableDeclaratorSyntax GetNewDeclarator(TypeSyntax type)
            {
                ExpressionSyntax value = typeSymbol.GetDefaultValueSyntax(document.GetDefaultSyntaxOptions(), type);

                EqualsValueClauseSyntax initializer = declarator.Initializer;
                EqualsValueClauseSyntax newInitializer = EqualsValueClause(value);

                if (initializer?.IsMissing != false)
                {
                    return declarator
                        .WithIdentifier(declarator.Identifier.WithoutTrailingTrivia())
                        .WithInitializer(newInitializer.WithTrailingTrivia(declarator.Identifier.TrailingTrivia));
                }
                else
                {
                    return declarator
                        .WithInitializer(newInitializer.WithTriviaFrom(initializer.EqualsToken));
                }
            }
        }
    }
}
