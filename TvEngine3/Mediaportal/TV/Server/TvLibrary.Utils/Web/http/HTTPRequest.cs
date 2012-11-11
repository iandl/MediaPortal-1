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
using System.Xml.Serialization;

namespace Mediaportal.TV.Server.TvLibrary.Utils.Web.http
{
  public class HTTPRequest
  {
    private string _agent = string.Empty;
    private string _cookies = string.Empty;
    private int _delay = 0;
    private string _encoding = string.Empty;
    private bool _externalBrowser = false;
    private string _getQuery = string.Empty;
    private string _host = string.Empty;
    private string _postQuery = string.Empty;
    private string _scheme = string.Empty;

    public HTTPRequest() {}

    public HTTPRequest(string baseUrl, string getQuery)
    {
      Uri baseUri = new Uri(baseUrl);
      Uri request = new Uri(baseUri, getQuery);
      BuildRequest(request);
    }

    public HTTPRequest(string baseUrl, string getQuery, string postQuery)
      : this(baseUrl, getQuery)
    {
      _postQuery = postQuery;
    }

    public HTTPRequest(string baseUrl, string getQuery, string postQuery, string encoding)
      : this(baseUrl, getQuery, postQuery)
    {
      _encoding = encoding;
    }

    public HTTPRequest(HTTPRequest request)
    {
      _scheme = request._scheme;
      _host = request._host;
      _getQuery = request._getQuery;
      _postQuery = request._postQuery;
      _cookies = request._cookies;
      _externalBrowser = request._externalBrowser;
      _encoding = request._encoding;
      _delay = request._delay;
      _agent = request._agent;
    }

    public HTTPRequest(Uri request)
    {
      BuildRequest(request);
    }

    public HTTPRequest(string uri)
    {
      Uri request = new Uri(uri);
      BuildRequest(request);
    }

    public string Host
    {
      get { return _host; }
    }

    public string GetQuery
    {
      get { return _getQuery; }
    }

    [XmlAttribute("url")]
    public string Url
    {
      set { BuildRequest(new Uri(value)); }
      get { return _scheme + Uri.SchemeDelimiter + _host + _getQuery; }
    }

    [XmlAttribute("post")]
    public string PostQuery
    {
      get { return _postQuery; }
      set { _postQuery = value; }
    }

    [XmlAttribute("external")]
    public bool External
    {
      get { return _externalBrowser; }
      set { _externalBrowser = value; }
    }

    [XmlAttribute("cookies")]
    public string Cookies
    {
      get { return _cookies; }
      set { _cookies = value; }
    }

    [XmlAttribute("encoding")]
    public string Encoding
    {
      get { return _encoding; }
      set { _encoding = value; }
    }

    [XmlAttribute("user-agent")]
    public string UserAgent
    {
      get { return _agent; }
      set { _agent = value; }
    }

    [XmlAttribute("delay")]
    public int Delay
    {
      get { return _delay; }
      set { _delay = value; }
    }

    public Uri Uri
    {
      get { return new Uri(Url); }
    }

    public Uri BaseUri
    {
      get { return new Uri(_scheme + Uri.SchemeDelimiter + _host); }
    }

    private void BuildRequest(Uri request)
    {
      _host = request.Authority;
      _scheme = request.Scheme;
      _getQuery = request.PathAndQuery;
      _getQuery = _getQuery.Replace("%5B", "[");
      _getQuery = _getQuery.Replace("%5D", "]");
    }

    // Add relative or absolute url
    public HTTPRequest Add(string relativeUri)
    {
      if (relativeUri.StartsWith("?"))
        relativeUri = Uri.LocalPath + relativeUri;
      Uri newUri = new Uri(Uri, relativeUri);
      HTTPRequest newHTTPRequest = new HTTPRequest(newUri);
      newHTTPRequest._encoding = _encoding;
      // Copy this also otherwise data is lost
      // Caused sublink delay in WebEPG always to be 0
      newHTTPRequest._externalBrowser = _externalBrowser;
      newHTTPRequest._cookies = _cookies;
      newHTTPRequest._delay = _delay;
      newHTTPRequest._agent = _agent;
      return newHTTPRequest;
    }

    public void ReplaceTag(string tag, string value)
    {
      _getQuery = _getQuery.Replace(tag, value);
      _postQuery = _postQuery.Replace(tag, value);
      _cookies = _cookies.Replace(tag, value);
    }

    public bool HasTag(string tag)
    {
      if (_getQuery.IndexOf(tag) != -1)
        return true;

      if (_postQuery.IndexOf(tag) != -1)
        return true;

      return false;
    }

    public override string ToString()
    {
      return Url + " POST: " + _postQuery;
    }

    public static bool operator ==(HTTPRequest r1, HTTPRequest r2)
    {
      if ((object)r1 == null || (object)r2 == null)
      {
        if ((object)r1 == null && (object)r2 == null)
          return true;
        return false;
      }
      return r1.Equals(r2);
    }

    public static bool operator !=(HTTPRequest r1, HTTPRequest r2)
    {
      return !(r1 == r2);
    }

    public override bool Equals(object obj)
    {
      HTTPRequest req = obj as HTTPRequest;
      if (req == null)
        return false;
      if (_scheme == req._scheme &&
          _host == req._host &&
          _getQuery == req._getQuery &&
          _postQuery == req._postQuery)
        return true;

      return false;
    }

    public override int GetHashCode()
    {
      return (_host + _getQuery + _scheme + _postQuery).GetHashCode();
    }
  }
}