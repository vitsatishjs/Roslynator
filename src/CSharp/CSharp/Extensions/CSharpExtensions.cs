﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Helpers;

namespace Roslynator.CSharp
{
    /// <summary>
    /// A set of extension methods for a <see cref="SemanticModel"/>.
    /// </summary>
    public static class CSharpExtensions
    {
        /// <summary>
        /// Returns a method symbol for the specified local function syntax.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="localFunction"></param>
        /// <param name="cancellationToken"></param>
        internal static IMethodSymbol GetDeclaredSymbol(
            this SemanticModel semanticModel,
            LocalFunctionStatementSyntax localFunction,
            CancellationToken cancellationToken = default)
        {
            return (IMethodSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, localFunction, cancellationToken);
        }

        /// <summary>
        /// Returns what symbol, if any, the specified attribute syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="attribute"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            AttributeSyntax attribute,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, attribute, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified constructor initializer syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="constructorInitializer"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            ConstructorInitializerSyntax constructorInitializer,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, constructorInitializer, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified cref syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="cref"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            CrefSyntax cref,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, cref, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified expression syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, expression, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified ordering syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="ordering"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            OrderingSyntax ordering,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, ordering, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns what symbol, if any, the specified select or group clause bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="selectOrGroupClause"></param>
        /// <param name="cancellationToken"></param>
        public static ISymbol GetSymbol(
            this SemanticModel semanticModel,
            SelectOrGroupClauseSyntax selectOrGroupClause,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetSymbolInfo(semanticModel, selectOrGroupClause, cancellationToken)
                .Symbol;
        }

