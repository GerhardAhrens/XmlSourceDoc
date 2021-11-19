namespace XmlSourceDoc.DocumentExport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDocument
    {
        string Assembly { get; }

        string Namespace { get; }
    }
}
