using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Model_Unit
{
    public int characters_Max; //Number of characters this unit can contain at max.
    public int characters_Current; //Number of characters this unit currently consists of.
    [Space(4)]
    public float[] health_Max; //Per character.
    public float[] health_Current; //Per character.
    public float moveSpeed; //Entire unit.
    [Space(4)]
    //Temp parameters.
    public float range;
    public float damage_Max;
    public float damage_Min;

    [Header("Behaviours")]
    public List<Model_Behaviour> behaviourCurrent = new List<Model_Behaviour>();
}
