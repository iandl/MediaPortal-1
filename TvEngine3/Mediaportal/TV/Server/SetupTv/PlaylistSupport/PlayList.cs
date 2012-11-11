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
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Mediaportal.TV.Server.SetupTV.PlaylistSupport
{
  //public class PlayList : IEnumerable<PlayListItem>
  [Serializable]
  public class PlayList : IEnumerable<PlayListItem> //, IComparer
  {
    protected List<PlayListItem> _listPlayListItems = new List<PlayListItem>();
    protected string _playListName = "";

    public string Name
    {
      get { return _playListName; }
      set
      {
        if (value == null) return;
        _playListName = value;
      }
    }

    public int Count
    {
      get { return _listPlayListItems.Count; }
    }

    public PlayListItem this[int iItem]
    {
      get { return _listPlayListItems[iItem]; }
    }

    #region IEnumerable<PlayListItem> Members

    public IEnumerator<PlayListItem> GetEnumerator()
    {
      return _listPlayListItems.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      IEnumerable enumerable = _listPlayListItems;
      return enumerable.GetEnumerator();
    }

    #endregion

    public bool AllPlayed()
    {
      foreach (PlayListItem item in _listPlayListItems)
      {
        if (!item.Played) return false;
      }
      return true;
    }

    public void ResetStatus()
    {
      foreach (PlayListItem item in _listPlayListItems)
      {
        item.Played = false;
      }
    }

    public void Add(PlayListItem item)
    {
      if (item == null) return;
      //this.LogDebug("Playlist: add {0}", item.FileName);
      _listPlayListItems.Add(item);
    }

    public bool Insert(PlayListItem item, int currentSong)
    {
      if (item == null)
        return false;

      if (currentSong < _listPlayListItems.Count)
      {
        _listPlayListItems.Insert(currentSong + 1, item);
      }
      else
      {
        _listPlayListItems.Add(item);
      }
      return true;
    }

    public bool Insert(PlayListItem item, PlayListItem afterThisItem)
    {
      bool success = false;
      if (item == null)
        return false;

      for (int i = 0; i < _listPlayListItems.Count; ++i)
      {
        if (afterThisItem.FileName == _listPlayListItems[i].FileName)
        {
          _listPlayListItems.Insert(i + 1, item);
          success = true;
        }
      }
      return success;
    }

    public int Remove(string fileName)
    {
      if (fileName == null) return -1;

      for (int i = 0; i < _listPlayListItems.Count; ++i)
      {
        PlayListItem item = _listPlayListItems[i];
        if (item.FileName == fileName)
        {
          _listPlayListItems.RemoveAt(i);
          return i;
        }
      }
      return -1;
    }

    public void Clear()
    {
      _listPlayListItems.Clear();
    }


    public int RemoveDVDItems()
    {
      //TODO
      return 0;
    }

    public virtual void Shuffle()
    {
      var r = new Random(DateTime.Now.Millisecond);

      // iterate through each catalogue item performing arbitrary swaps
      for (int item = 0; item < Count; item++)
      {
        int nArbitrary = r.Next(Count);

        PlayListItem anItem = _listPlayListItems[nArbitrary];
        _listPlayListItems[nArbitrary] = _listPlayListItems[item];
        _listPlayListItems[item] = anItem;
      }
    }

    public int MovePlayListItemUp(int iItem)
    {
      int selectedItemIndex;

      if (iItem < 0 || iItem >= _listPlayListItems.Count)
        return -1;

      int iPreviousItem = iItem - 1;

      if (iPreviousItem < 0)
        iPreviousItem = _listPlayListItems.Count - 1;

      PlayListItem playListItem1 = _listPlayListItems[iItem];
      PlayListItem playListItem2 = _listPlayListItems[iPreviousItem];

      if (playListItem1 == null || playListItem2 == null)
        return -1;

      try
      {
        Monitor.Enter(this);
        _listPlayListItems[iItem] = playListItem2;
        _listPlayListItems[iPreviousItem] = playListItem1;
        selectedItemIndex = iPreviousItem;
      }

      catch (Exception)
      {
        selectedItemIndex = -1;
      }

      finally
      {
        Monitor.Exit(this);
      }

      return selectedItemIndex;
    }

    public int MovePlayListItemDown(int iItem)
    {
      int selectedItemIndex;

      if (iItem < 0 || iItem >= _listPlayListItems.Count)
        return -1;

      int iNextItem = iItem + 1;

      if (iNextItem >= _listPlayListItems.Count)
        iNextItem = 0;

      PlayListItem playListItem1 = _listPlayListItems[iItem];
      PlayListItem playListItem2 = _listPlayListItems[iNextItem];

      if (playListItem1 == null || playListItem2 == null)
        return -1;

      try
      {
        Monitor.Enter(this);
        _listPlayListItems[iItem] = playListItem2;
        _listPlayListItems[iNextItem] = playListItem1;
        selectedItemIndex = iNextItem;
      }

      catch (Exception)
      {
        selectedItemIndex = -1;
      }

      finally
      {
        Monitor.Exit(this);
      }

      return selectedItemIndex;
    }
  }
}