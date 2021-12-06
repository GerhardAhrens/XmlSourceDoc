namespace XmlSourceDocDemo
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Demoklasse
    /// </summary>
    public class DemoClassFull
    {
        /// <summary>
        /// Konstruktormethode
        /// </summary>
        public DemoClassFull()
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, String
        /// </summary>
        /// <param name="args">Parameter, string</param>
        public DemoClassFull(string args)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, Integer
        /// </summary>
        /// <param name="args">Parameter, int</param>
        public DemoClassFull(int args)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, Integer, String
        /// </summary>
        /// <param name="argsInt">Parameter int</param>
        /// <param name="argsString">Parameter string</param>
        public DemoClassFull(int argsInt, string argsString)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, Tuple
        /// </summary>
        /// <param name="argsTuple">Ein Tuple als Parameter</param>
        public DemoClassFull(Tuple<string, bool> argsTuple)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, List of String
        /// </summary>
        /// <param name="args">Parameter als List of String</param>
        public DemoClassFull(List<string> args)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, List of String, Boolean
        /// </summary>
        /// <param name="args"></param>
        /// <param name="argsBool"></param>
        public DemoClassFull(List<string> args, bool argsBool)
        {
        }

        /// <summary>
        /// Konstruktormethode mit Parameter, Tuple, Dictionary, DateTime
        /// </summary>
        /// <param name="argsTuple"></param>
        /// <param name="argsDict"></param>
        /// <param name="argsDt"></param>
        public DemoClassFull(Tuple<string, bool> argsTuple, 
            Dictionary<Guid,string> argsDict, 
            DateTime argsDt)
        {
        }


        /// <summary>
        /// Get/Set for Numeric Value
        /// </summary>
        public int Numeric { get; set; }

        /// <summary>
        /// Gibt die aktuelle Datum/Zeit zurück
        /// </summary>
        public DateTime CurrentDateTime
        {
            get { return DateTime.Now; }
        }


        /// <summary>
        /// Display a name
        /// </summary>
        /// <param name="name">Textparameter</param>
        /// <example>
        /// Beispiel: ShowName("Gerhard")
        /// </example>
        /// <remarks>
        /// Prüfen, ob der Tag in der XML Docu auftaucht
        /// </remarks>
        public static void ShowName(string name)
        {
            Console.WriteLine($"Hi {name}");
        }

        /// <summary>
        /// Display a name a certain number of times
        /// </summary>
        public static void ShowName(string name, byte repeats)
        {
            for (int i = 0; i < repeats; i++)
                Console.WriteLine($"Hi {name}");
        }

        /// <summary>
        /// Display the type of the variable passed in, One Args
        /// </summary>
        public static void ShowGenericType<T>(T myVar)
        {
            Console.WriteLine($"Generic type {myVar.GetType()}");
        }

        /// <summary>
        /// Display the type of the variable passed in, Two Args
        /// </summary>
        public static void ShowGenericType<K, V>(K myVar1, V myVar2)
        {
            Console.WriteLine($"Generic type {myVar1.GetType()}");
        }

        /// <summary>
        /// Display the value of a nullable integer
        /// </summary>
        public static void ShowNullableInt(int? myInt)
        {
            Console.WriteLine(myInt);
        }

        /// <summary>
        /// Die Methode führ eine Kalkulation durch
        /// </summary>
        /// <param name="v1">Wert 1</param>
        /// <param name="v2">Wert 2</param>
        /// <param name="op">Opearor (+, -, *, /)</param>
        /// <returns>Ergebnis der Kalkulation</returns>
        public double Calc(double v1, double v2, string op)
        {
            return default;
        }

        /// <summary>
        /// Die Methode führ eine Kalkulation durch
        /// </summary>
        /// <param name="v1">Wert 1</param>
        /// <param name="v2">Wert 2</param>
        /// <param name="op">Opearor (+, -, *, /)</param>
        /// <returns>Ergebnis der Kalkulation</returns>
        public double Calc(double v1, double v2, Operators op)
        {
            return default;
        }

        /// <summary>
        /// Klasseninhalt als String zurückgeben
        /// </summary>
        /// <returns>Inhalt als String</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
