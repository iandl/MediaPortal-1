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
using System.IO;
using System.Text;
using System.Xml;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Implementations.Analog.GraphComponents;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Interfaces;
using Mediaportal.TV.Server.TVLibrary.Interfaces.Logging;

namespace Mediaportal.TV.Server.TVLibrary.Interfaces.Implementations.Analog
{
  /// <summary>
  /// Configuration object for the web server
  /// </summary>
  public class Configuration
  {
    #region variables

    #endregion

    #region ctor

    /// <summary>
    /// Simple constructor
    /// </summary>
    private Configuration()
    {
      PlaybackQualityMode = VIDEOENCODER_BITRATE_MODE.ConstantBitRate;
      RecordQualityMode = VIDEOENCODER_BITRATE_MODE.ConstantBitRate;
      PlaybackQualityType = QualityType.Default;
      RecordQualityType = QualityType.Default;
      CustomQualityValue = 50;
      CustomPeakQualityValue = 75;
    }

    #endregion

    #region properties

    ///<summary>
    /// Gets/Sets the custom quality value
    ///</summary>
    public int CustomQualityValue { get; set; }

    /// <summary>
    /// Gets/Sets the custom peak quality value
    /// </summary>
    public int CustomPeakQualityValue { get; set; }

    /// <summary>
    /// Gets/Sets the playback quality type
    /// </summary>
    public QualityType PlaybackQualityType { get; set; }

    /// <summary>
    /// Gets/Sets the record quality type
    /// </summary>
    public QualityType RecordQualityType { get; set; }

    /// <summary>
    /// Gets/Sets the playback quality mode
    /// </summary>
    public VIDEOENCODER_BITRATE_MODE PlaybackQualityMode { get; set; }

    /// <summary>
    /// Gets/Sets the record quality mode
    /// </summary>
    public VIDEOENCODER_BITRATE_MODE RecordQualityMode { get; set; }

    /// <summary>
    /// Gets/Sets the card name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets/Sets the device path
    /// </summary>
    public string DevicePath { get; set; }

    /// <summary>
    /// Gets/Sets the card id
    /// </summary>
    public int CardId { get; set; }

    /// <summary>
    /// Gets/Sets the graph
    /// </summary>
    public Graph Graph { get; set; }

    #endregion

    #region Read/Write methods

    /// <summary>
    /// Loads the configuration from a xml file
    /// </summary>
    /// <param name="name">Name of the card</param>
    /// <param name="cardId">Unique id of the card</param>
    /// <param name="devicePath">The device path of the card</param>
    /// <returns>Configuration object</returns>
    public static Configuration readConfiguration(int cardId, string name, string devicePath)
    {
      Configuration _configuration = new Configuration();
      String fileName = GetFileName(name, cardId);
      _configuration.Name = name;
      _configuration.DevicePath = devicePath;
      _configuration.CardId = cardId;
      if (cardId != 0 && File.Exists(fileName))
      {
        try
        {
          XmlDocument doc = new XmlDocument();
          doc.Load(fileName);
          if (doc.DocumentElement != null)
          {
            XmlNode cardNode = doc.DocumentElement.SelectSingleNode("/configuration/card");
            _configuration.CardId = int.Parse(cardNode.Attributes["cardId"].Value);
            if (_configuration.CardId != cardId)
            {
              File.Delete(fileName);
              _configuration = new Configuration();
              _configuration.Name = name;
              _configuration.DevicePath = devicePath;
              _configuration.CardId = cardId;
              return _configuration;
            }
            _configuration.Name = cardNode.Attributes["name"].Value;
            if (_configuration.Name != name)
            {
              File.Delete(fileName);
              _configuration = new Configuration();
              _configuration.Name = name;
              _configuration.DevicePath = devicePath;
              _configuration.CardId = cardId;
              return _configuration;
            }
            XmlNode node = cardNode.SelectSingleNode("device/path");
            _configuration.DevicePath = node.InnerText;
            if (!_configuration.DevicePath.Equals(devicePath))
            {
              File.Delete(fileName);
              _configuration = new Configuration();
              _configuration.Name = name;
              _configuration.DevicePath = devicePath;
              _configuration.CardId = cardId;
              return _configuration;
            }

            node = cardNode.SelectSingleNode("qualityControl");
            XmlNode tempNode = node.SelectSingleNode("customSettings");
            _configuration.CustomQualityValue = int.Parse(tempNode.Attributes["value"].Value);
            _configuration.CustomPeakQualityValue = int.Parse(tempNode.Attributes["peakValue"].Value);
            tempNode = node.SelectSingleNode("playback");
            int tempValue = int.Parse(tempNode.Attributes["mode"].Value);
            if (tempValue < 0 || tempValue > 2)
            {
              tempValue = 0;
            }
            _configuration.PlaybackQualityMode = (VIDEOENCODER_BITRATE_MODE)tempValue;
            tempValue = int.Parse(tempNode.Attributes["type"].Value);
            if (tempValue < 1 || tempValue > 6)
            {
              tempValue = 1;
            }
            _configuration.PlaybackQualityType = (QualityType)tempValue;
            tempNode = node.SelectSingleNode("record");
            tempValue = int.Parse(tempNode.Attributes["mode"].Value);
            if (tempValue < 0 || tempValue > 2)
            {
              tempValue = 0;
            }
            _configuration.RecordQualityMode = (VIDEOENCODER_BITRATE_MODE)tempValue;
            tempValue = int.Parse(tempNode.Attributes["type"].Value);
            if (tempValue < 1 || tempValue > 6)
            {
              tempValue = 1;
            }
            _configuration.RecordQualityType = (QualityType)tempValue;
            _configuration.Graph = Graph.CreateInstance(cardNode.SelectSingleNode("graph"));
          }
        }
        catch
        {
          Log.Debug("Error while reading analog card configuration file");
          _configuration = new Configuration();
          _configuration.Name = name;
          _configuration.DevicePath = devicePath;
          _configuration.CardId = cardId;
          _configuration.Graph = Graph.CreateInstance(null);
        }
      }
      else
      {
        _configuration.Graph = Graph.CreateInstance(null);
      }
      return _configuration;
    }

