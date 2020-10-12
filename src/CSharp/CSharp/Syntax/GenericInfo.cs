﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about generic syntax (class, struct, interface, delegate, method or local function).
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public readonly struct GenericInfo
    {
        private GenericInfo(TypeDeclarationSyntax typeDeclaration)
            : this(typeDeclaration, typeDeclaration.TypeParameterList, typeDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(DelegateDeclarationSyntax delegateDeclaration)
            : this(delegateDeclaration, delegateDeclaration.TypeParameterList, delegateDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(LocalFunctionStatementSyntax localFunctionStatement)
            : this(localFunctionStatement, localFunctionStatement.TypeParameterList, localFunctionStatement.ConstraintClauses)
        {
        }

        private GenericInfo(MethodDeclarationSyntax methodDeclaration)
            : this(methodDeclaration, methodDeclaration.TypeParameterList, methodDeclaration.ConstraintClauses)
        {
        }

        private GenericInfo(
            SyntaxNode declaration,
            TypeParameterListSyntax typeParameterList,
            SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            Node = declaration;
            TypeParameterList = typeParameterList;
            ConstraintClauses = constraintClauses;
        }

        /// <summary>
        /// The syntax node that can be generic (for example <see cref="ClassDeclarationSyntax"/> for a class or <see cref="LocalDeclarationStatementSyntax"/> for a local function).
        /// </summary>
        public SyntaxNode Node { get; }

        /// <summary>
        /// The kind of this syntax node.
        /// </summary>
        public SyntaxKind Kind
        {
            get { return Node?.Kind() ?? SyntaxKind.None; }
        }

        /// <summary>
        /// The type parameter list.
        /// </summary>
        public TypeParameterListSyntax TypeParameterList { get; }

        /// <summary>
        /// A list of type parameters.
        /// </summary>
        public SeparatedSyntaxList<TypeParameterSyntax> TypeParameters
        {
            get { return TypeParameterList?.Parameters ?? default; }
        }

        /// <summary>
        /// A list of constraint clauses.
        /// </summary>
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }

        /// <summary>
        /// Searches for a type parameter with the specified name and returns the first occurrence within the type parameters.
        /// </summary>
        /// <param name="name"></param>
        public TypeParameterSyntax FindTypeParameter(string name)
        {
            foreach (TypeParameterSyntax typeParameter in TypeParameters)
            {
                if (string.Equals(name, typeParameter.Identifier.ValueText, StringComparison.Ordinal))
                    return typeParameter;
            }

            return null;
        }

        /// <summary>
        /// Searches for a constraint clause with the specified type parameter name and returns the first occurrence within the constraint clauses.
        /// </summary>
        /// <param name="typeParameterName"></param>
        public TypeParameterConstraintClauseSyntax FindConstraintClause(string typeParameterName)
        {
            foreach (TypeParameterConstraintClauseSyntax constraintClause in ConstraintClauses)
            {
                if (string.Equals(typeParameterName, constraintClause.Name.Identifier.ValueText, StringComparison.Ordinal))
                    return constraintClause;
            }

            return null;
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Node != null; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return (Success)
                    ? $"{GetType().Name} {Node.Kind()} {TypeParameterList} {ConstraintClauses}"
                    : "Uninitialized";
            }
        }

        internal static GenericInfo Create(SyntaxNode node)
        {
            if (node == null)
                return default;

            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.StructDeclaration:
                    {
                        return new GenericInfo((TypeDeclarationSyntax)node);
                    }
                case SyntaxKind.DelegateDeclaration:
                    {
                        return new GenericInfo((DelegateDeclarationSyntax)node);
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        return new GenericInfo((LocalFunctionStatementSyntax)node);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        return new GenericInfo((MethodDeclarationSyntax)node);
                    }
                case SyntaxKind.TypeParameterList:
                    {
                        return Create((TypeParameterListSyntax)node);
                    }
                case SyntaxKind.TypeParameter:
                    {
                        return Create((TypeParameterSyntax)node);
                    }
                case SyntaxKind.TypeParameterConstraintClause:
                    {
                        return Create((TypeParameterConstraintClauseSyntax)node);
                    }
            }

            if (node is TypeParameterConstraintSyntax typeParameterConstraint)
                return Create(typeParameterConstraint);

            return default;
        }

        internal static GenericInfo Create(TypeParameterConstraintSyntax typeParameterConstraint)
        {
            return Create(typeParameterConstraint?.Parent as TypeParameterConstraintClauseSyntax);
        }

        internal static GenericInfo Create(TypeParameterConstraintClauseSyntax constraintClause)
        {
            return Create(constraintClause?.Parent);
        }

        internal static GenericInfo Create(TypeParameterSyntax typeParameter)
        {
            return Create(typeParameter?.Parent as TypeParameterListSyntax);
        }

        internal static GenericInfo Create(TypeParameterListSyntax typeParameterList)
        {
            return Create(typeParameterList?.Parent);
        }

        internal static GenericInfo Create(TypeDeclarationSyntax typeDeclaration)
        {
            if (typeDeclaration == null)
                return default;

            return new GenericInfo(typeDeclaration);
        }

        internal static GenericInfo Create(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                return default;

            return new GenericInfo(delegateDeclaration);
        }

        internal static GenericInfo Create(LocalFunctionStatementSyntax localFunctionStatement)
        {
            if (localFunctionStatement == null)
                return default;

            return new GenericInfo(localFunctionStatement);
        }

        internal static GenericInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return default;

            return new GenericInfo(methodDeclaration);
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the type parameter list updated.
        /// </summary>
        /// <param name="typeParameterList"></param>
        public GenericInfo WithTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Node).WithTypeParameterList(typeParameterList));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Node).WithTypeParameterList(typeParameterList));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Node).WithTypeParameterList(typeParameterList));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Node).WithTypeParameterList(typeParameterList));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Node).WithTypeParameterList(typeParameterList));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Node).WithTypeParameterList(typeParameterList));
            }

            Debug.Fail(Node.Kind().ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the specified type parameter removed.
        /// </summary>
        /// <param name="typeParameter"></param>
        public GenericInfo RemoveTypeParameter(TypeParameterSyntax typeParameter)
        {
            ThrowInvalidOperationIfNotInitialized();

            var self = this;

            switch (self.Node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)self.Node).WithTypeParameterList(RemoveTypeParameter()));
            }

            Debug.Fail(self.Node.Kind().ToString());
            return this;

            TypeParameterListSyntax RemoveTypeParameter()
            {
                SeparatedSyntaxList<TypeParameterSyntax> parameters = self.TypeParameters;

                return (parameters.Count == 1)
                    ? default
                    : self.TypeParameterList.WithParameters(parameters.Remove(typeParameter));
            }
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the constraint clauses updated.
        /// </summary>
        /// <param name="constraintClauses"></param>
        public GenericInfo WithConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Node).WithConstraintClauses(constraintClauses));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Node).WithConstraintClauses(constraintClauses));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Node).WithConstraintClauses(constraintClauses));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Node).WithConstraintClauses(constraintClauses));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Node).WithConstraintClauses(constraintClauses));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Node).WithConstraintClauses(constraintClauses));
            }

            Debug.Fail(Node.Kind().ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with the specified constraint clause removed.
        /// </summary>
        /// <param name="constraintClause"></param>
        public GenericInfo RemoveConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                    return new GenericInfo(((ClassDeclarationSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.DelegateDeclaration:
                    return new GenericInfo(((DelegateDeclarationSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.InterfaceDeclaration:
                    return new GenericInfo(((InterfaceDeclarationSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.LocalFunctionStatement:
                    return new GenericInfo(((LocalFunctionStatementSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.MethodDeclaration:
                    return new GenericInfo(((MethodDeclarationSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
                case SyntaxKind.StructDeclaration:
                    return new GenericInfo(((StructDeclarationSyntax)Node).WithConstraintClauses(ConstraintClauses.Remove(constraintClause)));
            }

            Debug.Fail(Node.Kind().ToString());
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="GenericInfo"/> with all constraint clauses removed.
        /// </summary>
        public GenericInfo RemoveAllConstraintClauses()
        {
            ThrowInvalidOperationIfNotInitialized();

            if (!ConstraintClauses.Any())
                return this;

            TypeParameterConstraintClauseSyntax first = ConstraintClauses[0];

            SyntaxToken token = first.WhereKeyword.GetPreviousToken();

            SyntaxTriviaList trivia = token.TrailingTrivia.EmptyIfWhitespace()
                .AddRange(first.GetLeadingTrivia().EmptyIfWhitespace())
                .AddRange(ConstraintClauses.Last().GetTrailingTrivia());

            return Create(Node.ReplaceToken(token, token.WithTrailingTrivia(trivia)))
                .WithConstraintClauses(default(SyntaxList<TypeParameterConstraintClauseSyntax>));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Node == null)
                throw new InvalidOperationException($"{nameof(GenericInfo)} is not initalized.");
        }
    }
}