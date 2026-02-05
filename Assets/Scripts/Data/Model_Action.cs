using System;
using UnityEngine;

[Serializable]
public class Model_Action : MonoBehaviour
{
    public string actionName;
    public float damageMin;
    public float damageMax;
    public float rangeUnit; //Range from within character range starts being calculated.
    public float rangeCharacter; //Used for actually calculating unit-to-unit range.
    public float delay; //Time to add to decision interval upon execution.
}
