// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Pihrtsoft.Markdown
{
    internal static class ErrorMessages
    {
        public const string OrderedListItemNumberCannotBeNegative = "Ordered list item number cannot be negative.";

        public const string HeadingLevelMustBeInRangeFromOneToSix = "Heading level must be in range from 1 to 6.";

        public const string NumberOfCharactersInHorizontalRuleCannotBeLessThanThree = "Number of characters in horizontal rule cannot be less than 3.";

        public const string EntityNameCannotBeEmpty = "Entity name cannot be empty.";

        public const string EntityNameCanContainsOnlyAlphanumericCharacters = "Entity name can contains only alphanumeric characters.";

        public static string UnknownEnumValue(object value)
        {
            return $"Unknown enum value '{value}'.";
        }
    }
}
