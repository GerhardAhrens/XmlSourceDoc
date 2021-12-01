﻿namespace XmlSourceDocDemo
{
    using EasyPrototyping.XMLSourceDoc;

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Startprogramm
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
    /// https://www.codeproject.com/articles/11701/documentation-in-c
    /// https://www.oreilly.com/library/view/c-in-a/0596001819/ch04s10.html
    /// </remarks>
    public class Program
    {
        private static void Main(string[] args)
        {
            DemoClassShort dom = new DemoClassShort();
            dom.Id = Guid.NewGuid();
            dom.ClassName = "Test-1-A";

            InMemoryRepository<DemoClassShort> repository = new InMemoryRepository<DemoClassShort>();
            Console.WriteLine($"Count={repository.CountByType()}", ConsoleColor.Yellow);
            Console.WriteLine($"Add: {dom.ClassName}");
            repository.Add(dom);
            Console.WriteLine($"Count={repository.CountByType()}", ConsoleColor.Yellow);

            DemoClassShort dom2 = new DemoClassShort();
            dom2.Id = Guid.NewGuid();
            dom2.ClassName = "Test-2-A";
            Console.WriteLine($"Add: {dom2.ClassName}");
            repository.Add(dom2);
            Console.WriteLine($"Count={repository.CountByType()}", ConsoleColor.Yellow);

            repository.SaveContent(@"c:\temp\test.xml");

            repository.LoadContent(@"c:\temp\test.xml");

            Console.WriteLine($"FindById: {dom.Id}");
            DemoClassShort result1 = repository.FindById(dom.Id);
            Console.WriteLine($"Result:{result1.ClassName};Id={result1.Id}");

            dom.ClassName = "Test-B";
            Console.WriteLine($"Update:{dom.ClassName};Id={dom.Id}");
            repository.Update(dom);
            Console.WriteLine($"Count={repository.CountByType()}", ConsoleColor.Yellow);
            DemoClassShort result2 = repository.FindById(dom.Id);
            Console.WriteLine($"Result:{result2.ClassName};Id={result2.Id}");

            /*repository.DeleteAllByType();*/

            Console.WriteLine($"Delete:{result2.ClassName};Id={result2.Id}");
            repository.Delete(result2);
            Console.WriteLine($"Count={repository.CountByType()}", ConsoleColor.Yellow);
            bool exist = repository.Exist(result2);
            Console.WriteLine($"Exist:{exist}");

            /*

            // read XML file to get info about documented methods
            string xmlFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "XmlSourceDocDemo.xml");
            XmlSourceDocumentation xd = new XmlSourceDocumentation(xmlFile);
            */
            /*
            Type classType = typeof(DemoClassFull);
            IEnumerable<SourceDocumentation> docList = xd.Get(classType);
            foreach (SourceDocumentation docItem in docList)
            {
                if (docItem.MemberTyp.Item1 == "M" && docItem.MemberTyp.Item2 == MemberTyp.Construktor)
                {
                    Console.WriteLine(docItem.Class, ConsoleColor.Red);
                    Console.WriteLine($"{docItem.Assembly}.{docItem.Namespace}", ConsoleColor.Red);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    Console.WriteLine(docItem.Constructor, ConsoleColor.White);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    if (docItem.Parameter != null)
                    {
                        Console.WriteLine("Parameter und Beschreibung:", ConsoleColor.DarkGray);
                        foreach (string paramDoc in docItem.MemberParams)
                        {
                            Console.WriteLine(paramDoc, ConsoleColor.White);
                        }

                        Console.WriteLine();

                        foreach (Tuple<int, string, string> parameter in docItem.Parameter.OrderBy(o => o.Item1))
                        {
                            Console.WriteLine($"({parameter.Item1}) {parameter.Item3} {parameter.Item2}", ConsoleColor.White);
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    Console.WriteLine("Beschreibung:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Summary, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Beispiel:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Example, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Hinweis:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Remark, ConsoleColor.White);

                    Console.WriteLine(new string('-', 40), ConsoleColor.Blue);
                }
                else if (docItem.MemberTyp.Item1 == "P" && docItem.MemberTyp.Item2 == MemberTyp.Property)
                {
                    Console.WriteLine(docItem.Class, ConsoleColor.Red);
                    Console.WriteLine($"{docItem.Assembly}.{docItem.Namespace}", ConsoleColor.Red);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    Console.WriteLine(docItem.Member, ConsoleColor.White);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    Console.WriteLine("Beschreibung:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Summary, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Return Wert und Beschreibung:", ConsoleColor.DarkGray);
                    if (docItem.Return != null)
                    {
                        if (string.IsNullOrEmpty(docItem.Return.Item2) == false)
                        {
                            Console.WriteLine(docItem.Return.Item2, ConsoleColor.White);
                        }

                        Console.WriteLine(docItem.Return.Item1, ConsoleColor.White);
                    }

                    Console.WriteLine();

                    Console.WriteLine("Beispiel:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Example, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Hinweis:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Remark, ConsoleColor.White);

                    Console.WriteLine(new string('-', 40), ConsoleColor.Blue);
                }
                else
                {
                    Console.WriteLine(docItem.Class, ConsoleColor.Red);
                    Console.WriteLine($"{docItem.Assembly}.{docItem.Namespace}", ConsoleColor.Red);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    Console.WriteLine(docItem.Member, ConsoleColor.White);
                    Console.WriteLine(new string('-', 40), ConsoleColor.Red);
                    Console.WriteLine("Beschreibung:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Summary, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Return Wert und Beschreibung:", ConsoleColor.DarkGray);
                    if (docItem.Return != null)
                    {
                        if (string.IsNullOrEmpty(docItem.Return.Item2) == false)
                        {
                            Console.WriteLine(docItem.Return.Item2, ConsoleColor.White);
                        }

                        Console.WriteLine(docItem.Return.Item1, ConsoleColor.White);
                    }

                    Console.WriteLine();

                    Console.WriteLine("Beispiel:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Example, ConsoleColor.White);
                    Console.WriteLine();
                    Console.WriteLine("Hinweis:", ConsoleColor.DarkGray);
                    Console.WriteLine(docItem.Remark, ConsoleColor.White);

                    Console.WriteLine(new string('-', 40), ConsoleColor.Blue);
                }
            }
            */

            Console.ReadKey();
        }
    }
}
