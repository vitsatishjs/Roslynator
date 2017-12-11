// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Roslynator.CSharp.Analyzers.UnusedParameter
{
    internal static class UnusedSyntaxWalkerCache
    {
        [ThreadStatic]
        private static UnusedSyntaxWalker _cachedInstance;

        public static UnusedSyntaxWalker Acquire(SemanticModel semanticModel, CancellationToken cancellationToken, bool isIndexer = false)
        {
            UnusedSyntaxWalker walker = _cachedInstance;

            if (walker != null)
            {
                _cachedInstance = null;
                walker.Symbols.Clear();
                walker.IsAnyTypeParameter = false;
            }
            else
            {
                walker = new UnusedSyntaxWalker();
            }

            walker.SemanticModel = semanticModel;
            walker.CancellationToken = cancellationToken;
            walker.IsIndexer = isIndexer;

            return walker;
        }

        public static void Release(UnusedSyntaxWalker walker)
        {
            _cachedInstance = walker;
        }

        public static ImmutableArray<SyntaxNode> GetNodesAndRelease(UnusedSyntaxWalker walker)
        {
            Dictionary<string, NodeSymbolInfo> names = walker.Symbols;

            Release(walker);

            if (names.Count > 0)
                return ImmutableArray.CreateRange(names.Select(f => f.Value.Node));

            return ImmutableArray<SyntaxNode>.Empty;
        }
    }
}

