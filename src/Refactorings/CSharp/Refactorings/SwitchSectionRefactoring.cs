﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.CSharp.Analysis;

namespace Roslynator.CSharp.Refactorings
{
    internal static class SwitchSectionRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, SwitchSectionSyntax switchSection)
        {
            if (SelectedStatementsRefactoring.IsAnyRefactoringEnabled(context)
                && StatementListSelection.TryCreate(switchSection, context.Span, out StatementListSelection selectedStatements))
            {
                await SelectedStatementsRefactoring.ComputeRefactoringAsync(context, selectedStatements).ConfigureAwait(false);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.SortCaseLabels)
                && SyntaxListSelection<SwitchLabelSyntax>.TryCreate(switchSection.Labels, context.Span, out SyntaxListSelection<SwitchLabelSyntax> selectedLabels)
                && selectedLabels.Count > 1)
            {
                SortCaseLabelsRefactoring.ComputeRefactoring(context, selectedLabels);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.SplitSwitchLabels))
                SplitSwitchLabelsRefactoring.ComputeRefactoring(context, switchSection);

            if (context.IsAnyRefactoringEnabled(
                RefactoringIdentifiers.AddBracesToSwitchSection,
                RefactoringIdentifiers.AddBracesToSwitchSections,
                RefactoringIdentifiers.RemoveBracesFromSwitchSection,
                RefactoringIdentifiers.RemoveBracesFromSwitchSections)
                && context.Span.IsEmpty
                && IsContainedInCaseOrDefaultKeyword(context.Span))
            {
                var switchStatement = (SwitchStatementSyntax)switchSection.Parent;

                SyntaxList<SwitchSectionSyntax> sections = switchStatement.Sections;

                BracesAnalysis analysis = BracesAnalysis.AnalyzeBraces(switchSection);

                if (analysis.AddBraces)
                {
                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.AddBracesToSwitchSection))
                    {
                        context.RegisterRefactoring(
                            AddBracesToSwitchSectionRefactoring.Title,
                            cancellationToken => AddBracesToSwitchSectionRefactoring.RefactorAsync(context.Document, switchSection, cancellationToken),
                            RefactoringIdentifiers.AddBracesToSwitchSection);
                    }

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.AddBracesToSwitchSections)
                        && sections.Any(f => f != switchSection && AddBracesToSwitchSectionAnalysis.CanAddBraces(f)))
                    {
                        context.RegisterRefactoring(
                            AddBracesToSwitchSectionsRefactoring.Title,
                            cancellationToken => AddBracesToSwitchSectionsRefactoring.RefactorAsync(context.Document, switchStatement, null, cancellationToken),
                            RefactoringIdentifiers.AddBracesToSwitchSections);
                    }
                }
                else if (analysis.RemoveBraces)
                {
                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.RemoveBracesFromSwitchSection))
                    {
                        context.RegisterRefactoring(
                            RemoveBracesFromSwitchSectionRefactoring.Title,
                            cancellationToken => RemoveBracesFromSwitchSectionRefactoring.RefactorAsync(context.Document, switchSection, cancellationToken),
                            RefactoringIdentifiers.RemoveBracesFromSwitchSection);
                    }

                    if (context.IsRefactoringEnabled(RefactoringIdentifiers.RemoveBracesFromSwitchSections)
                        && sections.Any(f => f != switchSection && RemoveBracesFromSwitchSectionRefactoring.CanRemoveBraces(f)))
                    {
                        context.RegisterRefactoring(
                            RemoveBracesFromSwitchSectionsRefactoring.Title,
                            cancellationToken => RemoveBracesFromSwitchSectionsRefactoring.RefactorAsync(context.Document, switchStatement, null, cancellationToken),
                            RefactoringIdentifiers.RemoveBracesFromSwitchSections);
                    }
                }
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.DuplicateSwitchSection))
                DuplicateSwitchSectionRefactoring.ComputeRefactoring(context, switchSection);

            bool IsContainedInCaseOrDefaultKeyword(TextSpan span)
            {
                foreach (SwitchLabelSyntax label in switchSection.Labels)
                {
                    if (label.Keyword.Span.Contains(span))
                        return true;
                }

                return false;
            }
        }
    }
}