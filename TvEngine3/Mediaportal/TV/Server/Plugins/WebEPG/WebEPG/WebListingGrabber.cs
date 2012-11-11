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
using System.IO;
using System.Xml.Serialization;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.TVBusinessLayer;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;
using Mediaportal.TV.Server.TvLibrary.Utils.Time;
using Mediaportal.TV.Server.TvLibrary.Utils.Web.Parser;
using Mediaportal.TV.Server.TvLibrary.Utils.Web.http;
using WebEPG.Parser;
using WebEPG.config.Grabber;

namespace WebEPG
{
  /// <summary>
  /// Get the listing for a given Channel
  /// </summary>
  public class WebListingGrabber
  {
    #region Variables

    private int _dbLastProg;
    private IList<Program> _dbPrograms;
    private bool _dblookup = true;
    private int _discarded;
    private bool _grabLinked;
    private DateTime _grabStart;
    private GrabberConfigFile _grabber;
    private TimeRange _linkTimeRange;
    private int _maxGrabDays;

    private IParser _parser;
    private List<ProgramData> _programs;
    private RequestBuilder _reqBuilder;
    private RequestData _reqData;
    private WorldTimeZone _siteTimeZone = null;
    private string _strBaseDir = string.Empty;
    private string _strID = string.Empty;
    private ListingTimeControl _timeControl;

    #endregion

