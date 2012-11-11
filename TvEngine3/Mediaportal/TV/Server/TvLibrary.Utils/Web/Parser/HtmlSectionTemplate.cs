#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Mediaportal.TV.Server.TvLibrary.Utils.Web.Parser
{
  /// <summary>
  /// Xml Serializable Class for SectionTemplate data.
  /// </summary>
  [Serializable]
  public class HtmlSectionTemplate
  {
    #region Variables

    [XmlArray("MatchList")] [XmlArrayItem("Match")] public List<HtmlSectionMatch> MatchValues;
    [XmlAttribute("tags")] public string Tags;
    [XmlElement("TemplateText")] public string Template;

    #endregion
  }
}