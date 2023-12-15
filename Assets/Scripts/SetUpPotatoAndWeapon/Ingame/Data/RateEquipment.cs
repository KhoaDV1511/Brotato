using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "RateEquipmentDAta")]
public class RateEquipment : ScriptableObject
{
    public List<RatePerWave> ratePerWaves = new List<RatePerWave>();
    
    public TextAsset data;

    // [Button]
    // private void LoadData()
    // {
    //     var jarray = JArray.Parse(data.text);
    //     
    //     for (int i = 1; i <= ratePerWaves.Count; i++)
    //     {
    //         ratePerWaves[i - 1].tireFour = float.Parse(jarray[3][$"{i.ToString()}"]!.ToString().Replace("%", ""));
    //     }
    //         
    // }
}

[Serializable]
public class RatePerWave
{
    public int wave;
    public float tireOne;
    public float tireTwo;
    public float tireThree;
    public float tireFour;
}