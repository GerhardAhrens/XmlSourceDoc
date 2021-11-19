//-----------------------------------------------------------------------
// <copyright file="Contracts.cs" company="Lifeprojects.de">
//     Class: Contracts
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>13.07.2017</date>
//
// <summary>Class for Checking from Mathodes Parameter</summary>
//-----------------------------------------------------------------------

namespace EasyPrototyping.Core
{
    using System;
    using System.ComponentModel;

    public sealed class Contracts
    {
        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// erfüllt wird.
        /// </summary>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Ensures(Func<bool> predicate, string message)
        {
            if (predicate() == true)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// erfüllt wird.
        /// </summary>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Ensures<TException>(Func<bool> predicate, string message = "") where TException : Exception, new()
        {
            if (predicate() == true)
            {
                throw Activator.CreateInstance(typeof(TException), message) as TException;
            }
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// erfüllt wird.
        /// </summary>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Ensures<TException>(bool conditon, string message = "") where TException : Exception, new()
        {
            if (conditon == true)
            {
                throw Activator.CreateInstance(typeof(TException), message) as TException;
            }
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// für den Vertrag nicht erfüllt wird.
        /// </summary>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Requires(Func<bool> predicate, string message)
        {
            if (predicate() == false)
            {
                throw new ArgumentException(message);
            }
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// für den Vertrag nicht erfüllt wird.
        /// </summary>
        /// <typeparam name="TException">
        /// The exception to be raised.
        /// </typeparam>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Requires<TException>(Func<bool> predicate, string message = "") where TException : Exception, new()
        {
            if (!predicate())
            {
                throw Activator.CreateInstance(typeof(TException), message) as TException;
            }
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// für den Vertrag nicht erfüllt wird.
        /// </summary>
        /// <typeparam name="TException">
        /// The exception to be raised.
        /// </typeparam>
        /// <param name="conditon">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void Requires<TException>(bool conditon, string message = "") where TException : Exception, new()
        {
            if (conditon == false)
            {
                throw Activator.CreateInstance(typeof(TException), message) as TException;
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if value equals null.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="value">Value to be checked.</param>
        public static void ThrowIfIsNull<T>(T value) where T : class
        {
            if (value == null)
            {
                var typeName = typeof(T).Name;
                throw new ArgumentNullException($"Parameter value of type {typeName} is not allowed to be null.");
            }
        }

        /// <summary>
        /// Casts the value to the given type.
        /// If cast fails an InvalidCastException will be throw.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">the input value to be casted.</param>
        /// <returns>The casted value.</returns>
        public static T CastOrThrow<T>(object value)
        {
            object getAs = null;

            try
            {
                getAs = value == null ? default(T) : (T)Convert.ChangeType(value, typeof(T));

                if ((getAs.GetType() == typeof(T)) == false)
                {
                    throw new InvalidCastException($"Incorrect type, cannot cast to type {typeof(T).FullName}");
                }

                return (T)getAs;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Incorrect type, cannot cast to type {typeof(T).FullName}");
            }
        }

        public static T CastOrThrow<T, TException>(object value) where TException : Exception, new()
        {
            object getAs = null;

            try
            {
                getAs = value == null ? default(T) : (T)Convert.ChangeType(value, typeof(T));

                if ((getAs.GetType() == typeof(T)) == false)
                {
                    throw Activator.CreateInstance(
                        typeof(TException),
                        $"Incorrect type, cannot cast to type {typeof(T).FullName}") as TException;
                }

                return (T)getAs;
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Incorrect type, cannot cast to type {typeof(T).FullName}");
            }
        }

        public static bool TryCast<T>(object obj, out T result)
        {
            result = default(T);
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            if (obj != null)
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter.CanConvertFrom(obj.GetType()))
                    {
                        result = (T)converter.ConvertFrom(obj);
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception)
                {
                    throw new InvalidCastException($"Incorrect type, cannot cast to type {typeof(T).FullName}");
                }
            }

            return !typeof(T).IsValueType;
        }
    }
}