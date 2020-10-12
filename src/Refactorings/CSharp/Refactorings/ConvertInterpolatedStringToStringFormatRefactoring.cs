﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ConvertInterpolatedStringToStringFormatRefactoring
    {
        public static void ComputeRefactoring(RefactoringContext context, InterpolatedStringExpressionSyntax interpolatedString)
        {
            var containsInterpolation = false;
            var containsInterpolatedText = false;

            Analyze();

            if (!containsInterpolation)
                return;

            if (!containsInterpolatedText)
                return;

            context.RegisterRefactoring(
                "Convert to 'string.Format'",
                ct => RefactorAsync(context.Document, interpolatedString, ct),
                RefactoringIdentifiers.ConvertInterpolatedStringToStringFormat);

            void Analyze()
            {
                foreach (InterpolatedStringContentSyntax content in interpolatedString.Contents)
                {
                    switch (content.Kind())
                    {
                        case SyntaxKind.Interpolation:
                            {
                                containsInterpolation = true;

                                if (containsInterpolatedText)
                                    return;

                                break;
                            }
                        case SyntaxKind.InterpolatedStringText:
                            {
                                containsInterpolatedText = true;

                                if (containsInterpolation)
                                    return;

                                break;
                            }
                    }
                }
            }
        }

        private static Task<Document> RefactorAsync(
            Document document,
            InterpolatedStringExpressionSyntax interpolatedString,
            CancellationToken cancellationToken)
        {
            StringBuilder sb = StringBuilderCache.GetInstance();

            var b = new SyntaxNodeTextBuilder(interpolatedString, sb);

            var arguments = new List<ArgumentSyntax>() { null };

            if (interpolatedString.IsVerbatim())
                b.Append("@");

            b.Append("\"");

            int index = 0;

            foreach (InterpolatedStringContentSyntax content in interpolatedString.Contents)
            {
                switch (content.Kind())
                {
                    case SyntaxKind.Interpolation:
                        {
                            var interpolation = (InterpolationSyntax)content;

                            b.Append("{");
                            b.Append(index.ToString(CultureInfo.InvariantCulture));
                            index++;

                            InterpolationAlignmentClauseSyntax alignmentClause = interpolation.AlignmentClause;
                            if (alignmentClause != null)
                            {
                                b.Append(",");
                                b.AppendSpan(alignmentClause.Value);
                            }

                            InterpolationFormatClauseSyntax formatClause = interpolation.FormatClause;
                            if (formatClause != null)
                            {
                                b.Append(":");
                                b.AppendSpan(formatClause.FormatStringToken);
                            }

                            b.Append("}");

                            arguments.Add(Argument(interpolation.Expression));
                            break;
                        }
                    case SyntaxKind.InterpolatedStringText:
                        {
                            b.AppendSpan(content);
                            break;
                        }
                }
            }

            b.Append("\"");

            arguments[0] = Argument(ParseExpression(StringBuilderCache.GetStringAndFree(sb)));

            InvocationExpressionSyntax invocation = SimpleMemberInvocationExpression(
                CSharpTypeFactory.StringType(),
                IdentifierName("Format"),
                ArgumentList(SeparatedList(arguments)));

            invocation = invocation.WithTriviaFrom(interpolatedString).WithFormatterAnnotation();

            return document.ReplaceNodeAsync(interpolatedString, invocation, cancellationToken);
        }
    }
}
