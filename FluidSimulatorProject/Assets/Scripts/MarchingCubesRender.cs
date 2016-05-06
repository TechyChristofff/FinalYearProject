using UnityEngine;
using System.Collections;

public class MarchingCubesRender : MonoBehaviour {

    Mesh m_mesh;
    
    int width = 64;
	int height = 64;
	int length = 64;
	// Use this for initialization
	void Start () {
        
       
	    MarchingCubes.SetTarget(0);
        MarchingCubes.SetWindingOrder(0,1,2);
        MarchingCubes.SetModeToCubes();
        
        float[,,] voxels = new float[width, height, length];
        
        for(int x = 0; x < width; x++)
	    {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < length; z++)
                {
                    voxels[x,y,z] = 0;
                    
                }
            }
        }
        
        m_mesh = MarchingCubes.CreateMesh(voxels) ;
        m_mesh.normals = new Vector3[m_mesh.vertices.Length];
        GetComponent<MeshFilter>().mesh = m_mesh;
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
