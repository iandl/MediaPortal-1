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
using System.Windows.Forms;
using Mediaportal.TV.Server.TVDatabase.Entities.Enums;

namespace Mediaportal.TV.Server.SetupTV.Dialogs
{
  public partial class FormChooseTuningDetailType : Form
  {
    private MediaTypeEnum _mediaType = MediaTypeEnum.TV;
    private int _tuningType;

    public FormChooseTuningDetailType()
    {
      InitializeComponent();
    }

    public int TuningType
    {
      get { return _tuningType; }
    }

    public MediaTypeEnum MediaType
    {
      get { return _mediaType; }
      set { _mediaType = value; }
    }


    private void mpButtonOk_Click(object sender, EventArgs e)
    {
      _tuningType = GetSelectedTuningType();
      DialogResult = DialogResult.OK;
      Close();
    }

    private int GetSelectedTuningType()
    {
      if (mpRadioButton7.Checked)
      {
        return 0;
      }
      if (mpRadioButton1.Checked)
      {
        return 5;
      }
      if (mpRadioButton2.Checked)
      {
        return 1;
      }
      if (mpRadioButton3.Checked)
      {
        return 2;
      }
      if (mpRadioButton4.Checked)
      {
        return 3;
      }
      if (mpRadioButton5.Checked)
      {
        return 4;
      }
      if (mpRadioButton6.Checked)
      {
        return 7;
      }
      return -1;
    }

    private void mpButtonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void FormChooseTuningDetailType_Load(object sender, EventArgs e)
    {
      if (_mediaType != MediaTypeEnum.Radio)
      {
        mpRadioButton1.Visible = false;
      }
    }
  }
}