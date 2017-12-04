// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

#pragma warning disable RCS1081, RCS1176

namespace Roslynator.CSharp.Analyzers.Tests
{
    internal static class RemoveUnusedMemberDeclaration
    {
        private class Foo
        {
            private static readonly string _f;
            private static readonly string _f2, _f3;

            private static void FooMethod()
            {
                string s = _f3;

                EventHandler eh = FooEvent3;

                FooMethod<string>();

                s.FooExtensionMethod<string>();
            }

            private static void FooMethod<T>()
            {
            }

            private static string FooProperty { get; }

            private static event EventHandler FooEvent;
            private static event EventHandler FooEvent2, FooEvent3;

            private delegate void FooDelegate();
        }

        private partial class FooPartial
        {
            private static void FooMethod()
            {
            }
        }

        private static void FooExtensionMethod<T>(this T value)
        {
        }

        private partial class FooPartial
        {
        }
    }
}
