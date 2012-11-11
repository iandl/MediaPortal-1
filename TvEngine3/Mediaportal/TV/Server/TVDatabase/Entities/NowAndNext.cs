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
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
  [DataContract]
  public class NowAndNext
  {
    private string _episodeName;
    private string _episodeNameNext;
    private string _episodeNum;
    private string _episodeNumNext;
    private string _episodePart;
    private string _episodePartNext;
    private int _idChannel;
    private int _idProgramNext;
    private int _idProgramNow;
    private DateTime _nowEnd;
    private DateTime _nowStart;
    private string _seriesNum;
    private string _seriesNumNext;
    private string _titleNext;
    private string _titleNow;

    public NowAndNext(int idChannel, DateTime nowStart, DateTime nowEnd, string titleNow, string titleNext,
                      int idProgramNow, int idProgramNext,
                      string episodeName, string episodeNameNext, string seriesNum, string seriesNumNext,
                      string episodeNum, string EpisodeNumNext, string episodePart, string episodePartNext)
    {
      _idChannel = idChannel;
      _nowStart = nowStart;
      _nowEnd = nowEnd;
      _titleNow = titleNow;
      _titleNext = titleNext;
      _idProgramNow = idProgramNow;
      _idProgramNext = idProgramNext;
      _episodeName = episodeName;
      _episodeNameNext = episodeNameNext;
      _seriesNum = seriesNum;
      _seriesNumNext = seriesNumNext;
      _episodeNum = episodeNum;
      _episodeNumNext = EpisodeNumNext;
      _episodePart = episodePart;
      _episodePartNext = episodePartNext;
    }

    [DataMember]
    public int IdChannel
    {
      get { return _idChannel; }
      set { _idChannel = value; }
    }

    [DataMember]
    public DateTime NowStartTime
    {
      get { return _nowStart; }
      set { _nowStart = value; }
    }

    [DataMember]
    public DateTime NowEndTime
    {
      get { return _nowEnd; }
      set { _nowEnd = value; }
    }

    [DataMember]
    public string TitleNow
    {
      get { return _titleNow; }
      set { _titleNow = value; }
    }

    [DataMember]
    public string TitleNext
    {
      get { return _titleNext; }
      set { _titleNext = value; }
    }

    [DataMember]
    public int IdProgramNow
    {
      get { return _idProgramNow; }
      set { _idProgramNow = value; }
    }

    [DataMember]
    public int IdProgramNext
    {
      get { return _idProgramNext; }
      set { _idProgramNext = value; }
    }

    [DataMember]
    public string EpisodeName
    {
      get { return _episodeName; }
      set { _episodeName = value; }
    }

    [DataMember]
    public string EpisodeNameNext
    {
      get { return _episodeNameNext; }
      set { _episodeNameNext = value; }
    }

    [DataMember]
    public string SeriesNum
    {
      get { return _seriesNum; }
      set { _seriesNum = value; }
    }

    [DataMember]
    public string SeriesNumNext
    {
      get { return _seriesNumNext; }
      set { _seriesNumNext = value; }
    }

    [DataMember]
    public string EpisodeNum
    {
      get { return _episodeNum; }
      set { _episodeNum = value; }
    }

    [DataMember]
    public string EpisodeNumNext
    {
      get { return _episodeNumNext; }
      set { _episodeNumNext = value; }
    }

    [DataMember]
    public string EpisodePart
    {
      get { return _episodePart; }
      set { _episodePart = value; }
    }

    [DataMember]
    public string EpisodePartNext
    {
      get { return _episodePartNext; }
      set { _episodePartNext = value; }
    }
  }
}