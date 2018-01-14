// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveEnumMemberValueRefactoring
    {
        public static void ComputeRefactoring(
            RefactoringContext context,
            EnumDeclarationSyntax enumDeclaration,
            SeparatedSyntaxListSelection<EnumMemberDeclarationSyntax> selection)
        {
            int count = 0;

            for (int i = selection.StartIndex; i <= selection.EndIndex; i++)
            {
                if (selection.Items[i].EqualsValue?.Value != null)
                {
                    if (count  == 1)
                        break;

                    count++;
                }
            }

            if (count == 0)
                return;

            context.RegisterRefactoring((count == 1)
                ? "Remove enum value"
                : "Remove enum values", cancellationToken => RefactorAsync(context.Document, enumDeclaration, selection, cancellationToken));
        }

        private static Task<Document> RefactorAsync(
            Document document,
            EnumDeclarationSyntax enumDeclaration,
            SeparatedSyntaxListSelection<EnumMemberDeclarationSyntax> selection,
            CancellationToken cancellationToken)
        {
            SeparatedSyntaxList<EnumMemberDeclarationSyntax> newMembers = enumDeclaration.Members;

            for (int i = selection.EndIndex; i < selection.EndIndex; i++)
                newMembers = newMembers.ReplaceAt(i, newMembers[i].WithEqualsValue(null).WithFormatterAnnotation());

            EnumDeclarationSyntax newEnumDeclaration = enumDeclaration.WithMembers(newMembers);

            return document.ReplaceNodeAsync(enumDeclaration, newEnumDeclaration, cancellationToken);
        }
    }
}