    #region Constructors/Destructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="maxGrabDays">The number of days to grab</param>
    /// <param name="baseDir">The baseDir for grabber files</param>
    public WebListingGrabber(string baseDir)
    {
      _strBaseDir = baseDir;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initalises the ListingGrabber class with a grabber config file
    /// </summary>
    /// <param name="File">The grabber config file file.</param>
    /// <returns>bool - success/fail loading the config file</returns>
    public bool Initalise(string File, int maxGrabDays)
    {
      _maxGrabDays = maxGrabDays;

      // Load configuration file
      this.LogInfo("WebEPG: Opening {0}", File);

      try
      {
        //_grabber = new GrabberConfig(_strBaseDir + File);

        XmlSerializer s = new XmlSerializer(typeof (GrabberConfigFile));

        using (TextReader r = new StreamReader(_strBaseDir + File)) 
        {
          _grabber = (GrabberConfigFile)s.Deserialize(r);
        }
      }
      catch (InvalidOperationException ex)
      {
        this.LogError(ex, "WebEPG: Config Error {0}", File);
        return false;
      }

      if (string.IsNullOrEmpty(_grabber.Info.Version))
      {
        this.LogInfo("WebEPG: Unknown Version");
      }
      else
      {
        this.LogInfo("WebEPG: Version: {0}", _grabber.Info.Version);
      }

      if (_grabber.Listing.SearchParameters == null)
      {
        _grabber.Listing.SearchParameters = new RequestData();
      }

      _reqData = _grabber.Listing.SearchParameters;

      // Setup timezone
      this.LogInfo("WebEPG: TimeZone, Local: {0}", TimeZone.CurrentTimeZone.StandardName);

      _siteTimeZone = null;
      if (!string.IsNullOrEmpty(_grabber.Info.TimeZone))
      {
        try
        {
          this.LogInfo("WebEPG: TimeZone, Site : {0}", _grabber.Info.TimeZone);
          _siteTimeZone = new WorldTimeZone(_grabber.Info.TimeZone);
        }
        catch (ArgumentException)
        {
          this.LogError("WebEPG: TimeZone Not valid");
          _siteTimeZone = null;
        }
      }

      if (_siteTimeZone == null)
      {
        this.LogInfo("WebEPG: No site TimeZone, using Local: {0}", TimeZone.CurrentTimeZone.StandardName);
        _siteTimeZone = new WorldTimeZone(TimeZone.CurrentTimeZone.StandardName);
      }

      switch (_grabber.Listing.listingType)
      {
        case ListingInfo.Type.Xml:
          _parser = new XmlParser(_grabber.Listing.XmlTemplate);
          break;

        case ListingInfo.Type.Data:

          if (_grabber.Listing.DataTemplate.Template == null)
          {
            this.LogError("WebEPG: {0}: No Template", File);
            return false;
          }
          _parser = new DataParser(_grabber.Listing.DataTemplate);
          break;

        case ListingInfo.Type.Html:
          HtmlParserTemplate defaultTemplate = _grabber.Listing.HtmlTemplate.GetTemplate("default");
          if (defaultTemplate == null ||
              defaultTemplate.SectionTemplate == null ||
              defaultTemplate.SectionTemplate.Template == null)
          {
            this.LogError("WebEPG: {0}: No Template", File);
            return false;
          }
          _parser = new WebParser(_grabber.Listing.HtmlTemplate);
          if (_grabber.Info.GrabDays < _maxGrabDays)
          {
            this.LogInfo("WebEPG: Grab days ({0}) more than Guide days ({1}), limiting grab to {1} days",
                         _maxGrabDays, _grabber.Info.GrabDays);
            _maxGrabDays = _grabber.Info.GrabDays;
          }

          break;
      }

      return true;
    }

    /// <summary>
    /// Gets the guide for a given channel.
    /// </summary>
    /// <param name="strChannelID">The channel ID.</param>
    /// <param name="Linked">if set to <c>true</c> get [linked] pages.</param>
    /// <param name="linkStart">The start time to get link pages.</param>
    /// <param name="linkEnd">The end time to get linked pages.</param>
    /// <returns>list of programs</returns>
    public List<ProgramData> GetGuide(string strChannelID, string displayName, bool Linked, TimeRange linkTime)
    {
      // Grab with start time Now
      return GetGuide(strChannelID, displayName, Linked, linkTime, DateTime.Now);
    }

    /// <summary>
    /// Gets the guide for a given channel.
    /// </summary>
    /// <param name="strChannelID">The channel ID.</param>
    /// <param name="Linked">if set to <c>true</c> get [linked] pages.</param>
    /// <param name="linkStart">The start time to get link pages.</param>
    /// <param name="linkEnd">The end time to get linked pages.</param>
    /// <param name="startDateTime">The start date time for grabbing.</param>
    /// <returns>list of programs</returns>
    public List<ProgramData> GetGuide(string strChannelID, string displayName, bool Linked, TimeRange linkTime,
                                      DateTime startDateTime)
    {
      _strID = strChannelID;
      _grabLinked = Linked;
      _linkTimeRange = linkTime;
      //int offset = 0;

      _reqData.ChannelId = _grabber.GetChannel(strChannelID);
      if (_reqData.ChannelId == null)
      {
        this.LogError("WebEPG: ChannelId: {0} not found!", strChannelID);
        return null;
      }

      //_removeProgramsList = _grabber.GetRemoveProgramList(strChannelID); // <--- !!!

      _programs = new List<ProgramData>();

      this.LogInfo("WebEPG: ChannelId: {0}", strChannelID);

      //_GrabDay = 0;

      // sets a minimum delay for the programlist Site page grabbing
      // why? likely not needed. to be tested.
      // possible reason: 'relaxing' webservers
      if (_grabber.Listing.Request.Delay < 500)
      {
        _grabber.Listing.Request.Delay = 500;
      }
      WorldDateTime reqStartTime = new WorldDateTime(_siteTimeZone.FromLocalTime(startDateTime), _siteTimeZone);
      _reqBuilder = new RequestBuilder(_grabber.Listing.Request, reqStartTime, _reqData);
      _grabStart = startDateTime;

      this.LogDebug("WebEPG: Grab Start {0} {1}", startDateTime.ToShortTimeString(),
                    startDateTime.ToShortDateString());
      int requestedStartDay = startDateTime.Subtract(DateTime.Now).Days;
      if (requestedStartDay > 0)
      {
        if (requestedStartDay > _grabber.Info.GrabDays)
        {
          this.LogError("WebEPG: Trying to grab past guide days");
          return null;
        }

        if (requestedStartDay + _maxGrabDays > _grabber.Info.GrabDays)
        {
          _maxGrabDays = _grabber.Info.GrabDays - requestedStartDay;
          this.LogInfo("WebEPG: Grab days more than Guide days, limiting to {0}", _maxGrabDays);
        }

        //_GrabDay = requestedStartDay;
        _reqBuilder.DayOffset = requestedStartDay;
        if (_reqBuilder.DayOffset > _maxGrabDays) //_GrabDay > _maxGrabDays)
        {
          _maxGrabDays = _reqBuilder.DayOffset + _maxGrabDays; // _GrabDay + _maxGrabDays;
        }
      }

      _dbPrograms = new List<Program>();
      _dbLastProg = 0;

      try
      {
        IList<Channel> epgChannels = ChannelManagement.GetChannelsByName(displayName);
        if (epgChannels.Count > 0)
        {
          _dbPrograms = epgChannels[0].Programs;
        }
      }
      catch (Exception)
      {
        this.LogError("WebEPG: Database failed, disabling db lookup");
        _dblookup = false;
      }

      _timeControl = new ListingTimeControl(_siteTimeZone.FromLocalTime(startDateTime));
      while (_reqBuilder.DayOffset < _maxGrabDays)
      {
        _reqBuilder.Offset = 0;

        bool error;
        while (GetListing(out error))
        {
          //if (_grabber.Listing.SearchParameters.MaxListingCount == 0)
          //  break;
          _reqBuilder.Offset++;
        }

        if (error)
        {
          if (!_grabber.Info.TreatErrorAsWarning)
          {
            this.LogError("WebEPG: ChannelId: {0} grabber error - stopping", strChannelID);
            break;
          }
          this.LogInfo("WebEPG: ChannelId: {0} grabber error - continuing", strChannelID);
        }

        //_GrabDay++;
        if (_reqBuilder.HasDate()) // < here
        {
          _reqBuilder.AddDays(1);
          _timeControl.NewDay();
        }
        else
        {
          //if (_reqBuilder.HasList()) // < here
          break;
          //_reqBuilder.AddDays(_timeControl.GrabDay);
        }
      }

      return _programs;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Check the TV database for a program.
    /// </summary>
    /// <param name="title">The program title.</param>
    /// <param name="start">The program start time.</param>
    /// <returns>The program record from the TV database</returns>
    private Program dbProgram(string title, DateTime start)
    {
      if (_dbPrograms.Count > 0)
      {
        for (int i = _dbLastProg; i < _dbPrograms.Count; i++)
        {
          Program prog = (Program)_dbPrograms[i];

          if (prog.Title == title && prog.StartTime == start)
          {
            _dbLastProg = i;
            return prog;
          }
        }

        for (int i = 0; i < _dbLastProg; i++)
        {
          Program prog = (Program)_dbPrograms[i];

          if (prog.Title == title && prog.StartTime == start)
          {
            _dbLastProg = i;
            return prog;
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Gets the program given index number.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>the tv program data</returns>
    private ProgramData GetProgram(int index)
    {
      ProgramData guideData = (ProgramData)_parser.GetData(index);

      if (guideData == null ||
          guideData.StartTime == null || guideData.Title == string.Empty)
      {
        return null;
      }

      // Set ChannelId
      guideData.ChannelId = _strID;

      if (_grabber.Actions != null && guideData.IsRemoved(_grabber.Actions))
      {
        _discarded++;
        return null;
      }

      //this.LogDebug("WebEPG: Guide, Program title: {0}", guideData.Title);
      //this.LogDebug("WebEPG: Guide, Program start: {0}:{1} - {2}/{3}/{4}", guideData.StartTime.Hour, guideData.StartTime.Minute, guideData.StartTime.Day, guideData.StartTime.Month, guideData.StartTime.Year);
      //if (guideData.EndTime != null)
      //  this.LogDebug("WebEPG: Guide, Program end  : {0}:{1} - {2}/{3}/{4}", guideData.EndTime.Hour, guideData.EndTime.Minute, guideData.EndTime.Day, guideData.EndTime.Month, guideData.EndTime.Year);
      //this.LogDebug("WebEPG: Guide, Program desc.: {0}", guideData.Description);
      //this.LogDebug("WebEPG: Guide, Program genre: {0}", guideData.Genre);

      // Adjust Time
      if (guideData.StartTime.Day == 0 || guideData.StartTime.Month == 0 || guideData.StartTime.Year == 0)
      {
        if (!_timeControl.CheckAdjustTime(ref guideData))
        {
          _discarded++;
          return null;
        }
      }

      //Set TimeZone
      guideData.StartTime.TimeZone = _siteTimeZone;
      if (guideData.EndTime != null)
      {
        guideData.EndTime.TimeZone = _siteTimeZone;
        this.LogInfo("WebEPG: Guide, Program Info: {0} / {1} - {2}",
                     guideData.StartTime.ToLocalLongDateTime(), guideData.EndTime.ToLocalLongDateTime(), guideData.Title);
      }
      else
      {
        this.LogInfo("WebEPG: Guide, Program Info: {0} - {1}", guideData.StartTime.ToLocalLongDateTime(),
                     guideData.Title);
      }

      if (guideData.StartTime.ToLocalTime() < _grabStart.AddHours(-2))
      {
        this.LogInfo("WebEPG: Program starts in the past, ignoring it.");
        _discarded++;
        return null;
      }

      // Check TV db if program exists
      if (_dblookup)
      {
        Program dbProg = dbProgram(guideData.Title, guideData.StartTime.ToLocalTime());
        if (dbProg != null)
        {
          this.LogInfo("WebEPG: Program already in db");
          Channel chan = ChannelManagement.GetChannelByExternalId(_strID);
          if (chan != null)
          {
            var dbProgramData = new ProgramData();
            dbProgramData.InitFromProgram(dbProg);
            dbProgramData.ChannelId = _strID;
            return dbProgramData;
          }
        }
      }

      // SubLink
      if (guideData.HasSublink())
      {
        if (_parser is WebParser)
        {
          // added: delay info
          this.LogInfo("WebEPG: SubLink Request {0} - Delay: {1}ms", guideData.SublinkRequest.ToString(),
                       guideData.SublinkRequest.Delay);

          WebParser webParser = (WebParser)_parser;

          if (!webParser.GetLinkedData(ref guideData))
          {
            this.LogInfo("WebEPG: Getting sublinked data failed");
          }
          else
          {
            this.LogDebug("WebEPG: Getting sublinked data sucessful");
          }
        }
      }

      if (_grabber.Actions != null)
      {
        guideData.Replace(_grabber.Actions);
      }

      return guideData /*.ToTvProgram()*/;
    }

    /// <summary>
    /// Gets the channel listing.
    /// </summary>
    /// <param name="error">if set to <c>true</c> [error].</param>
    /// <returns>bool - more data exist</returns>
    private bool GetListing(out bool error)
    {
      int listingCount = 0;
      int programCount = 0;
      bool bMore = false;
      error = false;

      HTTPRequest request = _reqBuilder.GetRequest();

      // added: delay info
      this.LogInfo("WebEPG: Reading {0} - Delay: {1}ms", request.ToString(), request.Delay);

      listingCount = _parser.ParseUrl(request);

      if (listingCount == 0)
      {
        if (_grabber.Listing.SearchParameters.MaxListingCount == 0 ||
            (_grabber.Listing.SearchParameters.MaxListingCount != 0 && _reqBuilder.Offset == 0))
        {
          this.LogInfo("WebEPG: No Listings Found");
          //_reqBuilder.AddDays(1); -- removed because adding double days if continuing on error. 5/3/09
          error = true;
        }
        else
        {
          this.LogInfo("WebEPG: Listing Count 0");
        }
      }
      else
      {
        this.LogInfo("WebEPG: Listing Count {0}", listingCount);

        if (_reqBuilder.IsMaxListing(listingCount) || !_reqBuilder.IsLastPage())
        {
          bMore = true;
        }

        _discarded = 0;
        programCount = 0;
        _timeControl.SetProgramCount(listingCount);
        for (int i = 0; i < listingCount; i++)
        {
          ProgramData program = GetProgram(i);
          if (program != null)
          {
            _programs.Add(program);
            programCount++;
          }
        }

        this.LogDebug("WebEPG: Program Count ({0}), Listing Count ({1}), Discard Count ({2})", programCount,
                      listingCount, _discarded);
        if (programCount < (listingCount - _discarded))
        {
          this.LogInfo("WebEPG: Program Count ({0}) < Listing Count ({1}) - Discard Count ({2}), possible template error",
                       programCount, listingCount, _discarded);
        }

        if (_timeControl.GrabDay > _maxGrabDays)
        {
          bMore = false;
        }
      }

      return bMore;
    }

    #endregion
  }
}