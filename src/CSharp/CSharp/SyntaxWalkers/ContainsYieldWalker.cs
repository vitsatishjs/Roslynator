﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.SyntaxWalkers
{
    internal sealed class ContainsYieldWalker : StatementWalker
    {
        [ThreadStatic]
        private static ContainsYieldWalker _cachedInstance;

        public ContainsYieldWalker(
            bool searchForYieldBreak = true,
            bool searchForYieldReturn = true)
        {
            SearchForYieldBreak = searchForYieldBreak;
            SearchForYieldReturn = searchForYieldReturn;
        }

        public override bool ShouldVisit
        {
            get { return YieldStatement == null; }
        }

        public bool SearchForYieldBreak { get; private set; }

        public bool SearchForYieldReturn { get; private set; }

        public YieldStatementSyntax YieldStatement { get; private set; }

        public static bool ContainsYield(StatementSyntax statement, bool searchForYieldReturn = true, bool searchForYieldBreak = true)
        {
            if (statement == null)
                throw new ArgumentNullException(nameof(statement));

            ContainsYieldWalker walker = GetInstance();
            walker.SearchForYieldBreak = searchForYieldBreak;
            walker.SearchForYieldReturn = searchForYieldReturn;

            walker.VisitStatement(statement);

            bool success = walker.YieldStatement != null;

            Free(walker);

            return success;
        }

        public override void VisitYieldStatement(YieldStatementSyntax node)
        {
            SyntaxKind kind = node.Kind();

            Debug.Assert(kind.Is(SyntaxKind.YieldBreakStatement, SyntaxKind.YieldReturnStatement), kind.ToString());

            if (kind == SyntaxKind.YieldReturnStatement)
            {
                if (SearchForYieldReturn)
                    YieldStatement = node;
            }
            else if (kind == SyntaxKind.YieldBreakStatement)
            {
                if (SearchForYieldBreak)
                    YieldStatement = node;
            }
        }

        public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
        }

        public static ContainsYieldWalker GetInstance()
        {
            ContainsYieldWalker walker = _cachedInstance;

            if (walker != null)
            {
                Debug.Assert(walker.YieldStatement == null);

                _cachedInstance = null;
                return walker;
            }

            return new ContainsYieldWalker();
        }

        public static void Free(ContainsYieldWalker walker)
        {
            walker.SearchForYieldBreak = true;
            walker.SearchForYieldReturn = true;
            walker.YieldStatement = null;

            _cachedInstance = walker;
        }
    }
}
