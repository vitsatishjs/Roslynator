// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Pihrtsoft.Markdown
{
    [DebuggerDisplay("{Kind} {Name}")]
    public class EntityReference : MElement
    {
        public EntityReference(string name)
            : this(name, validate: true)
        {
        }

        internal EntityReference(string name, bool validate)
        {
            if (validate)
                ThrowOnInvalidName(name);

            Name = name;
        }

        internal static void ThrowOnInvalidName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length == 0)
                throw new ArgumentException(ErrorMessages.EntityNameCannotBeEmpty, nameof(name));

            if (!TextUtility.IsAlphanumeric(name))
                throw new ArgumentException(ErrorMessages.EntityNameCanContainsOnlyAlphanumericCharacters, nameof(name));
        }

        public EntityReference(EntityReference other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Name = other.Name;
        }

        public string Name { get; }

        public override MarkdownKind Kind => MarkdownKind.EntityReference;

        public override MarkdownBuilder AppendTo(MarkdownBuilder builder)
        {
            return builder.AppendEntityReference(Name);
        }

        internal override MElement Clone()
        {
            return new EntityReference(this);
        }

        internal static EntityReference CreateTrusted(string name)
        {
            return new EntityReference(name, validate: false);
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
