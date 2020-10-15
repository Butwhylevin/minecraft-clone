using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public int sizeX;
    public int sizeZ;
    public int groundHeight;
    public float terDetail;
    public float terHeight;

    public GameObject[] blocks;
    public GameObject player;

    
    int seed;
    
    void Start()
    {
        seed = Random.Range(100000,999999);
        GenerateTerrain();
    }

    void Update()
    {
        
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                int maxY = (int)(Mathf.PerlinNoise((x / 2 + seed) / terDetail, (z / 2 + seed) / terDetail) * terHeight);
                maxY += groundHeight;
                GameObject grass = Instantiate(blocks[0], new Vector3(x,maxY,z), Quaternion.identity) as GameObject;
                grass.transform.SetParent(GameObject.FindGameObjectWithTag("Enviroment").transform);

                for (int y = 0; y < maxY; y++)
                {
                    GameObject stone = Instantiate(blocks[1], new Vector3(x,y,z), Quaternion.identity) as GameObject;
                    stone.transform.SetParent(GameObject.FindGameObjectWithTag("Enviroment").transform);
                }

                if(x ==(int)(sizeX / 2) && z == (int)(sizeZ / 2))
                {
                    Instantiate(player, new Vector3(x,maxY+3,z), Quaternion.identity);
                }
            }
        }
    }
}
