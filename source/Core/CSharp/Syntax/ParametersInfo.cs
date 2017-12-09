// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Roslynator.CSharp.Syntax.SyntaxInfoHelpers;

namespace Roslynator.CSharp.Syntax
{
    internal struct ParametersInfo
    {
        public ParametersInfo(TypeParameterListSyntax typeParameterList, ParameterSyntax parameter, CSharpSyntaxNode body)
        {
            TypeParameterList = typeParameterList;
            Body = body;
            Parameter = parameter;
            ParameterList = null;
        }

        public ParametersInfo(TypeParameterListSyntax typeParameterList, BaseParameterListSyntax parameterList, CSharpSyntaxNode body)
        {
            TypeParameterList = typeParameterList;
            Body = body;
            Parameter = null;
            ParameterList = parameterList;
        }

        private static ParametersInfo Default { get; } = new ParametersInfo();

        public SyntaxNode Node
        {
            get { return ParameterList?.Parent ?? Parameter?.Parent; }
        }

        public TypeParameterListSyntax TypeParameterList { get; }

        public SeparatedSyntaxList<TypeParameterSyntax> TypeParameters
        {
            get { return TypeParameterList?.Parameters ?? default(SeparatedSyntaxList<TypeParameterSyntax>); }
        }

        public ParameterSyntax Parameter { get; }

        public BaseParameterListSyntax ParameterList { get; }

        public SeparatedSyntaxList<ParameterSyntax> Parameters
        {
            get { return ParameterList?.Parameters ?? default(SeparatedSyntaxList<ParameterSyntax>); }
        }

        public CSharpSyntaxNode Body { get; }

        public TextSpan Span
        {
            get { return ParameterList?.Span ?? Parameter?.Span ?? default(TextSpan); }
        }

        public int ParameterCount
        {
            get
            {
                if (ParameterList != null)
                    return Parameters.Count;

                if (Parameter != null)
                    return 1;

                return 0;
            }
        }

        public bool Success
        {
            get { return Node != null; }
        }

        public static ParametersInfo Create(ConstructorDeclarationSyntax constructorDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = constructorDeclaration.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return Default;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return Default;
            }

            CSharpSyntaxNode body = constructorDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(default(TypeParameterListSyntax), parameterList, body);
        }

        public static ParametersInfo Create(MethodDeclarationSyntax methodDeclaration, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = methodDeclaration.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return Default;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return Default;
            }

            CSharpSyntaxNode body = methodDeclaration.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            TypeParameterListSyntax typeParameterList = methodDeclaration.TypeParameterList;

            if (typeParameterList != null)
            {
                foreach (TypeParameterSyntax typeParameter in typeParameterList.Parameters)
                {
                    if (!Check(typeParameter, allowMissing))
                        return Default;
                }
            }

            return new ParametersInfo(typeParameterList, parameterList, body);
        }

        public static ParametersInfo Create(LocalFunctionStatementSyntax localFunction, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = localFunction.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return Default;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return Default;
            }

            CSharpSyntaxNode body = localFunction.BodyOrExpressionBody();

            if (!Check(body, allowMissing))
                return Default;

            TypeParameterListSyntax typeParameterList = localFunction.TypeParameterList;

            if (typeParameterList != null)
            {
                foreach (TypeParameterSyntax typeParameter in typeParameterList.Parameters)
                {
                    if (!Check(typeParameter, allowMissing))
                        return Default;
                }
            }

            return new ParametersInfo(typeParameterList, parameterList, body);
        }

        public static ParametersInfo Create(IndexerDeclarationSyntax indexerDeclaration, bool allowMissing = false)
        {
            BaseParameterListSyntax parameterList = indexerDeclaration.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return Default;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return Default;
            }

            CSharpSyntaxNode body = indexerDeclaration.AccessorList ?? (CSharpSyntaxNode)indexerDeclaration.ExpressionBody;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(null, parameterList, body);
        }

        public static ParametersInfo Create(SimpleLambdaExpressionSyntax simpleLambda, bool allowMissing = false)
        {
            ParameterSyntax parameter = simpleLambda.Parameter;

            if (!Check(parameter, allowMissing))
                return Default;

            CSharpSyntaxNode body = simpleLambda.Body;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(null, parameter, body);
        }

        public static ParametersInfo Create(ParenthesizedLambdaExpressionSyntax parenthesizedLambda, bool allowMissing = false)
        {
            ParameterListSyntax parameterList = parenthesizedLambda.ParameterList;

            if (!Check(parameterList, allowMissing))
                return Default;

            SeparatedSyntaxList<ParameterSyntax> parameters = parameterList.Parameters;

            if (!parameters.Any())
                return Default;

            foreach (ParameterSyntax parameter in parameters)
            {
                if (!Check(parameter, allowMissing))
                    return Default;
            }

            CSharpSyntaxNode body = parenthesizedLambda.Body;

            if (!Check(body, allowMissing))
                return Default;

            return new ParametersInfo(null, parameterList, body);
        }

        public override string ToString()
        {
            return Node?.ToString() ?? base.ToString();
        }
    }
}
