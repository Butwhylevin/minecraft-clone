using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeAttributes", menuName = "Minecrooft/Biome Attribute")]
public class BiomeAttributes : ScriptableObject
{
    public string biomeName;

    public int solidGroundHeight; //if below this value, all blocks are solid
    public int terrainHeight; //height from solid ground to max height of terrain
    public float terrainScale; //noise scale

    public Lode[] lodes;

}

[System.Serializable]
public class Lode
{
    public string lodeName;
    public byte blockID;
    public int minHeight;
    public int maxHeight;
    public float scale;
    public float threshold;
    public float noiseOffset;
}
