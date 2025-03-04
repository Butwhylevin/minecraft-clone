﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform highlightBlock;
    public Transform placeBlock;
    public float checkIncrement = 0.1f;
    public float reach = 8f;
    public Transform cam;
    private World world;
    public Text selectedBlockText;
    public byte selectedBlockIndex = 1;

    //tnt related
    public GameObject tntPrefab, nukePrefab;

    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();

        selectedBlockText.text = world.blocktypes[selectedBlockIndex].blockName;

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void PlaceCursorBlocks()
    {
        float step = checkIncrement;
        Vector3 lastPos = new Vector3();

        while(step < reach)
        {
            Vector3 pos = cam.position + (cam.forward * step);

            if(world.CheckForVoxel(pos))
            {
                highlightBlock.position = new Vector3(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y),Mathf.FloorToInt(pos.z));
                placeBlock.position = lastPos;

                highlightBlock.gameObject.SetActive(true);
                placeBlock.gameObject.SetActive(true);

                return;

            }

            lastPos = new Vector3(Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y),Mathf.FloorToInt(pos.z));
        
            step += checkIncrement;
        }

        //there are no voxels in reach
        highlightBlock.gameObject.SetActive(false);
        placeBlock.gameObject.SetActive(false);
    }

    private void GetPlayerInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0)
        {
            if(scroll > 0)
            {
                //up
                selectedBlockIndex++;
            }
            else
            {
                //down
                selectedBlockIndex--;
            }
            if(selectedBlockIndex > (byte)world.blocktypes.Length - 1)
            {
                selectedBlockIndex = 1;
            }
            if(selectedBlockIndex < 1)
            {
                selectedBlockIndex = (byte)(world.blocktypes.Length - 1);
            }
            selectedBlockText.text = world.blocktypes[selectedBlockIndex].blockName;
        }

        if(highlightBlock.gameObject.activeSelf)
        {
            //Destroy Block
            if(Input.GetMouseButtonDown(0))
            {
                world.GetChunkFromVector3(highlightBlock.position).EditVoxel(highlightBlock.position, 0);
            }

            //Create Block
            if(Input.GetMouseButtonDown(1))
            {
                world.GetChunkFromVector3(highlightBlock.position).EditVoxel(placeBlock.position, selectedBlockIndex);
            }

            //Active TNT
            if(Input.GetMouseButtonDown(2))
            {
                string selectedBlock = world.GetVoxelName(highlightBlock.position);
                Debug.Log(selectedBlock);

                //is it a tnt block?
                if(selectedBlock == "Tnt")
                {
                    //destroy the block
                    world.GetChunkFromVector3(highlightBlock.position).EditVoxel(highlightBlock.position, 0);

                    //instantiate a tnt there
                    Vector3 createPos = new Vector3(highlightBlock.position.x+0.6f,highlightBlock.position.y+0.7f,highlightBlock.position.z+0.5f);
                    GameObject tntInstance = Instantiate(tntPrefab, createPos, highlightBlock.rotation);
                }

                //is it a nuke?
                if(selectedBlock == "Nuke")
                {
                    //destroy the block
                    world.GetChunkFromVector3(highlightBlock.position).EditVoxel(highlightBlock.position, 0);

                    //instantiate a tnt there
                    Vector3 createPos = new Vector3(highlightBlock.position.x+0.6f,highlightBlock.position.y+0.7f,highlightBlock.position.z+0.5f);
                    GameObject tntInstance = Instantiate(nukePrefab, createPos, highlightBlock.rotation);
                }
            }
        }
    }

    void Update()
    {
        GetPlayerInput();
        PlaceCursorBlocks();
    }
}
