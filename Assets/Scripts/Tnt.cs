using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tnt : MonoBehaviour
{
    public float waitTime = 100f;
    float curWaitTime;

    public GameObject particleEffect;
    Chunk thisChunk;

    Vector3 roundPos;

    World world;

    void Start()
    {
        curWaitTime = waitTime;
        world = GameObject.Find("World").GetComponent<World>();
    }

    void Update()
    {
        if(curWaitTime == 0)
        {
            Explode();
        }
        if(curWaitTime > 0)
        {
            curWaitTime--;
        }
    }

    void Explode()
    {
        Chunk origChunk = world.GetChunkFromVector3(transform.position);
        //check points for blocks touching
        foreach(Transform point in transform.GetChild(2))
        {
            Vector3 pos = point.position;
            roundPos = new Vector3(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y),Mathf.FloorToInt(pos.z));

            Chunk newChunk = world.GetChunkFromVector3(roundPos);
            bool surroundingUpdate = false;

            newChunk.EditVoxelNoUpdate(roundPos, 0);
            
        }
        world.GetChunkFromVector3(transform.position).UpdateChunk();
        
        Instantiate(particleEffect,transform.position,Quaternion.identity);

        Destroy(gameObject);
    }
}
