using System;

namespace XmlSourceDocDemo
{
    /// <summary>
    /// Eine kleine Demo Klasse
    /// </summary>
    public class DemoClassShort : IDomainRoot
    {
        /// <summary>
        /// Konstruktor ohne Parameter
        /// </summary>
        public DemoClassShort()
        {
        }

        /// <summary>
        /// Object Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClassName { get; set; }

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
