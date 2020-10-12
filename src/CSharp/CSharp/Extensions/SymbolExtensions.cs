﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp
{
    /// <summary>
    /// A set of static methods for <see cref="ISymbol"/> and derived types.
    /// </summary>
    public static class SymbolExtensions
    {
        #region INamedTypeSymbol
        internal static string ToDisplayString(this INamedTypeSymbol typeSymbol, SymbolDisplayFormat format, SymbolDisplayTypeDeclarationOptions typeDeclarationOptions)
        {
            return typeSymbol.ToDisplayParts(format, typeDeclarationOptions).ToDisplayString();
        }

        internal static ImmutableArray<SymbolDisplayPart> ToDisplayParts(this INamedTypeSymbol typeSymbol, SymbolDisplayFormat format, SymbolDisplayTypeDeclarationOptions typeDeclarationOptions)
        {
            if (typeDeclarationOptions == SymbolDisplayTypeDeclarationOptions.None)
                return typeSymbol.ToDisplayParts(format);

            ImmutableArray<SymbolDisplayPart> parts = typeSymbol.ToDisplayParts(format);

            ImmutableArray<SymbolDisplayPart>.Builder builder = ImmutableArray.CreateBuilder<SymbolDisplayPart>(parts.Length);

            if ((typeDeclarationOptions & SymbolDisplayTypeDeclarationOptions.IncludeAccessibility) != 0)
            {
                switch (typeSymbol.DeclaredAccessibility)
                {
                    case Accessibility.Public:
                        {
                            AddKeyword(SyntaxKind.PublicKeyword);
                            break;
                        }
                    case Accessibility.ProtectedOrInternal:
                        {
                            AddKeyword(SyntaxKind.ProtectedKeyword);
                            AddKeyword(SyntaxKind.InternalKeyword);
                            break;
                        }
                    case Accessibility.Internal:
                        {
                            AddKeyword(SyntaxKind.InternalKeyword);
                            break;
                        }
                    case Accessibility.Protected:
                        {
                            AddKeyword(SyntaxKind.ProtectedKeyword);
                            break;
                        }
                    case Accessibility.ProtectedAndInternal:
                        {
                            AddKeyword(SyntaxKind.PrivateKeyword);
                            AddKeyword(SyntaxKind.ProtectedKeyword);
                            break;
                        }
                    case Accessibility.Private:
                        {
                            AddKeyword(SyntaxKind.PrivateKeyword);
                            break;
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
            }

            if ((typeDeclarationOptions & SymbolDisplayTypeDeclarationOptions.IncludeModifiers) != 0)
            {
                if (typeSymbol.IsStatic)
                    AddKeyword(SyntaxKind.StaticKeyword);

                if (typeSymbol.IsSealed
                    && !typeSymbol.TypeKind.Is(TypeKind.Struct, TypeKind.Enum, TypeKind.Delegate))
                {
                    AddKeyword(SyntaxKind.SealedKeyword);
                }

                if (typeSymbol.IsAbstract
                    && typeSymbol.TypeKind != TypeKind.Interface)
                {
                    AddKeyword(SyntaxKind.AbstractKeyword);
                }
            }

            builder.AddRange(parts);

            return builder.ToImmutableArray();

            void AddKeyword(SyntaxKind kind)
            {
                builder.Add(SymbolDisplayPartFactory.Keyword(SyntaxFacts.GetText(kind)));
                AddSpace();
            }

            void AddSpace()
            {
                builder.Add(SymbolDisplayPartFactory.Space());
            }
        }
        #endregion INamedTypeSymbol

        #region INamespaceOrTypeSymbol
        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified namespace or type symbol.
        /// </summary>
        /// <param name="namespaceOrTypeSymbol"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToTypeSyntax(this INamespaceOrTypeSymbol namespaceOrTypeSymbol, SymbolDisplayFormat format = null)
        {
            if (namespaceOrTypeSymbol == null)
                throw new ArgumentNullException(nameof(namespaceOrTypeSymbol));

            if (namespaceOrTypeSymbol.IsType)
            {
                return ToTypeSyntax((ITypeSymbol)namespaceOrTypeSymbol, format);
            }
            else
            {
                return ToTypeSyntax((INamespaceSymbol)namespaceOrTypeSymbol, format);
            }
        }

        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified namespace or type symbol
        /// </summary>
        /// <param name="namespaceOrTypeSymbol"></param>
        /// <param name="semanticModel"></param>
        /// <param name="position"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToMinimalTypeSyntax(this INamespaceOrTypeSymbol namespaceOrTypeSymbol, SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            if (namespaceOrTypeSymbol == null)
                throw new ArgumentNullException(nameof(namespaceOrTypeSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (namespaceOrTypeSymbol.IsType)
            {
                return ToMinimalTypeSyntax((ITypeSymbol)namespaceOrTypeSymbol, semanticModel, position, format);
            }
            else
            {
                return ToMinimalTypeSyntax((INamespaceSymbol)namespaceOrTypeSymbol, semanticModel, position, format);
            }
        }
        #endregion INamespaceOrTypeSymbol

        #region INamespaceSymbol
        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified namespace symbol.
        /// </summary>
        /// <param name="namespaceSymbol"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToTypeSyntax(this INamespaceSymbol namespaceSymbol, SymbolDisplayFormat format = null)
        {
            if (namespaceSymbol == null)
                throw new ArgumentNullException(nameof(namespaceSymbol));

            ThrowIfExplicitDeclarationIsNotSupported(namespaceSymbol);

            return ParseTypeName(namespaceSymbol.ToDisplayString(format ?? SymbolDisplayFormats.FullName));
        }

        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified namespace symbol.
        /// </summary>
        /// <param name="namespaceSymbol"></param>
        /// <param name="semanticModel"></param>
        /// <param name="position"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToMinimalTypeSyntax(this INamespaceSymbol namespaceSymbol, SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            if (namespaceSymbol == null)
                throw new ArgumentNullException(nameof(namespaceSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ThrowIfExplicitDeclarationIsNotSupported(namespaceSymbol);

            return ParseTypeName(namespaceSymbol.ToMinimalDisplayString(semanticModel, position, format ?? SymbolDisplayFormats.FullName));
        }

        private static void ThrowIfExplicitDeclarationIsNotSupported(INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol.IsGlobalNamespace)
                throw new ArgumentException("Global namespace does not support explicit declaration.", nameof(namespaceSymbol));
        }
        #endregion INamespaceSymbol

        #region IParameterSymbol
        internal static ExpressionSyntax GetDefaultValueMinimalSyntax(this IParameterSymbol parameterSymbol, SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            if (parameterSymbol == null)
                throw new ArgumentNullException(nameof(parameterSymbol));

            if (!parameterSymbol.HasExplicitDefaultValue)
                throw new ArgumentException("Parameter does not specify default value.", nameof(parameterSymbol));

            object value = parameterSymbol.ExplicitDefaultValue;

            ITypeSymbol typeSymbol = parameterSymbol.Type;

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                if (value == null)
                    return NullLiteralExpression();

                IFieldSymbol fieldSymbol = FindFieldWithConstantValue();

                TypeSyntax type = typeSymbol.ToMinimalTypeSyntax(semanticModel, position, format);

                if (fieldSymbol != null)
                {
                    return SimpleMemberAccessExpression(type, IdentifierName(fieldSymbol.Name));
                }
                else
                {
                    return CastExpression(type, LiteralExpression(value));
                }
            }

            if (value == null
                && !typeSymbol.IsReferenceTypeOrNullableType())
            {
                return DefaultExpression(typeSymbol.ToMinimalTypeSyntax(semanticModel, position, format));
            }

            return LiteralExpression(value);

            IFieldSymbol FindFieldWithConstantValue()
            {
                foreach (ISymbol symbol in typeSymbol.GetMembers())
                {
                    if (symbol.Kind == SymbolKind.Field)
                    {
                        var fieldSymbol = (IFieldSymbol)symbol;

                        if (fieldSymbol.HasConstantValue
                            && object.Equals(fieldSymbol.ConstantValue, value))
                        {
                            return fieldSymbol;
                        }
                    }
                }

                return null;
            }
        }
        #endregion IParameterSymbol

        #region ITypeSymbol
        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified type symbol.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToTypeSyntax(this ITypeSymbol typeSymbol, SymbolDisplayFormat format = null)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            ThrowIfExplicitDeclarationIsNotSupported(typeSymbol);

            return ParseTypeName(typeSymbol.ToDisplayString(format ?? SymbolDisplayFormats.FullName));
        }

        /// <summary>
        /// Creates a new <see cref="TypeSyntax"/> based on the specified type symbol.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <param name="semanticModel"></param>
        /// <param name="position"></param>
        /// <param name="format"></param>
        public static TypeSyntax ToMinimalTypeSyntax(this ITypeSymbol typeSymbol, SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            ThrowIfExplicitDeclarationIsNotSupported(typeSymbol);

            return ParseTypeName(typeSymbol.ToMinimalDisplayString(semanticModel, position, format ?? SymbolDisplayFormats.FullName));
        }

        private static void ThrowIfExplicitDeclarationIsNotSupported(ITypeSymbol typeSymbol)
        {
            if (!typeSymbol.SupportsExplicitDeclaration())
                throw new ArgumentException($"Type '{typeSymbol.ToDisplayString()}' does not support explicit declaration.", nameof(typeSymbol));
        }

        /// <summary>
        /// Returns true if the specified type can be used to declare constant value.
        /// </summary>
        /// <param name="typeSymbol"></param>
        public static bool SupportsConstantValue(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                case SpecialType.System_String:
                    return true;
                default:
                    return typeSymbol.TypeKind == TypeKind.Enum;
            }
        }

        internal static bool SupportsPrefixOrPostfixUnaryOperator(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            return CSharpFacts.SupportsPrefixOrPostfixUnaryOperator(typeSymbol.SpecialType)
                || typeSymbol.TypeKind == TypeKind.Enum;
        }

        internal static bool IsReadOnlyStruct(this ITypeSymbol type)
        {
            return type.IsReadOnly
                && type.TypeKind == TypeKind.Struct;
        }
        #endregion ITypeSymbol
    }
}
