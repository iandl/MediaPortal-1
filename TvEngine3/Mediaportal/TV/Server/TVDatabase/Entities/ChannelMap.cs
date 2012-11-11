//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Card))]
    [KnownType(typeof(Channel))]
    public partial class ChannelMap: IObjectWithChangeTracker, INotifyPropertyChanged
    {
      #region Primitive Properties

      private bool _epgOnly;
      private int _idCard;
      private int _idChannel;
      private int _idChannelMap;

      [DataMember]
      public int IdChannelMap
      {
        get { return _idChannelMap; }
        set
        {
          if (_idChannelMap != value)
          {
            if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
            {
              throw new InvalidOperationException("The property 'IdChannelMap' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
            }
            _idChannelMap = value;
            OnPropertyChanged("IdChannelMap");
          }
        }
      }

      [DataMember]
      public int IdChannel
      {
        get { return _idChannel; }
        set
        {
          if (_idChannel != value)
          {
            ChangeTracker.RecordOriginalValue("IdChannel", _idChannel);
            if (!IsDeserializing)
            {
              if (Channel != null && Channel.IdChannel != value)
              {
                Channel = null;
              }
            }
            _idChannel = value;
            OnPropertyChanged("IdChannel");
          }
        }
      }

      [DataMember]
      public int IdCard
      {
        get { return _idCard; }
        set
        {
          if (_idCard != value)
          {
            ChangeTracker.RecordOriginalValue("IdCard", _idCard);
            if (!IsDeserializing)
            {
              if (Card != null && Card.IdCard != value)
              {
                Card = null;
              }
            }
            _idCard = value;
            OnPropertyChanged("IdCard");
          }
        }
      }

      [DataMember]
      public bool EpgOnly
      {
        get { return _epgOnly; }
        set
        {
          if (_epgOnly != value)
          {
            _epgOnly = value;
            OnPropertyChanged("EpgOnly");
          }
        }
      }

      #endregion

      #region Navigation Properties

      private Card _card;

      private Channel _channel;

      [DataMember]
      public Card Card
      {
        get { return _card; }
        set
        {
          if (!ReferenceEquals(_card, value))
          {
            var previousValue = _card;
            _card = value;
            FixupCard(previousValue);
            OnNavigationPropertyChanged("Card");
          }
        }
      }

      [DataMember]
      public Channel Channel
      {
        get { return _channel; }
        set
        {
          if (!ReferenceEquals(_channel, value))
          {
            var previousValue = _channel;
            _channel = value;
            FixupChannel(previousValue);
            OnNavigationPropertyChanged("Channel");
          }
        }
      }

      #endregion

      #region ChangeTracking

      private ObjectChangeTracker _changeTracker;
      protected bool IsDeserializing { get; private set; }

      #region INotifyPropertyChanged Members

      event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }

      #endregion

      #region IObjectWithChangeTracker Members

      [DataMember]
      public ObjectChangeTracker ChangeTracker
      {
        get
        {
          if (_changeTracker == null)
          {
            _changeTracker = new ObjectChangeTracker();
            _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
          }
          return _changeTracker;
        }
        set
        {
          if(_changeTracker != null)
          {
            _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
          }
          _changeTracker = value;
          if(_changeTracker != null)
          {
            _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
          }
        }
      }

      #endregion

      protected virtual void OnPropertyChanged(String propertyName)
      {
        if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
        {
          ChangeTracker.State = ObjectState.Modified;
        }
        if (_propertyChanged != null)
        {
          _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      }
    
      protected virtual void OnNavigationPropertyChanged(String propertyName)
      {
        if (_propertyChanged != null)
        {
          _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      }

      private event PropertyChangedEventHandler _propertyChanged;

      private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
      {
        if (e.NewState == ObjectState.Deleted)
        {
          ClearNavigationProperties();
        }
      }
    
      // This entity type is the dependent end in at least one association that performs cascade deletes.
      // This event handler will process notifications that occur when the principal end is deleted.
      internal void HandleCascadeDelete(object sender, ObjectStateChangingEventArgs e)
      {
        if (e.NewState == ObjectState.Deleted)
        {
          this.MarkAsDeleted();
        }
      }

      [OnDeserializing]
      public void OnDeserializingMethod(StreamingContext context)
      {
        IsDeserializing = true;
      }
    
      [OnDeserialized]
      public void OnDeserializedMethod(StreamingContext context)
      {
        IsDeserializing = false;
        ChangeTracker.ChangeTrackingEnabled = true;
      }
    
      protected virtual void ClearNavigationProperties()
      {
        Card = null;
        Channel = null;
      }

      #endregion

      #region Association Fixup
    
      private void FixupCard(Card previousValue)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (previousValue != null && previousValue.ChannelMaps.Contains(this))
        {
          previousValue.ChannelMaps.Remove(this);
        }
    
        if (Card != null)
        {
          if (!Card.ChannelMaps.Contains(this))
          {
            Card.ChannelMaps.Add(this);
          }
    
          IdCard = Card.IdCard;
        }
        if (ChangeTracker.ChangeTrackingEnabled)
        {
          if (ChangeTracker.OriginalValues.ContainsKey("Card")
              && (ChangeTracker.OriginalValues["Card"] == Card))
          {
            ChangeTracker.OriginalValues.Remove("Card");
          }
          else
          {
            ChangeTracker.RecordOriginalValue("Card", previousValue);
          }
          if (Card != null && !Card.ChangeTracker.ChangeTrackingEnabled)
          {
            Card.StartTracking();
          }
        }
      }
    
      private void FixupChannel(Channel previousValue)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (previousValue != null && previousValue.ChannelMaps.Contains(this))
        {
          previousValue.ChannelMaps.Remove(this);
        }
    
        if (Channel != null)
        {
          if (!Channel.ChannelMaps.Contains(this))
          {
            Channel.ChannelMaps.Add(this);
          }
    
          IdChannel = Channel.IdChannel;
        }
        if (ChangeTracker.ChangeTrackingEnabled)
        {
          if (ChangeTracker.OriginalValues.ContainsKey("Channel")
              && (ChangeTracker.OriginalValues["Channel"] == Channel))
          {
            ChangeTracker.OriginalValues.Remove("Channel");
          }
          else
          {
            ChangeTracker.RecordOriginalValue("Channel", previousValue);
          }
          if (Channel != null && !Channel.ChangeTracker.ChangeTrackingEnabled)
          {
            Channel.StartTracking();
          }
        }
      }

      #endregion
    }
}
