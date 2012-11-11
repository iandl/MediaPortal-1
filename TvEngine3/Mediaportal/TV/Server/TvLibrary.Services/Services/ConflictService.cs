﻿using System.Collections.Generic;
using System.Linq;
using Mediaportal.TV.Server.TVControl.Interfaces.Services;
using Mediaportal.TV.Server.TVDatabase.Entities;
using Mediaportal.TV.Server.TVDatabase.TVBusinessLayer;

namespace Mediaportal.TV.Server.TVLibrary.Services
{  
  public class ConflictService : IConflictService
  {
    #region IConflictService Members

    public IList<Conflict> ListAllConflicts()
    {
      List<Conflict> listAllConflicts = ConflictManagement.ListAllConflicts().ToList();
      return listAllConflicts;
    }

    public Conflict SaveConflict(Conflict conflict)
    {
      return ConflictManagement.SaveConflict(conflict);
    }

    public Conflict GetConflict(int idConflict)
    {
      Conflict conflict = ConflictManagement.GetConflict(idConflict);
      return conflict;
    }

    #endregion
  }

  
}
