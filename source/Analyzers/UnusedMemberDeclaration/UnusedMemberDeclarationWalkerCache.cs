// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Roslynator.CSharp.Analyzers.UnusedMemberDeclaration
{
    internal static class UnusedMemberDeclarationWalkerCache
    {
        [ThreadStatic]
        private static UnusedMemberDeclarationWalker _cachedInstance;

        public static UnusedMemberDeclarationWalker Acquire(SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            UnusedMemberDeclarationWalker walker = _cachedInstance;

            if (walker != null)
            {
                _cachedInstance = null;
                walker.Symbols.Clear();
            }
            else
            {
                //TODO:
                Debug.WriteLine("new UnusedMemberDeclarationWalker");

                walker = new UnusedMemberDeclarationWalker();
            }

            walker.SemanticModel = semanticModel;
            walker.CancellationToken = cancellationToken;

            return walker;
        }

        public static void Release(UnusedMemberDeclarationWalker walker)
        {
            _cachedInstance = walker;
        }

        public static List<NodeSymbolInfo> GetExpressionsAndRelease(UnusedMemberDeclarationWalker walker)
        {
            List<NodeSymbolInfo> names = walker.Symbols;

            Release(walker);

            return names;
        }
    }
}