        /// <summary>
        /// Returns type information about an attribute syntax.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="attribute"></param>
        /// <param name="cancellationToken"></param>
        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            AttributeSyntax attribute,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, attribute, cancellationToken)
                .Type;
        }

        /// <summary>
        /// Returns type information about a constructor initializer syntax.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="constructorInitializer"></param>
        /// <param name="cancellationToken"></param>
        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ConstructorInitializerSyntax constructorInitializer,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, constructorInitializer, cancellationToken)
                .Type;
        }

        /// <summary>
        /// Returns type information about an expression syntax.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, expression, cancellationToken)
                .Type;
        }

        /// <summary>
        /// Returns type information about a select or group clause.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="selectOrGroupClause"></param>
        /// <param name="cancellationToken"></param>
        public static ITypeSymbol GetTypeSymbol(
            this SemanticModel semanticModel,
            SelectOrGroupClauseSyntax selectOrGroupClause,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
                .GetTypeInfo(semanticModel, selectOrGroupClause, cancellationToken)
                .Type;
        }

        internal static bool IsExplicitConversion(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ITypeSymbol destinationType,
            bool isExplicitInSource = false)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType.Kind == SymbolKind.ErrorType)
                return false;

            if (destinationType.SpecialType == SpecialType.System_Void)
                return false;

            Conversion conversion = semanticModel.ClassifyConversion(
                expression,
                destinationType,
                isExplicitInSource);

            return conversion.IsExplicit;
        }

        internal static bool IsImplicitConversion(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            ITypeSymbol destinationType,
            bool isExplicitInSource = false)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (destinationType == null)
                throw new ArgumentNullException(nameof(destinationType));

            if (destinationType.Kind == SymbolKind.ErrorType)
                return false;

            if (destinationType.SpecialType == SpecialType.System_Void)
                return false;

            Conversion conversion = semanticModel.ClassifyConversion(
                expression,
                destinationType,
                isExplicitInSource);

            return conversion.IsImplicit;
        }

        /// <summary>
        /// Determines a parameter symbol that matches to the specified argument.
        /// Returns null if no matching parameter is found.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="argument"></param>
        /// <param name="allowParams"></param>
        /// <param name="allowCandidate"></param>
        /// <param name="cancellationToken"></param>
        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            ArgumentSyntax argument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (argument == null)
                throw new ArgumentNullException(nameof(argument));

            return DetermineParameterHelper.DetermineParameter(argument, semanticModel, allowParams, allowCandidate, cancellationToken);
        }

        /// <summary>
        /// Determines a parameter symbol that matches to the specified attribute argument.
        /// Returns null if not matching parameter is found.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="attributeArgument"></param>
        /// <param name="allowParams"></param>
        /// <param name="allowCandidate"></param>
        /// <param name="cancellationToken"></param>
        public static IParameterSymbol DetermineParameter(
            this SemanticModel semanticModel,
            AttributeArgumentSyntax attributeArgument,
            bool allowParams = false,
            bool allowCandidate = false,
            CancellationToken cancellationToken = default)
        {
            if (attributeArgument == null)
                throw new ArgumentNullException(nameof(attributeArgument));

            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            return DetermineParameterHelper.DetermineParameter(attributeArgument, semanticModel, allowParams, allowCandidate, cancellationToken);
        }

        /// <summary>
        /// Returns true if the specified expression represents default value of the specified type.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="typeSymbol"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static bool IsDefaultValue(
            this SemanticModel semanticModel,
            ITypeSymbol typeSymbol,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            if (semanticModel == null)
                throw new ArgumentNullException(nameof(semanticModel));

            if (typeSymbol == null)
                throw new ArgumentNullException(nameof(typeSymbol));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (typeSymbol.Kind == SymbolKind.ErrorType)
                return false;

            switch (expression.WalkDownParentheses().Kind())
            {
                case SyntaxKind.NullLiteralExpression:
                    {
                        return typeSymbol.IsReferenceTypeOrNullableType();
                    }
                case SyntaxKind.DefaultLiteralExpression:
                    {
                        return true;
                    }
                case SyntaxKind.DefaultExpression:
                    {
                        var defaultExpression = (DefaultExpressionSyntax)expression;

                        TypeSyntax type = defaultExpression.Type;

                        return type != null
                            && SymbolEqualityComparer.Default.Equals(typeSymbol, semanticModel.GetTypeSymbol(type, cancellationToken));
                    }
            }

            switch (typeSymbol.SpecialType)
            {
                case SpecialType.System_Void:
                    {
                        return false;
                    }
                case SpecialType.System_Boolean:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is bool value
                            && !value;
                    }
                case SpecialType.System_Char:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is char value
                            && value == '\0';
                    }
                case SpecialType.System_SByte:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is sbyte value
                            && value == 0;
                    }
                case SpecialType.System_Byte:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is byte value
                            && value == 0;
                    }
                case SpecialType.System_Int16:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is short value
                            && value == 0;
                    }
                case SpecialType.System_UInt16:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is ushort value
                            && value == 0;
                    }
                case SpecialType.System_Int32:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is int value
                            && value == 0;
                    }
                case SpecialType.System_UInt32:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is uint value
                            && value == 0;
                    }
                case SpecialType.System_Int64:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is long value
                            && value == 0;
                    }
                case SpecialType.System_UInt64:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is ulong value
                            && value == 0;
                    }
                case SpecialType.System_Decimal:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is decimal value
                            && value == 0;
                    }
                case SpecialType.System_Single:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is float value
                            && value == 0;
                    }
                case SpecialType.System_Double:
                    {
                        Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                        return optional.HasValue
                            && optional.Value is double value
                            && value == 0;
                    }
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                var enumSymbol = (INamedTypeSymbol)typeSymbol;

                switch (enumSymbol.EnumUnderlyingType.SpecialType)
                {
                    case SpecialType.System_SByte:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is sbyte value
                                && value == 0;
                        }
                    case SpecialType.System_Byte:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is byte value
                                && value == 0;
                        }
                    case SpecialType.System_Int16:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is short value
                                && value == 0;
                        }
                    case SpecialType.System_UInt16:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is ushort value
                                && value == 0;
                        }
                    case SpecialType.System_Int32:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is int value
                                && value == 0;
                        }
                    case SpecialType.System_UInt32:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is uint value
                                && value == 0;
                        }
                    case SpecialType.System_Int64:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is long value
                                && value == 0;
                        }
                    case SpecialType.System_UInt64:
                        {
                            Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                            return optional.HasValue
                                && optional.Value is ulong value
                                && value == 0;
                        }
                }

                Debug.Fail(enumSymbol.EnumUnderlyingType.SpecialType.ToString());

                return false;
            }

            if (typeSymbol.IsReferenceTypeOrNullableType())
            {
                Optional<object> optional = semanticModel.GetConstantValue(expression, cancellationToken);

                if (optional.HasValue)
                    return optional.Value == null;
            }

            return false;
        }

        /// <summary>
        /// Returns what extension method symbol, if any, the specified expression syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static ExtensionMethodSymbolInfo GetExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            if (GetSymbol(semanticModel, expression, cancellationToken) is IMethodSymbol methodSymbol
                && methodSymbol.IsExtensionMethod)
            {
                IMethodSymbol reducedFrom = methodSymbol.ReducedFrom;

                if (reducedFrom != null)
                    return new ExtensionMethodSymbolInfo(reducedFrom, methodSymbol);

                return new ExtensionMethodSymbolInfo(methodSymbol, null);
            }

            return default;
        }

        /// <summary>
        /// Returns what extension method symbol, if any, the specified expression syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static ExtensionMethodSymbolInfo GetReducedExtensionMethodInfo(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            if (GetSymbol(semanticModel, expression, cancellationToken) is IMethodSymbol methodSymbol
                && methodSymbol.IsExtensionMethod)
            {
                IMethodSymbol reducedFrom = methodSymbol.ReducedFrom;

                if (reducedFrom != null)
                    return new ExtensionMethodSymbolInfo(reducedFrom, methodSymbol);
            }

            return default;
        }

        /// <summary>
        /// Returns method symbol, if any, the specified expression syntax bound to.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static IMethodSymbol GetMethodSymbol(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            return GetSymbol(semanticModel, expression, cancellationToken) as IMethodSymbol;
        }

        internal static MethodDeclarationSyntax GetOtherPart(
            this SemanticModel semanticModel,
            MethodDeclarationSyntax methodDeclaration,
            CancellationToken cancellationToken = default)
        {
            IMethodSymbol methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, cancellationToken);

            IMethodSymbol otherSymbol = methodSymbol.PartialDefinitionPart ?? methodSymbol.PartialImplementationPart;

            if (otherSymbol != null)
                return (MethodDeclarationSyntax)otherSymbol.GetSyntax(cancellationToken);

            return null;
        }

        /// <summary>
        /// Returns true if the specified node has a constant value.
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        public static bool HasConstantValue(
            this SemanticModel semanticModel,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            return Microsoft.CodeAnalysis.CSharp.CSharpExtensions.GetConstantValue(semanticModel, expression, cancellationToken).HasValue;
        }
    }
}
