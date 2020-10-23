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

    void Combine(GameObject block)
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        //Destroy(this.gameObject.GetComponent<MeshCollider>());

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        
        MeshFilter filter = transform.GetComponent<MeshFilter>();
        filter.mesh = new Mesh();
        
        filter.mesh.CombineMeshes(combine,true);
        filter.mesh.RecalculateBounds();
        filter.mesh.RecalculateNormals();
        filter.mesh.Optimize();

        //this.gameObject.AddComponent<MeshCollider>();
        this.gameObject.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
        transform.gameObject.SetActive(true);

        Destroy(block);
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
                Combine(grass);

                for (int y = 0; y < maxY; y++) //dirt
                {
                    int dirtLayers = Random.Range(3,5);
                    if(y >= maxY - dirtLayers)
                    {
                        GameObject dirt = Instantiate(blocks[1], new Vector3(x,y,z), Quaternion.identity) as GameObject;
                        dirt.transform.SetParent(GameObject.FindGameObjectWithTag("Enviroment").transform);
                        //Combine(dirt);
                    }
                    else //stone
                    {
                        GameObject stone = Instantiate(blocks[2], new Vector3(x,y,z), Quaternion.identity) as GameObject;
                        stone.transform.SetParent(GameObject.FindGameObjectWithTag("Enviroment").transform);
                        //Combine(stone);
                    }
                   
                }

                if(x ==(int)(sizeX / 2) && z == (int)(sizeZ / 2))
                {
                    Instantiate(player, new Vector3(x,maxY+3,z), Quaternion.identity);
                }
            }
        }
        //Simplify(0.5f);
    }
    public void Simplify(float quality)
    {
        MeshFilter filter = transform.GetComponent<MeshFilter>();
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(filter.mesh);
        meshSimplifier.SimplifyMesh(quality);
        filter.mesh = meshSimplifier.ToMesh();
    }
}
