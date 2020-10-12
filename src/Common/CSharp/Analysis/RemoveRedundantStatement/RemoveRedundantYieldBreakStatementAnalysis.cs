﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.SyntaxWalkers;

namespace Roslynator.CSharp.Analysis.RemoveRedundantStatement
{
    internal sealed class RemoveRedundantYieldBreakStatementAnalysis : RemoveRedundantStatementAnalysis<YieldStatementSyntax>
    {
        public static RemoveRedundantYieldBreakStatementAnalysis Instance { get; } = new RemoveRedundantYieldBreakStatementAnalysis();

        private RemoveRedundantYieldBreakStatementAnalysis()
        {
        }

        protected override bool IsFixable(StatementSyntax statement, StatementSyntax containingStatement, BlockSyntax block, SyntaxKind parentKind)
        {
            if (!parentKind.Is(
                SyntaxKind.MethodDeclaration,
                SyntaxKind.LocalFunctionStatement))
            {
                return false;
            }

            SyntaxList<StatementSyntax> statements = block.Statements;

            if (object.ReferenceEquals(statements.SingleOrDefault(ignoreLocalFunctions: true, shouldThrow: false), containingStatement))
                return false;

            ContainsYieldWalker walker = ContainsYieldWalker.GetInstance();

            var success = false;

            int index = statements.IndexOf(containingStatement);

            for (int i = 0; i < index; i++)
            {
                walker.VisitStatement(statements[i]);

                success = walker.YieldStatement != null;

                if (success)
                    break;
            }

            ContainsYieldWalker.Free(walker);

            return success;
        }
    }
}
