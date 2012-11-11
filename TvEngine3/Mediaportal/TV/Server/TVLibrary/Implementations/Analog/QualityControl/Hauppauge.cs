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
using System.Runtime.InteropServices;
using System.Text;
using DirectShowLib;
using Mediaportal.TV.Server.TVLibrary.Interfaces;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;

namespace Mediaportal.TV.Server.TVLibrary.Implementations.Analog.QualityControl
{
  ///<summary>
  /// Hauppauge quality control
  ///</summary>
  public class Hauppauge : IDisposable
  {
    private readonly DeInit _DeInit;
    private readonly GetAudBitRate _GetAudBitRate;
    private readonly GetStreamType _GetStreamType;
    private readonly GetVidBitRate _GetVidBitRate;
    private readonly Init _Init;
    private readonly IsHauppauge _IsHauppauge;
    private readonly SetAudBitRate _SetAudBitRate;
    private readonly SetDNRFilter _SetDNRFilter;
    private readonly SetStreamType _SetStreamType;
    private readonly SetVidBitRate _SetVidBitRate;

    private readonly HResult hr;
    private bool disposed;
    private IntPtr hauppaugelib = IntPtr.Zero;

    //Initializes the Hauppauge interfaces

    /// <summary>
    /// Constructor: Require the Hauppauge capture filter, and the deviceid for the card to be passed in
    /// </summary>
    public Hauppauge(IBaseFilter filter, string tuner)
    {
      try
      {
        //Don't create the class if we don't have any filter;

        if (filter == null)
        {
          return;
        }

        //Load Library
        hauppaugelib = LoadLibrary("hauppauge.dll");

        //Get Proc addresses, and set the delegates for each function
        IntPtr procaddr = GetProcAddress(hauppaugelib, "Init");
        _Init = (Init)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (Init));

        procaddr = GetProcAddress(hauppaugelib, "DeInit");
        _DeInit = (DeInit)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (DeInit));

        procaddr = GetProcAddress(hauppaugelib, "IsHauppauge");
        _IsHauppauge = (IsHauppauge)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (IsHauppauge));

        procaddr = GetProcAddress(hauppaugelib, "SetVidBitRate");
        _SetVidBitRate = (SetVidBitRate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (SetVidBitRate));

        procaddr = GetProcAddress(hauppaugelib, "GetVidBitRate");
        _GetVidBitRate = (GetVidBitRate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (GetVidBitRate));

        procaddr = GetProcAddress(hauppaugelib, "SetAudBitRate");
        _SetAudBitRate = (SetAudBitRate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (SetAudBitRate));

