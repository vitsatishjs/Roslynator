﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Roslynator
{
    /// <summary>
    /// A set of extension methods for a <see cref="SemanticModel"/>.
    /// </summary>
    public static class SemanticModelExtensions
    {
        internal static Diagnostic GetDiagnostic(
            this SemanticModel semanticModel,
            string id,
            TextSpan? span = null,
            CancellationToken cancellationToken = default)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ImmutableArray<Diagnostic> diagnostics = semanticModel.GetDiagnostics(span, cancellationToken);

            for (int i = 0; i < diagnostics.Length; i++)
            {
                if (string.Equals(diagnostics[i].Id, id, StringComparison.Ordinal))
                    return diagnostics[i];
            }

            return null;
        }

        /// <summary>
        /// Returns the innermost named type symbol that the specified position is considered inside of.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="position"></param>
        /// <param name="cancellationToken"></param>
        public static INamedTypeSymbol GetEnclosingNamedType(
            this SemanticModel semanticModel,
            int position,
            CancellationToken cancellationToken = default)
        {
            return GetEnclosingSymbol<INamedTypeSymbol>(semanticModel, position, cancellationToken);
        }

        /// <summary>
        /// Returns the innermost symbol of type <typeparamref name="TSymbol"/> that the specified position is considered inside of.
        /// </summary>
        /// <typeparam name="TSymbol"></typeparam>
        /// <param name="semanticModel"></param>
        /// <param name="position"></param>
        /// <param name="cancellationToken"></param>
        public static TSymbol GetEnclosingSymbol<TSymbol>(
            this SemanticModel semanticModel,
            int position,
            CancellationToken cancellationToken = default) where TSymbol : ISymbol
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ISymbol symbol = semanticModel.GetEnclosingSymbol(position, cancellationToken);

            while (symbol != null)
            {
                if (symbol is TSymbol tsymbol)
                    return tsymbol;

                symbol = symbol.ContainingSymbol;
            }

            return default;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified node bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="node"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            SyntaxNode node,
            CancellationToken cancellationToken = default)
        {
            return ModelExtensions.GetSymbolInfo(semanticModel, node, cancellationToken).Symbol;
        }

        /// <summary>
        /// Returns type information about the specified node.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="node"></param>
        /// <param name="cancellationToken"></param>
        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            SyntaxNode node,
            CancellationToken cancellationToken = default)
        {
            return ModelExtensions.GetTypeInfo(semanticModel, node, cancellationToken).Type;
        }

        /// <summary>
        /// Returns the type within the compilation's assembly using its canonical CLR metadata name.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="fullyQualifiedMetadataName"></param>
        public static INamedTypeSymbol GetTypeByMetadataName(this SemanticModel semanticModel, string fullyQualifiedMetadataName)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            return semanticModel
                .Compilation
                .GetTypeByMetadataName(fullyQualifiedMetadataName);
        }

        internal static IMethodSymbol GetSpeculativeMethodSymbol(
            this SemanticModel semanticModel,
            int position,
            SyntaxNode expression)
        {
            SymbolInfo symbolInfo = ModelExtensions.GetSpeculativeSymbolInfo(
                semanticModel,
                position,
                expression,
                SpeculativeBindingOption.BindAsExpression);

            return symbolInfo.Symbol as IMethodSymbol;
        }

        internal static ImmutableArray<ISymbol> GetSymbolsDeclaredInEnclosingSymbol(
            this SemanticModel semanticModel,
            int position,
            bool excludeAnonymousTypeProperty = false,
            CancellationToken cancellationToken = default)
        {
            SyntaxNode node = GetEnclosingSymbolSyntax(semanticModel, position, cancellationToken);

            if (node != null)
                return GetDeclaredSymbols(semanticModel, node, excludeAnonymousTypeProperty, cancellationToken);

            return ImmutableArray<ISymbol>.Empty;
        }

        internal static SyntaxNode GetEnclosingSymbolSyntax(
            this SemanticModel semanticModel,
            int position,
            CancellationToken cancellationToken = default)
        {
            ISymbol enclosingSymbol = semanticModel.GetEnclosingSymbol(position, cancellationToken);

            ImmutableArray<SyntaxReference> syntaxReferences = enclosingSymbol.DeclaringSyntaxReferences;

            if (syntaxReferences.Length == 1)
            {
                return syntaxReferences[0].GetSyntax(cancellationToken);
            }
            else
            {
                foreach (SyntaxReference syntaxReference in syntaxReferences)
                {
                    SyntaxNode node = syntaxReference.GetSyntax(cancellationToken);

                    if (node.SyntaxTree == semanticModel.SyntaxTree
                        && node.FullSpan.Contains(position))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        internal static ImmutableArray<ISymbol> GetDeclaredSymbols(
            this SemanticModel semanticModel,
            SyntaxNode container,
            bool excludeAnonymousTypeProperty = false,
            CancellationToken cancellationToken = default)
        {
            HashSet<ISymbol> symbols = null;

            foreach (SyntaxNode node in container.DescendantNodesAndSelf())
            {
                ISymbol symbol = semanticModel.GetDeclaredSymbol(node, cancellationToken);

                if (symbol != null)
                {
                    if (!excludeAnonymousTypeProperty
                        || !symbol.IsPropertyOfAnonymousType())
                    {
                        (symbols ??= new HashSet<ISymbol>()).Add(symbol);
                    }
                }
            }

            return symbols?.ToImmutableArray() ?? ImmutableArray<ISymbol>.Empty;
        }
    }
}
