// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    //TODO: MEntityRef
    [DebuggerDisplay("{Kind} {Name}")]
    public class MEntityReference : MElement
    {
        private string _name;

        public MEntityReference(string name)
            : this(name, isTrustedName: false)
        {
        }

        private MEntityReference(string name, bool isTrustedName)
        {
            if (isTrustedName)
            {
                _name = name;
            }
            else
            {
                Name = name;
            }
        }

        public MEntityReference(MEntityReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            _name = other.Name;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                Error.ThrowOnInvalidEntityName(value);

                _name = value;
            }
        }

        public override MarkdownKind Kind => MarkdownKind.EntityReference;

        public override MarkdownWriter WriteTo(MarkdownWriter writer)
        {
            return writer.WriteEntityReference(Name);
        }

        internal override MElement Clone()
        {
            return new MEntityReference(this);
        }

        private static MEntityReference CreateTrusted(string name)
        {
            return new MEntityReference(name, isTrustedName: true);
        }

        //TODO: move to MFactory
        public static MEntityReference NonBreakingSpace() => CreateTrusted("nbsp");

        public static MEntityReference LessThan() => CreateTrusted("lt");

        public static MEntityReference GreaterThan() => CreateTrusted("gt");

        public static MEntityReference Ampersand() => CreateTrusted("amp");

        public static MEntityReference DoubleQuotationMark() => CreateTrusted("quot");

        public static MEntityReference RegisteredTrademark() => CreateTrusted("reg");

        public static MEntityReference Copyright() => CreateTrusted("copy");
    }
}
