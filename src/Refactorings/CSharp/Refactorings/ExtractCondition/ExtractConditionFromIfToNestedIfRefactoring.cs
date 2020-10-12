﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings.ExtractCondition
{
    internal sealed class ExtractConditionFromIfToNestedIfRefactoring : ExtractConditionFromIfRefactoring
    {
        private ExtractConditionFromIfToNestedIfRefactoring()
        {
        }

        public static ExtractConditionFromIfToNestedIfRefactoring Instance { get; } = new ExtractConditionFromIfToNestedIfRefactoring();

        public override string Title
        {
            get { return "Extract condition to nested if"; }
        }

        public Task<Document> RefactorAsync(
            Document document,
            BinaryExpressionSyntax condition,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            var ifStatement = (IfStatementSyntax)condition.Parent;

            IfStatementSyntax newIfStatement = RemoveExpressionFromCondition(ifStatement, condition, expression);

            IfStatementSyntax newNode = AddNestedIf(newIfStatement, expression).WithFormatterAnnotation();

            return document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken);
        }

        public Task<Document> RefactorAsync(
            Document document,
            IfStatementSyntax ifStatement,
            BinaryExpressionSyntax condition,
            in ExpressionChain expressionChain,
            CancellationToken cancellationToken)
        {
            IfStatementSyntax newNode = RemoveExpressionsFromCondition(ifStatement, condition, expressionChain);

            ExpressionSyntax expression = SyntaxFactory.ParseExpression(expressionChain.ToString());

            newNode = AddNestedIf(newNode, expression).WithFormatterAnnotation();

            return document.ReplaceNodeAsync(ifStatement, newNode, cancellationToken);
        }
    }
}
