namespace EasyPrototyping.XMLSourceDoc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// SourceDocumentation für ein Member
    /// </summary>
    [DebuggerDisplay("MemberTyp ={MemberTyp}; Member = {Member}")]
    public class SourceDocumentation
    {
        public int PositionInDoc { get; set; }

        public string Assembly { get; set; }

        public string Namespace { get; set; }

        public string Class { get; set; }

        public Tuple<string, MemberTyp> MemberTyp { get; set; }

        public string Constructor { get; set; }

        public string Summary { get; set; }

        public string Member { get; set; }

        /// <summary>
        /// Beschreibung zu den Parameterns
        /// </summary>
        public List<string> MemberParams { get; set; }

        public List<Tuple<int, string, string>> Parameter { get; set; }

        public Tuple<string,string> Return { get; set; }

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
