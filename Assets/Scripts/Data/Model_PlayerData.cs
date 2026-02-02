using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Model_PlayerData
{
    public List<Model_Unit> units = new List<Model_Unit>(); //Which units player is currently in possession of.
    public int currentBattle; //Which room/battle player is currently at.
    public bool _battleInProgress; //Is a battle currently ongoing?

    #region Settings



    #endregion
}
