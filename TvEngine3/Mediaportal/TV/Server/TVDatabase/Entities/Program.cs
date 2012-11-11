//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Mediaportal.TV.Server.TVDatabase.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Channel))]
    [KnownType(typeof(ProgramCategory))]
    [KnownType(typeof(ProgramCredit))]
    [KnownType(typeof(PersonalTVGuideMap))]
    public partial class Program: IObjectWithChangeTracker, INotifyPropertyChanged
    {
      #region Primitive Properties

      private string _classification;
      private string _description;
      private System.DateTime _endTime;
      private short _endTimeDayOfWeek;
      private System.DateTime _endTimeOffset;
      private string _episodeName;
      private string _episodeNum;
      private string _episodePart;
      private int _idChannel;
      private int _idProgram;
      private Nullable<int> _idProgramCategory;
      private Nullable<System.DateTime> _originalAirDate;
      private int _parentalRating;
      private bool _previouslyShown;
      private string _seriesNum;
      private int _starRating;
      private System.DateTime _startTime;
      private short _startTimeDayOfWeek;
      private System.DateTime _startTimeOffset;
      private int _state;
      private string _title;

      [DataMember]
      public int IdProgram
      {
        get { return _idProgram; }
        set
        {
          if (_idProgram != value)
          {
            if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
            {
              throw new InvalidOperationException("The property 'IdProgram' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
            }
            _idProgram = value;
            OnPropertyChanged("IdProgram");
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
      public System.DateTime StartTime
      {
        get { return _startTime; }
        set
        {
          if (_startTime != value)
          {
            _startTime = value;
            OnPropertyChanged("StartTime");
          }
        }
      }

      [DataMember]
      public System.DateTime EndTime
      {
        get { return _endTime; }
        set
        {
          if (_endTime != value)
          {
            _endTime = value;
            OnPropertyChanged("EndTime");
          }
        }
      }

      [DataMember]
      public string Title
      {
        get { return _title; }
        set
        {
          if (_title != value)
          {
            _title = value;
            OnPropertyChanged("Title");
          }
        }
      }

      [DataMember]
      public string Description
      {
        get { return _description; }
        set
        {
          if (_description != value)
          {
            _description = value;
            OnPropertyChanged("Description");
          }
        }
      }

      [DataMember]
      public string SeriesNum
      {
        get { return _seriesNum; }
        set
        {
          if (_seriesNum != value)
          {
            _seriesNum = value;
            OnPropertyChanged("SeriesNum");
          }
        }
      }

      [DataMember]
      public string EpisodeNum
      {
        get { return _episodeNum; }
        set
        {
          if (_episodeNum != value)
          {
            _episodeNum = value;
            OnPropertyChanged("EpisodeNum");
          }
        }
      }

      [DataMember]
      public Nullable<System.DateTime> OriginalAirDate
      {
        get { return _originalAirDate; }
        set
        {
          if (_originalAirDate != value)
          {
            _originalAirDate = value;
            OnPropertyChanged("OriginalAirDate");
          }
        }
      }

      [DataMember]
      public string Classification
      {
        get { return _classification; }
        set
        {
          if (_classification != value)
          {
            _classification = value;
            OnPropertyChanged("Classification");
          }
        }
      }

      [DataMember]
      public int StarRating
      {
        get { return _starRating; }
        set
        {
          if (_starRating != value)
          {
            _starRating = value;
            OnPropertyChanged("StarRating");
          }
        }
      }

      [DataMember]
      public int ParentalRating
      {
        get { return _parentalRating; }
        set
        {
          if (_parentalRating != value)
          {
            _parentalRating = value;
            OnPropertyChanged("ParentalRating");
          }
        }
      }

      [DataMember]
      public string EpisodeName
      {
        get { return _episodeName; }
        set
        {
          if (_episodeName != value)
          {
            _episodeName = value;
            OnPropertyChanged("EpisodeName");
          }
        }
      }

      [DataMember]
      public string EpisodePart
      {
        get { return _episodePart; }
        set
        {
          if (_episodePart != value)
          {
            _episodePart = value;
            OnPropertyChanged("EpisodePart");
          }
        }
      }

      [DataMember]
      public int State
      {
        get { return _state; }
        set
        {
          if (_state != value)
          {
            _state = value;
            OnPropertyChanged("State");
          }
        }
      }

      [DataMember]
      public bool PreviouslyShown
      {
        get { return _previouslyShown; }
        set
        {
          if (_previouslyShown != value)
          {
            _previouslyShown = value;
            OnPropertyChanged("PreviouslyShown");
          }
        }
      }

      [DataMember]
      public Nullable<int> IdProgramCategory
      {
        get { return _idProgramCategory; }
        set
        {
          if (_idProgramCategory != value)
          {
            ChangeTracker.RecordOriginalValue("IdProgramCategory", _idProgramCategory);
            if (!IsDeserializing)
            {
              if (ProgramCategory != null && ProgramCategory.IdProgramCategory != value)
              {
                ProgramCategory = null;
              }
            }
            _idProgramCategory = value;
            OnPropertyChanged("IdProgramCategory");
          }
        }
      }

      [DataMember]
      public short StartTimeDayOfWeek
      {
        get { return _startTimeDayOfWeek; }
        set
        {
          if (_startTimeDayOfWeek != value)
          {
            _startTimeDayOfWeek = value;
            OnPropertyChanged("StartTimeDayOfWeek");
          }
        }
      }

      [DataMember]
      public short EndTimeDayOfWeek
      {
        get { return _endTimeDayOfWeek; }
        set
        {
          if (_endTimeDayOfWeek != value)
          {
            _endTimeDayOfWeek = value;
            OnPropertyChanged("EndTimeDayOfWeek");
          }
        }
      }

      [DataMember]
      public System.DateTime EndTimeOffset
      {
        get { return _endTimeOffset; }
        set
        {
          if (_endTimeOffset != value)
          {
            _endTimeOffset = value;
            OnPropertyChanged("EndTimeOffset");
          }
        }
      }

      [DataMember]
      public System.DateTime StartTimeOffset
      {
        get { return _startTimeOffset; }
        set
        {
          if (_startTimeOffset != value)
          {
            _startTimeOffset = value;
            OnPropertyChanged("StartTimeOffset");
          }
        }
      }

      #endregion

      #region Navigation Properties

      private Channel _channel;
      private TrackableCollection<PersonalTVGuideMap> _personalTVGuideMaps;

      private ProgramCategory _programCategory;

      private TrackableCollection<ProgramCredit> _programCredits;

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

      [DataMember]
      public ProgramCategory ProgramCategory
      {
        get { return _programCategory; }
        set
        {
          if (!ReferenceEquals(_programCategory, value))
          {
            var previousValue = _programCategory;
            _programCategory = value;
            FixupProgramCategory(previousValue);
            OnNavigationPropertyChanged("ProgramCategory");
          }
        }
      }

      [DataMember]
      public TrackableCollection<ProgramCredit> ProgramCredits
      {
        get
        {
          if (_programCredits == null)
          {
            _programCredits = new TrackableCollection<ProgramCredit>();
            _programCredits.CollectionChanged += FixupProgramCredits;
          }
          return _programCredits;
        }
        set
        {
          if (!ReferenceEquals(_programCredits, value))
          {
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
            }
            if (_programCredits != null)
            {
              _programCredits.CollectionChanged -= FixupProgramCredits;
              // This is the principal end in an association that performs cascade deletes.
              // Remove the cascade delete event handler for any entities in the current collection.
              foreach (ProgramCredit item in _programCredits)
              {
                ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
              }
            }
            _programCredits = value;
            if (_programCredits != null)
            {
              _programCredits.CollectionChanged += FixupProgramCredits;
              // This is the principal end in an association that performs cascade deletes.
              // Add the cascade delete event handler for any entities that are already in the new collection.
              foreach (ProgramCredit item in _programCredits)
              {
                ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
              }
            }
            OnNavigationPropertyChanged("ProgramCredits");
          }
        }
      }

      [DataMember]
      public TrackableCollection<PersonalTVGuideMap> PersonalTVGuideMaps
      {
        get
        {
          if (_personalTVGuideMaps == null)
          {
            _personalTVGuideMaps = new TrackableCollection<PersonalTVGuideMap>();
            _personalTVGuideMaps.CollectionChanged += FixupPersonalTVGuideMaps;
          }
          return _personalTVGuideMaps;
        }
        set
        {
          if (!ReferenceEquals(_personalTVGuideMaps, value))
          {
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
            }
            if (_personalTVGuideMaps != null)
            {
              _personalTVGuideMaps.CollectionChanged -= FixupPersonalTVGuideMaps;
              // This is the principal end in an association that performs cascade deletes.
              // Remove the cascade delete event handler for any entities in the current collection.
              foreach (PersonalTVGuideMap item in _personalTVGuideMaps)
              {
                ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
              }
            }
            _personalTVGuideMaps = value;
            if (_personalTVGuideMaps != null)
            {
              _personalTVGuideMaps.CollectionChanged += FixupPersonalTVGuideMaps;
              // This is the principal end in an association that performs cascade deletes.
              // Add the cascade delete event handler for any entities that are already in the new collection.
              foreach (PersonalTVGuideMap item in _personalTVGuideMaps)
              {
                ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
              }
            }
            OnNavigationPropertyChanged("PersonalTVGuideMaps");
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
        Channel = null;
        ProgramCategory = null;
        ProgramCredits.Clear();
        PersonalTVGuideMaps.Clear();
      }

      #endregion

      #region Association Fixup
    
      private void FixupChannel(Channel previousValue)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (previousValue != null && previousValue.Programs.Contains(this))
        {
          previousValue.Programs.Remove(this);
        }
    
        if (Channel != null)
        {
          if (!Channel.Programs.Contains(this))
          {
            Channel.Programs.Add(this);
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
    
      private void FixupProgramCategory(ProgramCategory previousValue, bool skipKeys = false)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (previousValue != null && previousValue.Programs.Contains(this))
        {
          previousValue.Programs.Remove(this);
        }
    
        if (ProgramCategory != null)
        {
          if (!ProgramCategory.Programs.Contains(this))
          {
            ProgramCategory.Programs.Add(this);
          }
    
          IdProgramCategory = ProgramCategory.IdProgramCategory;
        }
        else if (!skipKeys)
        {
          IdProgramCategory = null;
        }
    
        if (ChangeTracker.ChangeTrackingEnabled)
        {
          if (ChangeTracker.OriginalValues.ContainsKey("ProgramCategory")
              && (ChangeTracker.OriginalValues["ProgramCategory"] == ProgramCategory))
          {
            ChangeTracker.OriginalValues.Remove("ProgramCategory");
          }
          else
          {
            ChangeTracker.RecordOriginalValue("ProgramCategory", previousValue);
          }
          if (ProgramCategory != null && !ProgramCategory.ChangeTracker.ChangeTrackingEnabled)
          {
            ProgramCategory.StartTracking();
          }
        }
      }
    
      private void FixupProgramCredits(object sender, NotifyCollectionChangedEventArgs e)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (e.NewItems != null)
        {
          foreach (ProgramCredit item in e.NewItems)
          {
            item.Program = this;
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              if (!item.ChangeTracker.ChangeTrackingEnabled)
              {
                item.StartTracking();
              }
              ChangeTracker.RecordAdditionToCollectionProperties("ProgramCredits", item);
            }
            // This is the principal end in an association that performs cascade deletes.
            // Update the event listener to refer to the new dependent.
            ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
          }
        }
    
        if (e.OldItems != null)
        {
          foreach (ProgramCredit item in e.OldItems)
          {
            if (ReferenceEquals(item.Program, this))
            {
              item.Program = null;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              ChangeTracker.RecordRemovalFromCollectionProperties("ProgramCredits", item);
            }
            // This is the principal end in an association that performs cascade deletes.
            // Remove the previous dependent from the event listener.
            ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
          }
        }
      }
    
      private void FixupPersonalTVGuideMaps(object sender, NotifyCollectionChangedEventArgs e)
      {
        if (IsDeserializing)
        {
          return;
        }
    
        if (e.NewItems != null)
        {
          foreach (PersonalTVGuideMap item in e.NewItems)
          {
            item.Program = this;
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              if (!item.ChangeTracker.ChangeTrackingEnabled)
              {
                item.StartTracking();
              }
              ChangeTracker.RecordAdditionToCollectionProperties("PersonalTVGuideMaps", item);
            }
            // This is the principal end in an association that performs cascade deletes.
            // Update the event listener to refer to the new dependent.
            ChangeTracker.ObjectStateChanging += item.HandleCascadeDelete;
          }
        }
    
        if (e.OldItems != null)
        {
          foreach (PersonalTVGuideMap item in e.OldItems)
          {
            if (ReferenceEquals(item.Program, this))
            {
              item.Program = null;
            }
            if (ChangeTracker.ChangeTrackingEnabled)
            {
              ChangeTracker.RecordRemovalFromCollectionProperties("PersonalTVGuideMaps", item);
            }
            // This is the principal end in an association that performs cascade deletes.
            // Remove the previous dependent from the event listener.
            ChangeTracker.ObjectStateChanging -= item.HandleCascadeDelete;
          }
        }
      }

      #endregion
    }
}