        procaddr = GetProcAddress(hauppaugelib, "GetAudBitRate");
        _GetAudBitRate = (GetAudBitRate)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (GetAudBitRate));

        procaddr = GetProcAddress(hauppaugelib, "SetStreamType");
        _SetStreamType = (SetStreamType)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (SetStreamType));

        procaddr = GetProcAddress(hauppaugelib, "GetStreamType");
        _GetStreamType = (GetStreamType)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (GetStreamType));

        procaddr = GetProcAddress(hauppaugelib, "SetDNRFilter");
        _SetDNRFilter = (SetDNRFilter)Marshal.GetDelegateForFunctionPointer(procaddr, typeof (SetDNRFilter));

        //Hack
        //The following is strangely necessary when using delegates instead of P/Invoke - linked to MP using utf-8
        //Hack

        byte[] encodedstring = Encoding.UTF32.GetBytes(tuner);
        string card = Encoding.Unicode.GetString(encodedstring);

        hr = new HResult(_Init(filter, card));
        this.LogDebug("Hauppauge Quality Control Initializing " + hr.ToDXString());
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Init failed");
      }
    }

    #region IDisposable Members

    /// <summary>
    /// Deallocate Hauppauge class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    [DllImport("kernel32.dll")]
    internal static extern IntPtr LoadLibrary(String dllname);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, String procname);

    [DllImport("kernel32.dll")]
    internal static extern bool FreeLibrary(IntPtr hModule);

    /// <summary>
    /// Toggles Dynamic Noise Reduction on/off
    /// </summary>
    public bool SetDNR(bool onoff)
    {
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _SetDNRFilter(onoff);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge SetDNR failed");
      }
      return false;
    }

    /// <summary>
    /// Get the video bit rate
    /// </summary>
    public bool GetVideoBitRate(out int minKbps, out int maxKbps, out bool isVBR)
    {
      maxKbps = minKbps = -1;
      isVBR = false;
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _GetVidBitRate(out maxKbps, out minKbps, out isVBR);
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Error GetBitrate");
      }

      return true;
    }

    /// <summary>
    /// Sets the video bit rate
    /// </summary>
    public bool SetVideoBitRate(int minKbps, int maxKbps, bool isVBR)
    {
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            hr.Set(_SetVidBitRate(maxKbps, minKbps, isVBR));
            this.LogDebug("Hauppauge Set Bit Rate " + hr.ToDXString());
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Set Vid Rate");
      }
      return false;
    }

    /// <summary>
    /// Get the audio bit rate
    /// </summary>
    public bool GetAudioBitRate(out int Kbps)
    {
      Kbps = -1;
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _GetAudBitRate(out Kbps);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Get Audio Bitrate");
      }
      return false;
    }

    /// <summary>
    /// Set the audio bit rate
    /// </summary>
    public bool SetAudioBitRate(int Kbps)
    {
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _SetAudBitRate(Kbps);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Set Audio Bit Rate");
      }
      return false;
    }

    /// <summary>
    /// Get the stream type
    /// </summary>
    public bool GetStream(out int stream)
    {
      stream = -1;
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _GetStreamType(out stream);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Get Stream");
      }
      return false;
    }

    /// <summary>
    /// Set the stream type
    /// </summary>
    public bool SetStream(int stream)
    {
      try
      {
        if (hauppaugelib != IntPtr.Zero)
        {
          if (_IsHauppauge())
          {
            _SetStreamType(103);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LogError(ex, "Hauppauge Set Stream Type");
      }
      return false;
    }

    /// <summary>
    /// Deallocate Hauppauge class
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          // Dispose managed resources if any
        }
        try
        {
          if (hauppaugelib != IntPtr.Zero)
          {
            if (_IsHauppauge())
              _DeInit();

            FreeLibrary(hauppaugelib);
            hauppaugelib = IntPtr.Zero;
          }
        }
        catch (Exception ex)
        {
          this.LogError(ex, "Hauppauge exception");
          this.LogError("Hauppauge Disposed hcw.txt");
        }
      }
      disposed = true;
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~Hauppauge()
    {
      Dispose(false);
    }

    #region Nested type: DeInit

    private delegate int DeInit();

    #endregion

    #region Nested type: GetAudBitRate

    private delegate int GetAudBitRate(out int bitrate);

    #endregion

    #region Nested type: GetStreamType

    private delegate int GetStreamType(out int stream);

    #endregion

    #region Nested type: GetVidBitRate

    private delegate int GetVidBitRate(out int maxkbps, out int minkbps, out bool isVBR);

    #endregion

    #region Nested type: Init

    private delegate int Init(IBaseFilter capture, [MarshalAs(UnmanagedType.LPStr)] string tuner);

    #endregion

    #region Nested type: IsHauppauge

    private delegate bool IsHauppauge();

    #endregion

    #region Nested type: SetAudBitRate

    private delegate int SetAudBitRate(int bitrate);

    #endregion

    #region Nested type: SetDNRFilter

    private delegate int SetDNRFilter(bool onoff);

    #endregion

    #region Nested type: SetStreamType

    private delegate int SetStreamType(int stream);

    #endregion

    #region Nested type: SetVidBitRate

    private delegate int SetVidBitRate(int maxkbps, int minkbps, bool isVBR);

    #endregion
  }
}