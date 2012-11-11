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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Mediaportal.TV.Server.SetupControls.UserInterfaceControls
{

  #region Formattable TextBox Column

  public class FormattableTextBoxColumn : DataGridTextBoxColumn
  {
    public event FormatCellEventHandler SetCellFormat;

    //used to fire an event to retrieve formatting info
    //and then draw the cell with this formatting info
    protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush,
                                  Brush foreBrush, bool alignToRight)
    {
      DataGridFormatCellEventArgs e = null;

      bool callBaseClass = true;

      //fire the formatting event
      if (SetCellFormat != null)
      {
        int col = DataGridTableStyle.GridColumnStyles.IndexOf(this);
        e = new DataGridFormatCellEventArgs(rowNum, col, GetColumnValueAtRow(source, rowNum));
        SetCellFormat(this, e);
        if (e.BackBrush != null)
          backBrush = e.BackBrush;

        //if these properties set, then must call drawstring
        if (e.ForeBrush != null || e.TextFont != null)
        {
          if (e.ForeBrush == null)
            e.ForeBrush = foreBrush;
          if (e.TextFont == null)
            e.TextFont = DataGridTableStyle.DataGrid.Font;
          g.FillRectangle(backBrush, bounds);
          Region saveRegion = g.Clip;
          Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
          using (Region newRegion = new Region(rect))
          {
            g.Clip = newRegion;
            int charWidth =
              (int)Math.Ceiling(g.MeasureString("c", e.TextFont, 20, StringFormat.GenericTypographic).Width);

            string s = GetColumnValueAtRow(source, rowNum).ToString();
            int maxChars = Math.Min(s.Length, (bounds.Width / charWidth));

            try
            {
              g.DrawString(s.Substring(0, maxChars), e.TextFont, e.ForeBrush, bounds.X, bounds.Y + 2);
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
            } //empty catch
            finally
            {
              g.Clip = saveRegion;
            }
          }
          callBaseClass = false;
        }

        if (!e.UseBaseClassDrawing)
        {
          callBaseClass = false;
        }
      }
      if (callBaseClass)
        base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);

      //clean up
      if (e != null)
      {
        if (e.BackBrushDispose)
          if (e.BackBrush != null)
            e.BackBrush.Dispose();
        if (e.ForeBrushDispose)
          if (e.ForeBrush != null)
            e.ForeBrush.Dispose();
        if (e.TextFontDispose)
          if (e.TextFont != null)
            e.TextFont.Dispose();
      }
    }
  }

  #endregion

  #region Formattable Bool Column

  public class FormattableBooleanColumn : DataGridBoolColumn
  {
    public const int VK_SPACE = 32; // 0x20
    private bool beingEdited;
    private bool lockValue;
    private int saveRow = -1;
    private bool saveValue;

    public FormattableBooleanColumn()
    {
      AllowNull = false;
    }

    public event FormatCellEventHandler SetCellFormat;

    //overridden to fire BoolChange event and Formatting event
    protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush,
                                  Brush foreBrush, bool alignToRight)
    {
      int colNum = DataGridTableStyle.GridColumnStyles.IndexOf(this);

      //used to handle the boolchanging
      ManageBoolValueChanging(rowNum, colNum);

      //fire formatting event
      DataGridFormatCellEventArgs e = null;
      bool callBaseClass = true;
      if (SetCellFormat != null)
      {
        e = new DataGridFormatCellEventArgs(rowNum, colNum, GetColumnValueAtRow(source, rowNum));
        SetCellFormat(this, e);
        if (e.BackBrush != null)
          backBrush = e.BackBrush;
        callBaseClass = e.UseBaseClassDrawing;
      }
      if (callBaseClass)
        base.Paint(g, bounds, source, rowNum, backBrush, new SolidBrush(Color.Red), alignToRight);

      //clean up
      if (e != null)
      {
        if (e.BackBrushDispose)
          if (e.BackBrush != null)
            e.BackBrush.Dispose();
        if (e.ForeBrushDispose)
          e.ForeBrush.Dispose();
        if (e.TextFontDispose)
          e.TextFont.Dispose();
      }
    }

    //changed event
    public event BoolValueChangedEventHandler BoolValueChanged;

    //needed to get the space bar changing of the bool value
    [DllImport("user32.dll")]
    private static extern short GetKeyState(int nVirtKey);

    //set variables to start tracking bool changes
    protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText,
                                 bool cellIsVisible)
    {
      lockValue = true;
      beingEdited = true;
      saveRow = rowNum;
      saveValue = (bool)base.GetColumnValueAtRow(source, rowNum);
      base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
    }

    //turn off tracking bool changes
    protected override bool Commit(CurrencyManager dataSource, int rowNum)
    {
      lockValue = true;
      beingEdited = false;
      return base.Commit(dataSource, rowNum);
    }

    //fire the bool change event if the value changes
    private void ManageBoolValueChanging(int rowNum, int colNum)
    {
      Point mousePos = DataGridTableStyle.DataGrid.PointToClient(Control.MousePosition);
      DataGrid dg = DataGridTableStyle.DataGrid;
      bool isClickInCell = ((Control.MouseButtons == MouseButtons.Left) &&
                            dg.GetCellBounds(dg.CurrentCell).Contains(mousePos));

      bool changing = dg.Focused && (isClickInCell
                                     || GetKeyState(VK_SPACE) < 0); // or spacebar

      if (!lockValue && beingEdited && changing && saveRow == rowNum)
      {
        saveValue = !saveValue;
        lockValue = false;

        //fire the event
        if (BoolValueChanged != null)
        {
          BoolValueChangedEventArgs e = new BoolValueChangedEventArgs(rowNum, colNum, saveValue);
          BoolValueChanged(this, e);
        }
      }
      if (saveRow == rowNum)
        lockValue = false;
    }
  }

  #endregion

  #region CellFormatting Event

  public delegate void FormatCellEventHandler(object sender, DataGridFormatCellEventArgs e);

  public class DataGridFormatCellEventArgs : EventArgs
  {
    private readonly object currentCellValue;

    public DataGridFormatCellEventArgs(int row, int col, object cellValue)
    {
      Row = row;
      Column = col;
      TextFont = null;
      BackBrush = null;
      ForeBrush = null;
      TextFontDispose = false;
      BackBrushDispose = false;
      ForeBrushDispose = false;
      UseBaseClassDrawing = true;
      currentCellValue = cellValue;
    }


    //column being painted
    public int Column { get; set; }

    //row being painted
    public int Row { get; set; }

    //font used for drawing the text
    public Font TextFont { get; set; }

    //background brush
    public Brush BackBrush { get; set; }

    //foreground brush
    public Brush ForeBrush { get; set; }

    //set true if you want the Paint method to call Dispose on the font
    public bool TextFontDispose { get; set; }

    //set true if you want the Paint method to call Dispose on the brush
    public bool BackBrushDispose { get; set; }

    //set true if you want the Paint method to call Dispose on the brush
    public bool ForeBrushDispose { get; set; }

    //set true if you want the Paint method to call base class
    public bool UseBaseClassDrawing { get; set; }

    //contains the current cell value
    public object CurrentCellValue
    {
      get { return currentCellValue; }
    }
  }

  #endregion

  #region BoolValueChanging Event

  public delegate void BoolValueChangedEventHandler(object sender, BoolValueChangedEventArgs e);

  public class BoolValueChangedEventArgs : EventArgs
  {
    private readonly bool boolVal;

    public BoolValueChangedEventArgs(int row, int col, bool val)
    {
      Row = row;
      Column = col;
      boolVal = val;
    }

    //column to be painted
    public int Column { get; set; }

    //row to be painted
    public int Row { get; set; }

    //current value to be painted
    public bool BoolValue
    {
      get { return boolVal; }
    }
  }

  #endregion
}