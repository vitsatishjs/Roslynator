// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown.Linq
{
    public abstract class MBlockContainer : MContainer
    {
        protected MBlockContainer()
        {
        }

        protected MBlockContainer(object content)
            : base(content)
        {
        }

        protected MBlockContainer(params object[] content)
            : base(content)
        {
        }

        protected MBlockContainer(MBlockContainer other)
            : base(other)
        {
        }
    }
}