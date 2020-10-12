﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class LocalFunctionStatementRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement.IsParentKind(SyntaxKind.Block))
            {
                BlockSyntax body = localFunctionStatement.Body;

                if (body != null)
                {
                    if (body.OpenBraceToken.Span.Contains(context.Span)
                        || body.CloseBraceToken.Span.Contains(context.Span))
                    {
                        if (context.IsRefactoringEnabled(RefactoringIdentifiers.RemoveMember))
                        {
                            context.RegisterRefactoring(CodeActionFactory.RemoveStatement(context.Document, localFunctionStatement, equivalenceKey: RefactoringIdentifiers.RemoveMember));
                        }

                        if (context.IsRefactoringEnabled(RefactoringIdentifiers.DuplicateMember))
                        {
                            context.RegisterRefactoring(
                                "Duplicate local function",
                                cancellationToken => DuplicateMemberDeclarationRefactoring.RefactorAsync(context.Document, localFunctionStatement, cancellationToken),
                                RefactoringIdentifiers.DuplicateMember);
                        }

                        if (context.IsRefactoringEnabled(RefactoringIdentifiers.CommentOutMember))
                            CommentOutRefactoring.RegisterRefactoring(context, localFunctionStatement);
                    }
                }
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.ChangeMethodReturnTypeToVoid)
                && context.Span.IsEmptyAndContainedInSpan(localFunctionStatement))
            {
                await ChangeMethodReturnTypeToVoidRefactoring.ComputeRefactoringAsync(context, localFunctionStatement).ConfigureAwait(false);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.AddTypeParameter))
                AddTypeParameterRefactoring.ComputeRefactoring(context, localFunctionStatement);

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.ConvertBlockBodyToExpressionBody)
                && ConvertBlockBodyToExpressionBodyRefactoring.CanRefactor(localFunctionStatement, context.Span))
            {
                context.RegisterRefactoring(
                    ConvertBlockBodyToExpressionBodyRefactoring.Title,
                    cancellationToken => ConvertBlockBodyToExpressionBodyRefactoring.RefactorAsync(context.Document, localFunctionStatement, cancellationToken),
                    RefactoringIdentifiers.ConvertBlockBodyToExpressionBody);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.MoveUnsafeContextToContainingDeclaration))
                MoveUnsafeContextToContainingDeclarationRefactoring.ComputeRefactoring(context, localFunctionStatement);
        }
    }
}
