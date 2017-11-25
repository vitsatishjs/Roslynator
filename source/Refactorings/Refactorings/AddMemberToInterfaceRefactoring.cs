// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.CodeFixes;
using Roslynator.CSharp.Comparers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AddMemberToInterfaceRefactoring
    {
        public static void ComputeRefactoring(
            RefactoringContext context,
            MethodDeclarationSyntax methodDeclaration,
            SemanticModel semanticModel)
        {
            if (methodDeclaration.TypeParameterList?.Parameters.Any() == true)
                return;

            ComputeRefactoring(context, methodDeclaration, methodDeclaration.Modifiers, methodDeclaration.ExplicitInterfaceSpecifier, semanticModel);
        }

        public static void ComputeRefactoring(RefactoringContext context, PropertyDeclarationSyntax propertyDeclaration, SemanticModel semanticModel)
        {
            ComputeRefactoring(context, propertyDeclaration, propertyDeclaration.Modifiers, propertyDeclaration.ExplicitInterfaceSpecifier, semanticModel);
        }

        public static void ComputeRefactoring(RefactoringContext context, IndexerDeclarationSyntax indexerDeclaration, SemanticModel semanticModel)
        {
            ComputeRefactoring(context, indexerDeclaration, indexerDeclaration.Modifiers, indexerDeclaration.ExplicitInterfaceSpecifier, semanticModel);
        }

        public static void ComputeRefactoring(RefactoringContext context, EventDeclarationSyntax eventDeclaration, SemanticModel semanticModel)
        {
            ComputeRefactoring(context, eventDeclaration, eventDeclaration.Modifiers, eventDeclaration.ExplicitInterfaceSpecifier, semanticModel);
        }

        private static void ComputeRefactoring(
            RefactoringContext context,
            MemberDeclarationSyntax memberDeclaration,
            SyntaxTokenList modifiers,
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier,
            SemanticModel semanticModel)
        {
            if (!modifiers.Contains(SyntaxKind.PublicKeyword))
                return;

            if (modifiers.Contains(SyntaxKind.StaticKeyword))
                return;

            BaseListSyntax baseList = GetBaseList(memberDeclaration.Parent);

            if (baseList == null)
                return;

            SeparatedSyntaxList<BaseTypeSyntax> types = baseList.Types;

            if (!types.Any())
                return;

            NameSyntax explicitInterfaceName = explicitInterfaceSpecifier?.Name;

            ITypeSymbol explicitInterfaceSymbol = (explicitInterfaceName != null)
                ? semanticModel.GetTypeSymbol(explicitInterfaceName, context.CancellationToken)
                : null;

            if (!(semanticModel.GetDeclaredSymbol(memberDeclaration.Parent) is INamedTypeSymbol containingTypeSymbol))
                return;

            foreach (BaseTypeSyntax baseType in types)
                ComputeRefactoring(context, memberDeclaration, baseType, explicitInterfaceSymbol, containingTypeSymbol, semanticModel);
        }

        private static void ComputeRefactoring(
            RefactoringContext context,
            MemberDeclarationSyntax memberDeclaration,
            BaseTypeSyntax baseType,
            ITypeSymbol explicitInterfaceSymbol,
            INamedTypeSymbol containingTypeSymbol,
            SemanticModel semanticModel)
        {
            TypeSyntax type = baseType.Type;

            if (type == null)
                return;

            ITypeSymbol interfaceSymbol = semanticModel.GetTypeSymbol(type, context.CancellationToken);

            if (interfaceSymbol?.TypeKind != TypeKind.Interface)
                return;

            if (!(interfaceSymbol.GetSyntaxOrDefault(context.CancellationToken) is InterfaceDeclarationSyntax interfaceDeclaration))
                return;

            if (interfaceSymbol.Equals(explicitInterfaceSymbol))
                return;

            ImmutableArray<ISymbol> members = interfaceSymbol.GetMembers();

            SyntaxKind kind = memberDeclaration.Kind();

            for (int i = 0; i < members.Length; i++)
            {
                if (CheckKind(members[i], kind))
                {
                    ISymbol other = containingTypeSymbol.FindImplementationForInterfaceMember(members[i]);

                    if (other != null
                        && interfaceSymbol.Equals(other.ContainingType))
                    {
                        return;
                    }
                }
            }

            string displayName = SymbolDisplay.GetMinimalString(interfaceSymbol, semanticModel, type.SpanStart);

            context.RegisterRefactoring(
                $"Add to interface '{displayName}'",
                cancellationToken => RefactorAsync(context.Document, memberDeclaration, interfaceDeclaration, cancellationToken),
                RefactoringIdentifiers.AddMemberToInterface + "." + displayName);
        }

        private static bool CheckKind(ISymbol symbol, SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.MethodDeclaration:
                    {
                        return symbol.Kind == SymbolKind.Method;
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        return symbol.Kind == SymbolKind.Property
                            && !((IPropertySymbol)symbol).IsIndexer;
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        return symbol.Kind == SymbolKind.Property
                            && ((IPropertySymbol)symbol).IsIndexer;
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        return symbol.Kind == SymbolKind.Event;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private static BaseListSyntax GetBaseList(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax classDeclaration)
                return classDeclaration.BaseList;

            if (node is StructDeclarationSyntax structDeclaration)
                return structDeclaration.BaseList;

            return null;
        }

        private static Task<Document> RefactorAsync(
            Document document,
            MemberDeclarationSyntax memberDeclaration,
            InterfaceDeclarationSyntax interfaceDeclaration,
            CancellationToken cancellationToken)
        {
            MemberDeclarationSyntax interfaceMember = CreateInterfaceMemberDeclaration(memberDeclaration).WithFormatterAnnotation();

            InterfaceDeclarationSyntax newNode = interfaceDeclaration.InsertMember(interfaceMember, MemberDeclarationComparer.ByKind);

            return document.ReplaceNodeAsync(interfaceDeclaration, newNode, cancellationToken);
        }

        private static MemberDeclarationSyntax CreateInterfaceMemberDeclaration(MemberDeclarationSyntax memberDeclaration)
        {
            switch (memberDeclaration)
            {
                case MethodDeclarationSyntax methodDeclaration:
                    {
                        return MethodDeclaration(
                            default(SyntaxList<AttributeListSyntax>),
                            default(SyntaxTokenList),
                            methodDeclaration.ReturnType,
                            default(ExplicitInterfaceSpecifierSyntax),
                            methodDeclaration.Identifier,
                            default(TypeParameterListSyntax),
                            methodDeclaration.ParameterList,
                            default(SyntaxList<TypeParameterConstraintClauseSyntax>),
                            default(BlockSyntax),
                            SemicolonToken());
                    }
                case PropertyDeclarationSyntax propertyDeclaration:
                    {
                        return PropertyDeclaration(
                            default(SyntaxList<AttributeListSyntax>),
                            default(SyntaxTokenList),
                            propertyDeclaration.Type,
                            default(ExplicitInterfaceSpecifierSyntax),
                            propertyDeclaration.Identifier,
                            CreateInterfaceAccessorList(propertyDeclaration.AccessorList),
                            default(ArrowExpressionClauseSyntax),
                            default(EqualsValueClauseSyntax),
                            SemicolonToken());
                    }
                case IndexerDeclarationSyntax indexerDeclaration:
                    {
                        return IndexerDeclaration(
                            default(SyntaxList<AttributeListSyntax>),
                            default(SyntaxTokenList),
                            indexerDeclaration.Type,
                            default(ExplicitInterfaceSpecifierSyntax),
                            indexerDeclaration.ThisKeyword,
                            indexerDeclaration.ParameterList,
                            CreateInterfaceAccessorList(indexerDeclaration.AccessorList),
                            default(ArrowExpressionClauseSyntax),
                            SemicolonToken());
                    }
                case EventDeclarationSyntax eventDeclaration:
                    {
                        return EventDeclaration(
                            default(SyntaxList<AttributeListSyntax>),
                            default(SyntaxTokenList),
                            eventDeclaration.EventKeyword,
                            eventDeclaration.Type,
                            default(ExplicitInterfaceSpecifierSyntax),
                            eventDeclaration.Identifier,
                            CreateInterfaceAccessorList(eventDeclaration.AccessorList));
                    }
                default:
                    {
                        throw new ArgumentException("", nameof(memberDeclaration));
                    }
            }
        }

        private static AccessorListSyntax CreateInterfaceAccessorList(AccessorListSyntax accessorList)
        {
            return AccessorList(accessorList
                .Accessors
                .Select(f => f.WithBody(null).WithExpressionBody(null).WithSemicolonToken(SemicolonToken()))
                .ToSyntaxList());
        }
    }
}
