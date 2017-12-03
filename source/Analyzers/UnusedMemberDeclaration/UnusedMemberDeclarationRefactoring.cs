// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analyzers.UnusedMemberDeclaration
{
    internal static class UnusedMemberDeclarationRefactoring
    {
        public static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            AnalyzeTypeDeclaration(context, (TypeDeclarationSyntax)context.Node);
        }

        public static void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
        {
            var structDeclaration = (StructDeclarationSyntax)context.Node;

            AnalyzeTypeDeclaration(context, (TypeDeclarationSyntax)context.Node);
        }

        private static void AnalyzeTypeDeclaration(SyntaxNodeAnalysisContext context, TypeDeclarationSyntax typeDeclaration)
        {
            SyntaxList<MemberDeclarationSyntax> members = typeDeclaration.Members;

            UnusedMemberDeclarationWalker walker = null;

            foreach (MemberDeclarationSyntax member in members)
            {
                if (member.ContainsDiagnostics)
                    continue;

                if (member.ContainsDirectives)
                    continue;

                switch (member.Kind())
                {
                    case SyntaxKind.DelegateDeclaration:
                        {
                            var declaration = (DelegateDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                walker.Symbols.Add(new NodeSymbolInfo(declaration.Identifier.ValueText, declaration));
                            }

                            break;
                        }
                    case SyntaxKind.EventDeclaration:
                        {
                            var declaration = (EventDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                walker.Symbols.Add(new NodeSymbolInfo(declaration.Identifier.ValueText, declaration));
                            }

                            break;
                        }
                    case SyntaxKind.EventFieldDeclaration:
                        {
                            var declaration = (EventFieldDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                foreach (VariableDeclaratorSyntax declarator in declaration.Declaration.Variables)
                                    walker.Symbols.Add(new NodeSymbolInfo(declarator.Identifier.ValueText, declarator));
                            }

                            break;
                        }
                    case SyntaxKind.FieldDeclaration:
                        {
                            var declaration = (FieldDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                foreach (VariableDeclaratorSyntax declarator in declaration.Declaration.Variables)
                                    walker.Symbols.Add(new NodeSymbolInfo(declarator.Identifier.ValueText, declarator));
                            }

                            break;
                        }
                    case SyntaxKind.MethodDeclaration:
                        {
                            var declaration = (MethodDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                walker.Symbols.Add(new NodeSymbolInfo(declaration.Identifier.ValueText, declaration));
                            }

                            break;
                        }
                    case SyntaxKind.PropertyDeclaration:
                        {
                            var declaration = (PropertyDeclarationSyntax)member;

                            if (IsPrivate(declaration, declaration.Modifiers))
                            {
                                if (walker == null)
                                    walker = UnusedMemberDeclarationWalkerCache.Acquire(context.SemanticModel, context.CancellationToken);

                                walker.Symbols.Add(new NodeSymbolInfo(declaration.Identifier.ValueText, declaration));
                            }

                            break;
                        }
                }
            }

            if (walker == null)
                return;

            walker.Visit(typeDeclaration);

            foreach (NodeSymbolInfo info in UnusedMemberDeclarationWalkerCache.GetExpressionsAndRelease(walker))
            {
                context.ReportDiagnostic(DiagnosticDescriptors.RemoveUnusedMemberDeclaration, info.Node, GetTitle(info.Node));
            }
        }

        private static string GetTitle(SyntaxNode node)
        {
            if (node.Kind() == SyntaxKind.VariableDeclarator)
                return node.Parent.Parent.GetTitle();

            return node.GetTitle();
        }

        private static bool IsPrivate(MemberDeclarationSyntax memberDeclaration, SyntaxTokenList modifiers)
        {
            Accessibility accessibility = modifiers.GetAccessibility();

            if (accessibility == Accessibility.NotApplicable)
                accessibility = memberDeclaration.GetDefaultExplicitAccessibility();

            return accessibility == Accessibility.Private;
        }

        public static Task<Document> RefactorAsync(
            Document document,
            SyntaxNode node,
            CancellationToken cancellationToken)
        {
            if (node.Kind() == SyntaxKind.VariableDeclarator)
            {
                var variableDeclaration = (VariableDeclarationSyntax)node.Parent;

                if (variableDeclaration.Variables.Count == 1)
                {
                    SyntaxNode parent = variableDeclaration.Parent;

                    return document.RemoveNodeAsync(parent, RemoveHelper.GetRemoveOptions(parent), cancellationToken);
                }
            }

            return document.RemoveNodeAsync(node, RemoveHelper.GetRemoveOptions(node), cancellationToken);
        }
    }
}
