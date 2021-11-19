//-----------------------------------------------------------------------
// <copyright file="MemberTyp.cs" company="Lifeprojects.de">
//     Class: MemberTyp
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>12.11.2021</date>
//
// <summary>
// Class for Enum Typ 
// </summary>
//-----------------------------------------------------------------------

namespace EasyPrototyping.XMLSourceDoc
{
    using System.ComponentModel;

    public enum MemberTyp : int
    {
        [Description("Unbekannt")]
        None = 0,
        [Description("Class")]
        Class = 1,
        [Description("Construktor")]
        Construktor = 2,
        [Description("Methode")]
        Methode = 3,
        [Description("Property")]
        Property = 4,
        [Description("Enum")]
        Enum = 5
    }
}
