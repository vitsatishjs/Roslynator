// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.Utilities;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class InitializeFieldFromConstructorRefactoring
    {
        public static void ComputeRefactoring(RefactoringContext context, VariableDeclaratorSyntax variableDeclarator)
        {
            if (variableDeclarator.Initializer != null)
                return;

            if (!variableDeclarator.IsParentKind(SyntaxKind.VariableDeclaration))
                return;

            if (!(variableDeclarator.Parent.Parent is FieldDeclarationSyntax fieldDeclaration))
                return;

            if (fieldDeclaration.Modifiers.ContainsAny(SyntaxKind.StaticKeyword, SyntaxKind.ConstKeyword))
                return;

            if (!(fieldDeclaration.Parent is TypeDeclarationSyntax typeDeclaration))
                return;

            if (!typeDeclaration.IsKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
                return;

            if (!typeDeclaration
                .Members
                .Any(f => f.Kind() == SyntaxKind.ConstructorDeclaration && !((ConstructorDeclarationSyntax)f).Modifiers.Contains(SyntaxKind.StaticKeyword)))
            {
                return;
            }

            context.RegisterRefactoring(
                "Initialize field from constructor",
                cancellationToken => RefactorAsync(context.Document, variableDeclarator, typeDeclaration, cancellationToken));
        }

        private static Task<Document> RefactorAsync(
            Document document,
            VariableDeclaratorSyntax variableDeclarator,
            TypeDeclarationSyntax typeDeclaration,
            CancellationToken cancellationToken)
        {
            SyntaxList<MemberDeclarationSyntax> members = typeDeclaration.Members;

            string name = variableDeclarator.Identifier.ValueText;

            string parameterName = GetUniqueParameterName(StringUtility.ToCamelCase(name.TrimStart('.')), members);

            for (int i = 0; i < members.Count; i++)
            {
                if (!(members[i] is ConstructorDeclarationSyntax constructorDeclaration))
                    continue;

                if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
                    continue;

                ParameterListSyntax parameterList = constructorDeclaration.ParameterList;

                if (parameterList == null)
                    continue;

                BlockSyntax body = constructorDeclaration.Body;

                if (body == null)
                    continue;

                SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

                ParameterSyntax parameter = Parameter(((VariableDeclarationSyntax)variableDeclarator.Parent).Type.WithoutTrivia(), parameterName);

                constructorDeclaration = constructorDeclaration.WithParameterList(
                    parameterList.WithParameters(
                        parameters.Add(parameter))).WithFormatterAnnotation();

                ConstructorInitializerSyntax initializer = constructorDeclaration.Initializer;

                if (initializer?.Kind() == SyntaxKind.ThisConstructorInitializer)
                {
                    ArgumentListSyntax argumentList = initializer.ArgumentList;

                    if (argumentList != null)
                    {
                        SeparatedSyntaxList<ArgumentSyntax> arguments = argumentList.Arguments;

                        constructorDeclaration = constructorDeclaration.WithInitializer(
                            initializer.WithArgumentList(
                                argumentList.WithArguments(
                                    arguments.Add(Argument(IdentifierName(parameterName))))).WithFormatterAnnotation());
                    }
                }

                SyntaxList<StatementSyntax> statements = body.Statements;

                constructorDeclaration = constructorDeclaration.WithBody(
                    body.WithStatements(
                        statements.Add(
                            SimpleAssignmentStatement(
                                SimpleMemberAccessExpression(ThisExpression(), IdentifierName(name)).WithSimplifierAnnotation(),
                                IdentifierName(parameterName)).WithFormatterAnnotation())));

                members = members.ReplaceAt(i, constructorDeclaration);
            }

            TypeDeclarationSyntax newNode = typeDeclaration.WithMembers(members);

            return document.ReplaceNodeAsync(typeDeclaration, newNode, cancellationToken);
        }

        private static string GetUniqueParameterName(string name, SyntaxList<MemberDeclarationSyntax> members)
        {
            HashSet<string> reservedNames = null;

            foreach (MemberDeclarationSyntax member in members)
            {
                if (!(member is ConstructorDeclarationSyntax constructorDeclaration))
                    continue;

                if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
                    continue;

                ParameterListSyntax parameterList = constructorDeclaration.ParameterList;

                if (parameterList == null)
                    continue;

                foreach (ParameterSyntax parameter in parameterList.Parameters)
                    (reservedNames ?? (reservedNames = new HashSet<string>())).Add(parameter.Identifier.ValueText);
            }

            if (reservedNames != null)
                name = NameGenerator.Default.EnsureUniqueName(name, reservedNames);

            return name;
        }
    }
}
