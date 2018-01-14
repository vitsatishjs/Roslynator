// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Syntax
{
    internal struct ModifiersInfo
    {
        private static ModifiersInfo Default { get; } = new ModifiersInfo();

        private ModifiersInfo(SyntaxNode node, SyntaxTokenList modifiers)
        {
            Node = node;
            Modifiers = modifiers;
            ModifierFlags = GetModifierFlags(modifiers);
        }

        public SyntaxNode Node { get; }

        public SyntaxTokenList Modifiers { get; }

        internal ModifierFlags ModifierFlags { get; }

        public bool HasNew => ModifierFlags.ContainsAny(ModifierFlags.New);

        public bool HasPublic => ModifierFlags.ContainsAny(ModifierFlags.Public);

        public bool HasPrivate => ModifierFlags.ContainsAny(ModifierFlags.Private);

        public bool HasProtected => ModifierFlags.ContainsAny(ModifierFlags.Protected);

        public bool HasInternal => ModifierFlags.ContainsAny(ModifierFlags.Internal);

        public bool HasConst => ModifierFlags.ContainsAny(ModifierFlags.Const);

        public bool HasStatic => ModifierFlags.ContainsAny(ModifierFlags.Static);

        public bool HasVirtual => ModifierFlags.ContainsAny(ModifierFlags.Virtual);

        public bool HasSealed => ModifierFlags.ContainsAny(ModifierFlags.Sealed);

        public bool HasOverride => ModifierFlags.ContainsAny(ModifierFlags.Override);

        public bool HasAbstract => ModifierFlags.ContainsAny(ModifierFlags.Abstract);

        public bool HasReadOnly => ModifierFlags.ContainsAny(ModifierFlags.ReadOnly);

        public bool HasExtern => ModifierFlags.ContainsAny(ModifierFlags.Extern);

        public bool HasUnsafe => ModifierFlags.ContainsAny(ModifierFlags.Unsafe);

        public bool HasVolatile => ModifierFlags.ContainsAny(ModifierFlags.Volatile);

        public bool HasAsync => ModifierFlags.ContainsAny(ModifierFlags.Async);

        public bool HasPartial => ModifierFlags.ContainsAny(ModifierFlags.Partial);

        public bool HasRef => ModifierFlags.ContainsAny(ModifierFlags.Ref);

        public bool HasOut => ModifierFlags.ContainsAny(ModifierFlags.Out);

        public bool HasIn => ModifierFlags.ContainsAny(ModifierFlags.In);

        public bool HasParams => ModifierFlags.ContainsAny(ModifierFlags.Params);

        public bool HasAbstractOrVirtualOrOverride
        {
            get { return ModifierFlags.ContainsAny(ModifierFlags.AbstractVirtualOverride); }
        }

        public Accessibility Accessibility
        {
            get
            {
                if (HasPublic)
                    return Accessibility.Public;

                if (HasInternal)
                    return (HasProtected) ? Accessibility.ProtectedOrInternal : Accessibility.Internal;

                if (HasProtected)
                    return (HasPrivate) ? Accessibility.ProtectedAndInternal : Accessibility.Protected;

                if (HasPrivate)
                    return Accessibility.Private;

                return Accessibility.NotApplicable;
            }
        }

        public bool Success
        {
            get { return Node != null; }
        }

        internal static ModifiersInfo Create(SyntaxNode node)
        {
            if (node == null)
                return Default;

            return new ModifiersInfo(node, node.GetModifiers());
        }

        internal static ModifiersInfo Create(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                return Default;

            return new ModifiersInfo(methodDeclaration, methodDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                return Default;

            return new ModifiersInfo(propertyDeclaration, propertyDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                return Default;

            return new ModifiersInfo(indexerDeclaration, indexerDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                return Default;

            return new ModifiersInfo(eventDeclaration, eventDeclaration.Modifiers);
        }

        internal static ModifiersInfo Create(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                return Default;

            return new ModifiersInfo(eventFieldDeclaration, eventFieldDeclaration.Modifiers);
        }

        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }

        private static ModifierFlags GetModifierFlags(SyntaxTokenList modifiers)
        {
            var flags = ModifierFlags.None;

            for (int i = 0; i < modifiers.Count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            flags |= ModifierFlags.Public;
                            break;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            flags |= ModifierFlags.Private;
                            break;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            flags |= ModifierFlags.Internal;
                            break;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            flags |= ModifierFlags.Protected;
                            break;
                        }
                    case SyntaxKind.StaticKeyword:
                        {
                            flags |= ModifierFlags.Static;
                            break;
                        }
                    case SyntaxKind.ReadOnlyKeyword:
                        {
                            flags |= ModifierFlags.ReadOnly;
                            break;
                        }
                    case SyntaxKind.SealedKeyword:
                        {
                            flags |= ModifierFlags.Sealed;
                            break;
                        }
                    case SyntaxKind.ConstKeyword:
                        {
                            flags |= ModifierFlags.Const;
                            break;
                        }
                    case SyntaxKind.VolatileKeyword:
                        {
                            flags |= ModifierFlags.Volatile;
                            break;
                        }
                    case SyntaxKind.NewKeyword:
                        {
                            flags |= ModifierFlags.New;
                            break;
                        }
                    case SyntaxKind.OverrideKeyword:
                        {
                            flags |= ModifierFlags.Override;
                            break;
                        }
                    case SyntaxKind.AbstractKeyword:
                        {
                            flags |= ModifierFlags.Abstract;
                            break;
                        }
                    case SyntaxKind.VirtualKeyword:
                        {
                            flags |= ModifierFlags.Virtual;
                            break;
                        }
                    case SyntaxKind.RefKeyword:
                        {
                            flags |= ModifierFlags.Ref;
                            break;
                        }
                    case SyntaxKind.OutKeyword:
                        {
                            flags |= ModifierFlags.Out;
                            break;
                        }
                    case SyntaxKind.InKeyword:
                        {
                            flags |= ModifierFlags.In;
                            break;
                        }
                    case SyntaxKind.ParamsKeyword:
                        {
                            flags |= ModifierFlags.Params;
                            break;
                        }
                    case SyntaxKind.UnsafeKeyword:
                        {
                            flags |= ModifierFlags.Unsafe;
                            break;
                        }
                    case SyntaxKind.PartialKeyword:
                        {
                            flags |= ModifierFlags.Partial;
                            break;
                        }
                    case SyntaxKind.AsyncKeyword:
                        {
                            flags |= ModifierFlags.Async;
                            break;
                        }
                    default:
                        {
                            Debug.Fail(modifiers[i].Kind().ToString());
                            break;
                        }
                }
            }

            return flags;
        }
    }
}
