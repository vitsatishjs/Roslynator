// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp.Refactorings.Tests
{
    internal static class AddMemberToInterface
    {
        public interface IFoo
        {
            void BarExplicit();
            void Bar();
        }

        public interface IFoo2
        {
            void BarExplicit2();
        }

        public interface IFoo3
        {
        }

        public interface IFoo<T>
        {
        }

        private class BaseFoo
        {
        }

        private class Foo<T> : BaseFoo, IFoo, IFoo2, IFoo<T>
        {
            public void Bar()
            {
            }

            public void Bar(string s)
            {

            }

            void IFoo.BarExplicit()
            {
                throw new NotImplementedException();
            }

            void IFoo2.BarExplicit2()
            {
                throw new NotImplementedException();
            }
        }
    }
}
