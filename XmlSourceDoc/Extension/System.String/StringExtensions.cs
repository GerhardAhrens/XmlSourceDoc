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

    public static class StringExtensions
    {
        public static IEnumerable<string> SplitAt(this string @this, params int[] index)
        {
            var indices = new[] { 0 }.Union(index).Union(new[] { @this.Length });

            return indices.Zip(indices.Skip(1), (a, b) => (a, b)).Select(_ => @this.Substring(_.a, _.b - _.a));
        }

        public static IEnumerable<string> SplitAt(this string @this, char removeChar, params int[] index)
        {
            var indices = new[] { 0 }.Union(index).Union(new[] { @this.Length });

            return indices.Zip(indices.Skip(1), (a, b) => (a, b)).Select(_ => @this.Substring(_.a, _.b - _.a).RemoveChar(removeChar).ToString());
        }

        public static string RemoveChar(this string @this, params char[] removingChars)
        {
            foreach (char c in removingChars)
            {
                @this = @this.Replace(c.ToString(), string.Empty);
            }

            return @this;
        }
    }
}
