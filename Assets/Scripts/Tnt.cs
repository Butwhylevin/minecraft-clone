using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tnt : MonoBehaviour
{
    public float waitTime = 100f;
    float curWaitTime;

    public GameObject particleEffect;
    Chunk thisChunk;

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
        //create particle emmitter
        //Instantiate(particleEffect,transform.position,Quaternion.identity);
        
        //check through for blocks touching
        foreach(Transform point in transform.GetChild(2))
        {
            Vector3 pos = point.position;
            Vector3 roundPos = new Vector3(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y),Mathf.FloorToInt(pos.z));

            world.GetChunkFromVector3(roundPos).EditVoxelNoUpdate(roundPos, 0);
        }
        world.GetChunkFromVector3(transform.position).UpdateChunk();

        Destroy(gameObject);
    }

    IEnumerator CheckPoints()
    {
        foreach(Transform point in transform.GetChild(2))
        {
            Vector3 pos = point.position;
            Vector3 roundPos = new Vector3(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y),Mathf.FloorToInt(pos.z));

            world.GetChunkFromVector3(roundPos).EditVoxelNoUpdate(roundPos, 0);
            yield return null;
        }
        world.GetChunkFromVector3(transform.position).UpdateChunk();

        Destroy(gameObject);
    }
}
