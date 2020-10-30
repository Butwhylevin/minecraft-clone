using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;
    public BlockType[] blockTypes;
    Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

    private void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for(int x= 0; x<VoxelData.WorldSizeInChunks; x++)
        {
            for(int z= 0; z<VoxelData.WorldSizeInChunks; z++)
            {
                CreateNewChunk(x,z);
            }
        }
    }

    public byte GetVoxel(Vector3 pos)
    {
        if(pos.y<1)
            return 1;
        else if(pos.y==VoxelData.ChunkHeight - 1)
            return 2;
        else
            return 3;
    }

    void CreateNewChunk(int x, int z)
    {
        chunks[x,z]= new Chunk(new ChunkCoord(x,z),this);

    }

    bool isChunkInWorld(ChunkCoord coord)
    {
        if(coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks-1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks-1)
            return true;
        else
            return false;
    }

    bool isVoxelInWorld(Vector3 pos)
    {
        if(pos.x>0 && pos.x < VoxelData.WorldSizeInVoxels-1 && pos.y>0 && pos.y < VoxelData.ChunkHeight-1 && pos.z>0 && pos.z < VoxelData.WorldSizeInVoxels-1 )
            return true;
        else
            return false;
    }
}

public class ChunkCoord
{
    public int x;
    public int z;

    public ChunkCoord(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}   

[System.Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int frontFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;
    public int leftFaceTexture;
    public int rightFaceTexture;

    public int GetTextureID(int faceIndex)
    {
        switch(faceIndex)
        {
            //back, front, top, bottom, left, right
            case 0:
                return backFaceTexture;
            case 1:
                return frontFaceTexture;
            case 2:
                return topFaceTexture;
            case 3:
                return bottomFaceTexture;
            case 4:
                return leftFaceTexture;
            case 5:
                return rightFaceTexture;
            default:
                Debug.Log("Error in Get Texture ID; Invalid face index");
                return 0;
        }
    }
}
