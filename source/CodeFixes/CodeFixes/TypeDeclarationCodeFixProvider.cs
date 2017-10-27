// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Comparers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TypeDeclarationCodeFixProvider))]
    [Shared]
    public class TypeDeclarationCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(
                    CompilerDiagnosticIdentifiers.TypeDefinesEqualityOperatorButDoesNotOverrideObjectEquals,
                    CompilerDiagnosticIdentifiers.TypeDefinesEqualityOperatorButDoesNotOverrideObjectGetHashCode);
            }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            if (!Settings.IsAnyCodeFixEnabled(
                CodeFixIdentifiers.DefineObjectEquals,
                CodeFixIdentifiers.DefineObjectGetHashCode))
            {
                return;
            }

            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out TypeDeclarationSyntax typeDeclaration))
                return;

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                switch (diagnostic.Id)
                {
                    case CompilerDiagnosticIdentifiers.TypeDefinesEqualityOperatorButDoesNotOverrideObjectEquals:
                        {
                            if (!Settings.IsCodeFixEnabled(CodeFixIdentifiers.DefineObjectEquals))
                                break;

                            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                            MethodDeclarationSyntax methodDeclaration = MethodDeclaration(
                                Modifiers.PublicOverride(),
                                semanticModel.GetSpecialTypeSyntax(SpecialType.System_Boolean),
                                "Equals",
                                ParameterList(Parameter(semanticModel.GetSpecialTypeSyntax(SpecialType.System_Object), "obj")),
                                Block(
                                    ReturnStatement(
                                        SimpleMemberInvocationExpression(
                                            BaseExpression(),
                                            IdentifierName("Equals"),
                                            Argument(IdentifierName("obj")))))).WithSimplifierAnnotation();

                            CodeAction codeAction = CodeAction.Create(
                                "Override object.Equals",
                                cancellationToken =>
                                {
                                    TypeDeclarationSyntax newNode = typeDeclaration.InsertMember(methodDeclaration, MemberDeclarationComparer.ByKind);

                                    return context.Document.ReplaceNodeAsync(typeDeclaration, newNode, cancellationToken);
                                },
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                    case CompilerDiagnosticIdentifiers.TypeDefinesEqualityOperatorButDoesNotOverrideObjectGetHashCode:
                        {
                            if (!Settings.IsCodeFixEnabled(CodeFixIdentifiers.DefineObjectGetHashCode))
                                break;

                            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                            MethodDeclarationSyntax methodDeclaration = MethodDeclaration(
                                Modifiers.PublicOverride(),
                                semanticModel.GetSpecialTypeSyntax(SpecialType.System_Int32),
                                "GetHashCode",
                                ParameterList(),
                                Block(
                                    ReturnStatement(
                                        SimpleMemberInvocationExpression(
                                            BaseExpression(),
                                            IdentifierName("GetHashCode"),
                                            ArgumentList())))).WithSimplifierAnnotation();

                            CodeAction codeAction = CodeAction.Create(
                                "Override object.GetHashCode",
                                cancellationToken =>
                                {
                                    TypeDeclarationSyntax newNode = typeDeclaration.InsertMember(methodDeclaration, MemberDeclarationComparer.ByKind);

                                    return context.Document.ReplaceNodeAsync(typeDeclaration, newNode, cancellationToken);
                                },
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                }
            }
        }
    }
}
