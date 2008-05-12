#region Copyright (C) 2007-2008 Team MediaPortal

/*
    Copyright (C) 2007-2008 Team MediaPortal
    http://www.team-mediaportal.com
 
    This file is part of MediaPortal II

    MediaPortal II is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal II is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal II.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using MediaPortal.Presentation.Properties;
using SlimDX;
using Presentation.SkinEngine.MarkupExtensions;

namespace Presentation.SkinEngine.Controls.Animations
{
  public class PointAnimation : Timeline
  {
    Property _fromProperty;
    Property _toProperty;
    Property _byProperty;

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="PointAnimation"/> class.
    /// </summary>
    public PointAnimation()
    {
      Init();
    }

    public PointAnimation(PointAnimation a)
      : base(a)
    {
      Init();
      From = a.From;
      To = a.To;
      By = a.By;
    }

    public override object Clone()
    {
      PointAnimation result = new PointAnimation(this);
      BindingMarkupExtension.CopyBindings(this, result);
      return result;
    }

    void Init()
    {
      _fromProperty = new Property(typeof(Vector2), new Vector2(0, 0));
      _toProperty = new Property(typeof(Vector2), new Vector2(0, 0));
      _byProperty = new Property(typeof(Vector2), new Vector2(0, 0));

    }

    #endregion

    #region Public properties

    /// <summary>
    /// Gets or sets from property.
    /// </summary>
    /// <value>From property.</value>
    public Property FromProperty
    {
      get
      {
        return _fromProperty;
      }
      set
      {
        _fromProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets from.
    /// </summary>
    /// <value>From.</value>
    public Vector2 From
    {
      get
      {
        return (Vector2)_fromProperty.GetValue();
      }
      set
      {
        _fromProperty.SetValue(value);
      }
    }


    /// <summary>
    /// Gets or sets to property.
    /// </summary>
    /// <value>To property.</value>
    public Property ToProperty
    {
      get
      {
        return _toProperty;
      }
      set
      {
        _toProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets to.
    /// </summary>
    /// <value>To.</value>
    public Vector2 To
    {
      get
      {
        return (Vector2)_toProperty.GetValue();
      }
      set
      {
        _toProperty.SetValue(value);
      }
    }

    /// <summary>
    /// Gets or sets the by property.
    /// </summary>
    /// <value>The by property.</value>
    public Property ByProperty
    {
      get
      {
        return _byProperty;
      }
      set
      {
        _byProperty = value;
      }
    }

    /// <summary>
    /// Gets or sets the by.
    /// </summary>
    /// <value>The by.</value>
    public Vector2 By
    {
      get
      {
        return (Vector2)_byProperty.GetValue();
      }
      set
      {
        _byProperty.SetValue(value);
      }
    }

    #endregion

    #region Animation methods

    protected override void AnimateProperty(AnimationContext context, uint timepassed)
    {
      if (context.DataDescriptor == null) return;
      double distx = (To.X - From.X) / Duration.TotalMilliseconds;
      distx *= timepassed;
      distx += From.X;

      double disty = (To.X - From.Y) / Duration.TotalMilliseconds;
      disty *= timepassed;
      disty += From.Y;

      SetValue(context,new Vector2((float)distx, (float)disty));
    }

    public override void Ended(AnimationContext context)
    {
      if (IsStopped(context)) return;
      if (context.DataDescriptor != null)
        if (FillBehaviour != FillBehaviour.HoldEnd)
          SetValue(context, (Vector2)OriginalValue);
    }

    public override void Start(AnimationContext context, uint timePassed)
    {
      if (!IsStopped(context))
        Stop(context);

      context.State = State.Starting;

      context.TimeStarted = timePassed;
      context.State = State.WaitBegin;
    }

    public override void Stop(AnimationContext context)
    {
      if (IsStopped(context)) return;
      context.State = State.Idle;
      if (context.DataDescriptor != null)
      {
        SetValue(context, (Vector2)OriginalValue);
      }
    }

    Vector2 GetValue(AnimationContext context)
    {
      if (context.DataDescriptor == null) return new Vector2(0, 0);
      object o = context.DataDescriptor.Value;
      if (o.GetType() == typeof(Vector2)) return (Vector2)o;
      if (o.GetType() == typeof(Vector3))
      {
        Vector3 v = (Vector3)o;
        return new Vector2(v.X, v.Y);
      }
      return new Vector2(0, 0);

    }

    void SetValue(AnimationContext context,Vector2 vector)
    {
      if (context.DataDescriptor == null) return;
      object o = context.DataDescriptor.Value;
      if (o.GetType() == typeof(Vector2))
      {
        context.DataDescriptor.Value = vector;
        return;
      }
      if (o.GetType() == typeof(Vector3))
      {
        Vector3 v = new Vector3(vector.X, vector.Y, ((Vector3)o).Z);
        context.DataDescriptor.Value = v;
        return;
      }
    }

    #endregion
  }
}
