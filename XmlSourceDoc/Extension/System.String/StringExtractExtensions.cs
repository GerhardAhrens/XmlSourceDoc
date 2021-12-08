//-----------------------------------------------------------------------
// <copyright file="StringExtractExtensions.cs" company="PTA">
//     Class: StringExtractExtensions
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
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Collections.Generic;

    using EasyPrototyping.Core;

    public static class StringExtractExtensions
    {
        public static IEnumerable<string> ExtractFromString(this string @this, string startString, string endString)
        {
            if (string.IsNullOrEmpty(@this) == true || string.IsNullOrEmpty(startString) == false || string.IsNullOrEmpty(endString) == false)
            {
                yield return null;
            }

            Regex r = new Regex(Regex.Escape(startString) + "(.*?)" + Regex.Escape(endString), RegexOptions.Singleline);
            MatchCollection matches = r.Matches(@this);
            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value;
            }
        }

        public static T[] ExtractContent<T>(this string @this, string regex)
        {
            if (string.IsNullOrEmpty(@this) == true && string.IsNullOrEmpty(regex) == false)
            {
                return new T[0];
            }

            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(typeof(string)) == false)
            {
                throw new ArgumentException("Type does not have a TypeConverter from string", "T");
            }
            if (string.IsNullOrEmpty(@this) == false)
            {
                return
                    Regex.Matches(@this, regex)
                    .Cast<Match>()
                    .Select(f => f.ToString())
                    .Select(f => (T)tc.ConvertFrom(f))
                    .ToArray();
            }
            else
            {
                return new T[0];
            }
        }

        public static int[] ExtractInts(this string @this)
        {
            return @this.ExtractContent<int>(@"\d+");
        }
    }
}