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
    /// Provides information about a list of statements.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public readonly struct StatementListInfo : IReadOnlyList<StatementSyntax>
    {
        internal StatementListInfo(BlockSyntax block)
        {
            Debug.Assert(block != null);

            Parent = block;
            Statements = block.Statements;
        }

        internal StatementListInfo(SwitchSectionSyntax switchSection)
        {
            Debug.Assert(switchSection != null);

            Parent = switchSection;
            Statements = switchSection.Statements;
        }

        /// <summary>
        /// The node that contains the statements. It can be either a <see cref="BlockSyntax"/> or a <see cref="SwitchSectionSyntax"/>.
        /// </summary>
        public SyntaxNode Parent { get; }

        /// <summary>
        /// The list of statements.
        /// </summary>
        public SyntaxList<StatementSyntax> Statements { get; }

        /// <summary>
        /// Determines whether the statements are contained in a <see cref="BlockSyntax"/>.
        /// </summary>
        public bool IsParentBlock
        {
            get { return Parent?.Kind() == SyntaxKind.Block; }
        }

        /// <summary>
        /// Determines whether the statements are contained in a <see cref="SwitchSectionSyntax"/>.
        /// </summary>
        public bool IsParentSwitchSection
        {
            get { return Parent?.Kind() == SyntaxKind.SwitchSection; }
        }

        /// <summary>
        /// Gets a block that contains the statements. Returns null if the statements are not contained in a block.
        /// </summary>
        public BlockSyntax ParentAsBlock
        {
            get
            {
                SyntaxNode parent = Parent;
                return (parent?.Kind() == SyntaxKind.Block) ? (BlockSyntax)parent : null;
            }
        }

        /// <summary>
        /// Gets a switch section that contains the statements. Returns null if the statements are not contained in a switch section.
        /// </summary>
        public SwitchSectionSyntax ParentAsSwitchSection
        {
            get
            {
                SyntaxNode parent = Parent;
                return (parent?.Kind() == SyntaxKind.SwitchSection) ? (SwitchSectionSyntax)parent : null;
            }
        }

        /// <summary>
        /// Determines whether this struct was initialized with an actual syntax.
        /// </summary>
        public bool Success
        {
            get { return Parent != null; }
        }

        /// <summary>
        /// The number of statement in the list.
        /// </summary>
        public int Count
        {
            get { return Statements.Count; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get { return SyntaxInfoHelpers.ToDebugString(Success, Statements); }
        }

        /// <summary>
        /// Gets the statement at the specified index in the list.
        /// </summary>
        /// <param name="index">The zero-based index of the statement to get. </param>
        /// <returns>The statement at the specified index in the list.</returns>
        public StatementSyntax this[int index]
        {
            get { return Statements[index]; }
        }

        IEnumerator<StatementSyntax> IEnumerable<StatementSyntax>.GetEnumerator()
        {
            return ((IReadOnlyList<StatementSyntax>)Statements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<StatementSyntax>)Statements).GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator the list of statements.
        /// </summary>
        public SyntaxList<StatementSyntax>.Enumerator GetEnumerator()
        {
            return Statements.GetEnumerator();
        }

        internal static StatementListInfo Create(StatementSyntax statementInList)
        {
            if (statementInList == null)
                return default;

            SyntaxNode parent = statementInList.Parent;

            switch (parent?.Kind())
            {
                case SyntaxKind.Block:
                    return new StatementListInfo((BlockSyntax)parent);
                case SyntaxKind.SwitchSection:
                    return new StatementListInfo((SwitchSectionSyntax)parent);
                default:
                    return default;
            }
        }

        internal static StatementListInfo Create(StatementListSelection selectedStatements)
        {
            return Create(selectedStatements?.First());
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the statements updated.
        /// </summary>
        /// <param name="statements"></param>
        public StatementListInfo WithStatements(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(List(statements));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the statements updated.
        /// </summary>
        /// <param name="statements"></param>
        public StatementListInfo WithStatements(SyntaxList<StatementSyntax> statements)
        {
            ThrowInvalidOperationIfNotInitialized();

            SyntaxNode parent = Parent;

            if (parent.Kind() == SyntaxKind.Block)
                return new StatementListInfo(((BlockSyntax)parent).WithStatements(statements));

            return new StatementListInfo(((SwitchSectionSyntax)parent).WithStatements(statements));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified node removed.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="options"></param>
        public StatementListInfo RemoveNode(SyntaxNode node, SyntaxRemoveOptions options)
        {
            ThrowInvalidOperationIfNotInitialized();

            SyntaxNode parent = Parent;

            if (parent.Kind() == SyntaxKind.Block)
                return new StatementListInfo(((BlockSyntax)parent).RemoveNode(node, options));

            return new StatementListInfo(((SwitchSectionSyntax)parent).RemoveNode(node, options));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified old node replaced with a new node.
        /// </summary>
        /// <param name="oldNode"></param>
        /// <param name="newNode"></param>
        public StatementListInfo ReplaceNode(SyntaxNode oldNode, SyntaxNode newNode)
        {
            ThrowInvalidOperationIfNotInitialized();

            SyntaxNode parent = Parent;

            if (parent.Kind() == SyntaxKind.Block)
                return new StatementListInfo(((BlockSyntax)parent).ReplaceNode(oldNode, newNode));

            return new StatementListInfo(((SwitchSectionSyntax)parent).ReplaceNode(oldNode, newNode));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statement added at the end.
        /// </summary>
        /// <param name="statement"></param>
        public StatementListInfo Add(StatementSyntax statement)
        {
            return WithStatements(Statements.Add(statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statements added at the end.
        /// </summary>
        /// <param name="statements"></param>
        public StatementListInfo AddRange(IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.AddRange(statements));
        }

        /// <summary>
        /// True if the list has at least one statement.
        /// </summary>
        public bool Any()
        {
            return Statements.Any();
        }

        /// <summary>
        /// The first statement in the list.
        /// </summary>
        public StatementSyntax First()
        {
            return Statements[0];
        }

        /// <summary>
        /// The first statement in the list or null if the list is empty.
        /// </summary>
        public StatementSyntax FirstOrDefault()
        {
            return Statements.FirstOrDefault();
        }

        /// <summary>
        /// Searches for a statement that matches the predicate and returns zero-based index of the first occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        public int IndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.IndexOf(predicate);
        }

        /// <summary>
        /// The index of the statement in the list.
        /// </summary>
        /// <param name="statement"></param>
        public int IndexOf(StatementSyntax statement)
        {
            return Statements.IndexOf(statement);
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statement inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statement"></param>
        public StatementListInfo Insert(int index, StatementSyntax statement)
        {
            return WithStatements(Statements.Insert(index, statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statements inserted at the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="statements"></param>
        public StatementListInfo InsertRange(int index, IEnumerable<StatementSyntax> statements)
        {
            return WithStatements(Statements.InsertRange(index, statements));
        }

        /// <summary>
        /// The last statement in the list.
        /// </summary>
        public StatementSyntax Last()
        {
            return Statements.Last();
        }

        /// <summary>
        /// The last statement in the list or null if the list is empty.
        /// </summary>
        public StatementSyntax LastOrDefault()
        {
            return Statements.LastOrDefault();
        }

        /// <summary>
        /// Searches for a statement that matches the predicate and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="predicate"></param>
        public int LastIndexOf(Func<StatementSyntax, bool> predicate)
        {
            return Statements.LastIndexOf(predicate);
        }

        /// <summary>
        /// Searches for a statement and returns zero-based index of the last occurrence in the list.
        /// </summary>
        /// <param name="statement"></param>
        public int LastIndexOf(StatementSyntax statement)
        {
            return Statements.LastIndexOf(statement);
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statement removed.
        /// </summary>
        /// <param name="statement"></param>
        public StatementListInfo Remove(StatementSyntax statement)
        {
            return WithStatements(Statements.Remove(statement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the statement at the specified index removed.
        /// </summary>
        /// <param name="index"></param>
        public StatementListInfo RemoveAt(int index)
        {
            return WithStatements(Statements.RemoveAt(index));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statement replaced with the new statement.
        /// </summary>
        /// <param name="statementInList"></param>
        /// <param name="newStatement"></param>
        public StatementListInfo Replace(StatementSyntax statementInList, StatementSyntax newStatement)
        {
            return WithStatements(Statements.Replace(statementInList, newStatement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the statement at the specified index replaced with a new statement.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newStatement"></param>
        public StatementListInfo ReplaceAt(int index, StatementSyntax newStatement)
        {
            return WithStatements(Statements.ReplaceAt(index, newStatement));
        }

        /// <summary>
        /// Creates a new <see cref="StatementListInfo"/> with the specified statement replaced with new statements.
        /// </summary>
        /// <param name="statementInList"></param>
        /// <param name="newStatements"></param>
        public StatementListInfo ReplaceRange(StatementSyntax statementInList, IEnumerable<StatementSyntax> newStatements)
        {
            return WithStatements(Statements.ReplaceRange(statementInList, newStatements));
        }

        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (Parent == null)
                throw new InvalidOperationException($"{nameof(StatementListInfo)} is not initalized.");
        }
    }
}
