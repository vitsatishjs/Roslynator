// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown.Linq
{
    [DebuggerDisplay("{Kind} {Name}")]
    public class EntityReference : MElement
    {
        private string _name;

        public EntityReference(string name)
            : this(name, isTrustedName: false)
        {
        }

        private EntityReference(string name, bool isTrustedName)
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

        public EntityReference(EntityReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Name = other.Name;
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
            return new EntityReference(this);
        }

        private static EntityReference CreateTrusted(string name)
        {
            return new EntityReference(name, isTrustedName: true);
        }

        public static EntityReference NonBreakingSpace() => CreateTrusted("nbsp");

        public static EntityReference LessThan() => CreateTrusted("lt");

        public static EntityReference GreaterThan() => CreateTrusted("gt");

        public static EntityReference Ampersand() => CreateTrusted("amp");

        public static EntityReference DoubleQuotationMark() => CreateTrusted("quot");

        public static EntityReference RegisteredTrademark() => CreateTrusted("reg");

        public static EntityReference Copyright() => CreateTrusted("copy");
    }
}
