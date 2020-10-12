﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Syntax
{
    /// <summary>
    /// Provides information about a list of using directives.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public readonly struct UsingDirectiveListInfo : IReadOnlyList<UsingDirectiveSyntax>
    {
        internal UsingDirectiveListInfo(SyntaxNode parent, SyntaxList<UsingDirectiveSyntax> usings)
        {
            Parent = parent;
            Usings = usings;
        }

        /// <summary>
        /// The declaration that contains the usings.
        /// </summary>
        public SyntaxNode Parent { get; }

        /// <summary>
        /// A list of usings.
        /// </summary>
        public SyntaxList<UsingDirectiveSyntax> Usings { get; }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// A number of usings in the list.
        /// </summary>
        public int Count
        {
            get { return Usings.Count; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get { return SyntaxInfoHelpers.ToDebugString(Success, Usings); }
        }

        /// <summary>
        /// Gets the using directive at the specified index in the list.
        /// </summary>
        /// <param name="index">The zero-based index of the using directive to get. </param>
        /// <returns>The using directive at the specified index in the list.</returns>
        public UsingDirectiveSyntax this[int index]
        {
            get { return Usings[index]; }
        }

        IEnumerator<UsingDirectiveSyntax> IEnumerable<UsingDirectiveSyntax>.GetEnumerator()
        {
            return ((IReadOnlyList<UsingDirectiveSyntax>)Usings).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<UsingDirectiveSyntax>)Usings).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for the list of usings.
        /// </summary>
        public SyntaxList<UsingDirectiveSyntax>.Enumerator GetEnumerator()
        {
            return Usings.GetEnumerator();
        }

        internal static UsingDirectiveListInfo Create(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                return default;

            return new UsingDirectiveListInfo(namespaceDeclaration, namespaceDeclaration.Usings);
        }

        internal static UsingDirectiveListInfo Create(CompilationUnitSyntax compilationUnit)
        {
            if (compilationUnit == null)
                return default;

            return new UsingDirectiveListInfo(compilationUnit, compilationUnit.Usings);
        }

        internal static UsingDirectiveListInfo Create(SyntaxNode declaration)
        {
            switch (declaration?.Kind())
            {
                case SyntaxKind.CompilationUnit:
                    {
                        var typeDeclaration = (CompilationUnitSyntax)declaration;
                        return new UsingDirectiveListInfo(typeDeclaration, typeDeclaration.Usings);
                    }
                case SyntaxKind.NamespaceDeclaration:
                    {
                        var namespaceDeclaration = (NamespaceDeclarationSyntax)declaration;
                        return new UsingDirectiveListInfo(namespaceDeclaration, namespaceDeclaration.Usings);
                    }
            }

            return default;
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the usings updated.
        /// </summary>
        /// <param name="usings"></param>
        public UsingDirectiveListInfo WithUsings(IEnumerable<UsingDirectiveSyntax> usings)
        {
            return WithUsings(List(usings));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the usings updated.
        /// </summary>
        /// <param name="usings"></param>
        public UsingDirectiveListInfo WithUsings(SyntaxList<UsingDirectiveSyntax> usings)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent)
            {
                case CompilationUnitSyntax compilationUnit:
                    {
                        compilationUnit = compilationUnit.WithUsings(usings);
                        return new UsingDirectiveListInfo(compilationUnit, compilationUnit.Usings);
                    }
                case NamespaceDeclarationSyntax declaration:
                    {
                        declaration = declaration.WithUsings(usings);
                        return new UsingDirectiveListInfo(declaration, declaration.Usings);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified node removed.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="options"></param>
        public UsingDirectiveListInfo RemoveNode(SyntaxNode node, SyntaxRemoveOptions options)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent)
            {
                case CompilationUnitSyntax compilationUnit:
                    {
                        compilationUnit = compilationUnit.RemoveNode(node, options);
                        return new UsingDirectiveListInfo(compilationUnit, compilationUnit.Usings);
                    }
                case NamespaceDeclarationSyntax declaration:
                    {
                        declaration = declaration.RemoveNode(node, options);
                        return new UsingDirectiveListInfo(declaration, declaration.Usings);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified old node replaced with a new node.
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        public UsingDirectiveListInfo ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
        {
            ThrowInvalidOperationIfNotInitialized();

            switch (Parent)
            {
                case CompilationUnitSyntax compilationUnit:
                    {
                        compilationUnit = compilationUnit.ReplaceNode(oldNode, newNode);
                        return new UsingDirectiveListInfo(compilationUnit, compilationUnit.Usings);
                    }
                case NamespaceDeclarationSyntax declaration:
                    {
                        declaration = declaration.ReplaceNode(oldNode, newNode);
                        return new UsingDirectiveListInfo(declaration, declaration.Usings);
                    }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified using directive added at the end.
        /// </summary>
        /// <param name="usingDirective"></param>
        public UsingDirectiveListInfo Add(UsingDirectiveSyntax usingDirective)
        {
            return WithUsings(Usings.Add(usingDirective));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified usings added at the end.
        /// </summary>
        /// <param name="usings"></param>
        public UsingDirectiveListInfo AddRange(IEnumerable<UsingDirectiveSyntax> usings)
        {
            return WithUsings(Usings.AddRange(usings));
        }

        /// <summary>
        /// True if the list has at least one using directive.
        /// </summary>
        public bool Any()
        {
            return Usings.Any();
        }

        /// <summary>
        /// The first using directive in the list.
        /// </summary>
        public UsingDirectiveSyntax First()
        {
            return Usings[0];
        }

        /// <summary>
        /// The first using directive in the list or null if the list is empty.
        /// </summary>
        public UsingDirectiveSyntax FirstOrDefault()
        {
            return Usings.FirstOrDefault();
        }

        /// <summary>
        /// Searches for an using directive that matches the predicate and returns zero-based index of the first occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        public int IndexOf(Func<UsingDirectiveSyntax, bool> predicate)
        {
            return Usings.IndexOf(predicate);
        }

        /// <summary>
        /// The index of the using directive in the list.
        /// </summary>
        /// <param name="usingDirective"></param>
        public int IndexOf(UsingDirectiveSyntax usingDirective)
        {
            return Usings.IndexOf(usingDirective);
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified using directive inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="usingDirective"></param>
        public UsingDirectiveListInfo Insert(int index, UsingDirectiveSyntax usingDirective)
        {
            return WithUsings(Usings.Insert(index, usingDirective));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified usings inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="usings"></param>
        public UsingDirectiveListInfo InsertRange(int index, IEnumerable<UsingDirectiveSyntax> usings)
        {
            return WithUsings(Usings.InsertRange(index, usings));
        }

        /// <summary>
        /// The last using directive in the list.
        /// </summary>
        public UsingDirectiveSyntax Last()
        {
            return Usings.Last();
        }

        /// <summary>
        /// The last using directive in the list or null if the list is empty.
        /// </summary>
        public UsingDirectiveSyntax LastOrDefault()
        {
            return Usings.LastOrDefault();
        }

        /// <summary>
        /// Searches for an using directive that matches the predicate and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        public int LastIndexOf(Func<UsingDirectiveSyntax, bool> predicate)
        {
            return Usings.LastIndexOf(predicate);
        }

        /// <summary>
        /// Searches for an using directive and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="usingDirective"></param>
        public int LastIndexOf(UsingDirectiveSyntax usingDirective)
        {
            return Usings.LastIndexOf(usingDirective);
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified using directive removed.
        /// </summary>
        /// <param name="usingDirective"></param>
        public UsingDirectiveListInfo Remove(UsingDirectiveSyntax usingDirective)
        {
            return WithUsings(Usings.Remove(usingDirective));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the using directive at the specified index removed.
        /// </summary>
        /// <param name="index"></param>
        public UsingDirectiveListInfo RemoveAt(int index)
        {
            return WithUsings(Usings.RemoveAt(index));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified using directive replaced with the new using directive.
        /// </summary>
        /// <param name="usingInLine"></param>
        /// <param name="newUsingDirective"></param>
        public UsingDirectiveListInfo Replace(UsingDirectiveSyntax usingInLine, UsingDirectiveSyntax newUsingDirective)
        {
            return WithUsings(Usings.Replace(usingInLine, newUsingDirective));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the using directive at the specified index replaced with a new using directive.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newUsingDirective"></param>
        public UsingDirectiveListInfo ReplaceAt(int index, UsingDirectiveSyntax newUsingDirective)
        {
            return WithUsings(Usings.ReplaceAt(index, newUsingDirective));
        }

        /// <summary>
        /// Creates a new <see cref="UsingDirectiveListInfo"/> with the specified using directive replaced with new usings.
        /// </summary>
        /// <param name="usingInLine"></param>
        /// <param name="newUsingDirectives"></param>
        public UsingDirectiveListInfo ReplaceRange(UsingDirectiveSyntax usingInLine, IEnumerable<UsingDirectiveSyntax> newUsingDirectives)
        {
            return WithUsings(Usings.ReplaceRange(usingInLine, newUsingDirectives));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Parent == null)
                throw new InvalidOperationException($"{nameof(UsingDirectiveListInfo)} is not initalized.");
        }
    }
}
