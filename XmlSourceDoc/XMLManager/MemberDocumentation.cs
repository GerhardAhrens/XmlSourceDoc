namespace EasyPrototyping.XMLSourceDoc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DocClass für ein Member
    /// </summary>
    public class MemberDocumentation
    {
        public string Assembly { get; set; }

        /// <summary>
        /// Membertyp
        /// </summary>
        public Tuple<string, MemberTyp> MemberTyp { get; set; }

        public string FullName { get; set; }

        public string FullNameWithTyp { get; set; }

        /// <summary>
        /// Summery Member
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Beschreibung zu den Parameterns
        /// </summary>
        public Dictionary<string,Tuple<string, string, string>> MemberParams { get; set; }

        /// <summary>
        /// Return Member
        /// </summary>
        public string Returns { get; set; }

        /// <summary>
        /// Gibt aus dem XML Documentation den Text einer Bemerkung/Hinweis zurück
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Gibt aus dem XML Documentation den Text zu einem zurück
        /// </summary>
        public string Example { get; set; }
    }
}
