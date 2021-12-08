//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="PTA">
//     Class: StringExtensions
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@contractors.roche.com</email>
// <date>1.1.2016</date>
//
// <summary>Extensions Class for String Types</summary>
//-----------------------------------------------------------------------

namespace System
{
    using global::System.Collections.Generic;
    using global::System.Linq;

    public static class CharExtensions
    {
        public static IEnumerable<char> RemoveChars(this IEnumerable<char> originalString, params char[] removingChars)
        {
            return originalString.Except(removingChars);
        }
    }
}
