using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "RateDropItems")]
public class RateDropItems : ScriptableObject
{
    public List<ElementRateDropItem> elementRateDropItems = new List<ElementRateDropItem>();
}
[Serializable]
public class ElementRateDropItem
{
    public int wave;
    public float dropChange;
}
