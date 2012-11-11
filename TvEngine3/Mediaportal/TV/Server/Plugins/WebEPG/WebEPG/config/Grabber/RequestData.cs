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

using System.Xml.Serialization;

namespace WebEPG.config.Grabber
{
  /// <summary>
  /// Request Search Data
  /// </summary>
  public class RequestData
  {
    #region Variables

    [XmlAttribute("baseDate")] public string BaseDate;
    [XmlIgnore()] public string ChannelId;
    [XmlArray("DayNames")] [XmlArrayItem("Day")] public string[] DayNames;
    [XmlAttribute("listStart")] public int ListStart = 0;
    [XmlAttribute("maxlistings")] public int MaxListingCount = 0;
    [XmlAttribute("startOffset")] public int OffsetStart;
    [XmlAttribute("endPage")] public int PageEnd;
    [XmlAttribute("startPage")] public int PageStart;
    [XmlAttribute("language")] public string SearchLang = "en-US";
    [XmlAttribute("weekday")] public string WeekDay = "dddd";
    [XmlArray("WeekDayNames")] [XmlArrayItem("WeekDay")] public string[] WeekDayNames;

    #endregion
  }
}