    /// <summary>
    /// Saves the configuration object in a xml file
    /// </summary>
    /// <param name="configuration">Configuration object to be saved</param>
    public static void writeConfiguration(Configuration configuration)
    {
      if (configuration != null && configuration.CardId != 0)
      {
        String fileName = GetFileName(configuration.Name, configuration.CardId);
        using (var writer = new XmlTextWriter(fileName, Encoding.UTF8)) 
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char)9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("configuration"); //<configuration>
          writer.WriteAttributeString("version", "2");
          writer.WriteStartElement("card"); //<card>
          writer.WriteAttributeString("cardId", XmlConvert.ToString(configuration.CardId));
          writer.WriteAttributeString("name", configuration.Name);
          writer.WriteStartElement("device"); //<device>
          writer.WriteElementString("path", configuration.DevicePath);
          writer.WriteEndElement(); //</device>
          configuration.Graph.WriteGraph(writer);
          writer.WriteStartElement("qualityControl"); //<qualityControl>
          writer.WriteStartElement("customSettings"); //<customSettings>
          writer.WriteAttributeString("value", XmlConvert.ToString(configuration.CustomQualityValue));
          writer.WriteAttributeString("peakValue", XmlConvert.ToString(configuration.CustomPeakQualityValue));
          writer.WriteEndElement(); //</customSettings>
          writer.WriteStartElement("playback"); //<playback>
          writer.WriteAttributeString("mode", XmlConvert.ToString((int)configuration.PlaybackQualityMode));
          writer.WriteAttributeString("type", XmlConvert.ToString((int)configuration.PlaybackQualityType));
          writer.WriteEndElement(); //</playback>
          writer.WriteStartElement("record"); //<record>
          writer.WriteAttributeString("mode", XmlConvert.ToString((int)configuration.RecordQualityMode));
          writer.WriteAttributeString("type", XmlConvert.ToString((int)configuration.RecordQualityType));
          writer.WriteEndElement(); //</record>
          writer.WriteEndElement(); //</qualityControl>
          writer.WriteEndElement(); //</card>
          writer.WriteEndElement(); //</configuration>
          writer.WriteEndDocument();
        }
      }
    }

    #endregion

    #region private helper

    /// <summary>
    /// Generates the file and pathname of the configuration file
    /// </summary>
    /// <param name="name">Name of the card</param>
    /// <param name="cardId">Unique id of the card</param>
    /// <returns>Complete filename of the configuration file</returns>
    private static String GetFileName(string name, int cardId)
    {
      if (!string.IsNullOrEmpty(name))
      {
        foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
        {
          name = name.Replace(invalidCharacter.ToString(), "");
        }
      }
      String pathName = PathManager.GetDataPath;
      String fileName = String.Format(@"{0}\AnalogCard\Configuration-{1}-{2}.xml", pathName, cardId, name);
      Directory.CreateDirectory(Path.GetDirectoryName(fileName));
      return fileName;
    }

    #endregion
  }
}