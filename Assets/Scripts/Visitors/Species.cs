using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "Scriptable Objects/Species"), System.Serializable]
public class Species : ScriptableObject
{
    public int MatureAge = 18;
    public int MinAge = 13;
    public int CommonAgeCap = 32;
    public int RareAgeCap = 80;

    public string SpeciesName;
    public List<FaceLayerData> FaceLayers;
}

[System.Serializable]
public class FaceLayerData
{
    public List<Sprite> Sprites;
}
