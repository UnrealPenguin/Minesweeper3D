using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks 
{
    const float SPACER = 1.05f; // Distance between each cube
    private int x, y, z;
    private GameObject block;
    

    // Int, Int, Int, Array, Mesh, Material
    // Constructs a block at x y z position with it's default state
    // _center provides the mid-point of each axis. Also takes in a Mesh and Material for rendering.
    public Blocks(int _x, int _y, int _z, float[] _center, Mesh _blockMesh, Material _blockMat, int[,,] _matrix,bool _isBomb) {
        this.x = _x;
        this.y = _y;
        this.z = _z;

        // Render blocks in game
        this.block = new GameObject("x:"+this.x+"y:"+this.y+"z:"+this.z);
        this.block.transform.position = new Vector3(this.x*SPACER - _center[0], this.y*SPACER - _center[1], this.z*SPACER - _center[1]);
    
        this.block.AddComponent<BoxCollider>();
        this.block.AddComponent<MeshRenderer>();
        this.block.AddComponent<MeshFilter>().mesh = _blockMesh;
        this.block.GetComponent<MeshRenderer>().material = _blockMat;
        this.block.AddComponent<BlockManager>();
        // Store the x y and z of the matrix to know where each individual block is. 
        this.block.GetComponent<BlockManager>().initialize(_x, _y, _z, _matrix, _isBomb);

    }

}
