﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class UseLambdaExpressionInsteadOfAnonymousMethodRefactoring
    {
        public static Task<Document> RefactorAsync(
            Document document,
            AnonymousMethodExpressionSyntax anonymousMethod,
            CancellationToken cancellationToken = default)
        {
            ExpressionSyntax newNode = ParenthesizedLambdaExpression(
                anonymousMethod.AsyncKeyword,
                anonymousMethod.ParameterList,
                Token(SyntaxKind.EqualsGreaterThanToken),
                anonymousMethod.Block);

            newNode = newNode
                .WithTriviaFrom(anonymousMethod)
                .Parenthesize()
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(anonymousMethod, newNode, cancellationToken);
        }
    }
}
