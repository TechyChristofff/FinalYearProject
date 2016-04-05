using UnityEngine;
using System.Collections.Generic;

public class MetaballSImulation : MonoBehaviour {

    Vector3 vec;
    Wrapper Simulation;
    GameObject[] particlePool;
    List<GameObject> Nodes;
    int currentPoints;
    public Vector3 rotationAxis = Vector3.up;
    
	// Use this for initialization
	void Start () {
        vec = new Vector3();
        Simulation = GameObject.Find("Simulation").GetComponent<Wrapper>();
	    
        ResetSubParticles();
        
        
	}
	void Update()
    {
        if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
            ResetSubParticles();
    }
	// Update is called once per frame
	void LateUpdate () {
        
	    currentPoints = Simulation.SimulationPointCount;
	}
    
    void MeshRenderer()
    {
        if (Simulation.SimulationPointCount<= 0) return;
		
		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
	
        Particle[] particles = GetComponent<ParticleEmitter>().particles;
        
		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i >= Simulation.Points.Length)
			{
				particleObject.GetComponent<Renderer>().enabled = false;
				
			}
			else
			{
				//float[] p = Point[i];
                //Debug.Log(Simulation.Points[i,0]);
                vec.Set((float)Simulation.Points[i,0],(float)Simulation.Points[i,1],(float)Simulation.Points[i,2]); 
                //Debug.Log(vec);  
                //particleObject.GetComponent<Renderer>().enabled = true;
                particleObject.transform.position = vec;
				particleObject.transform.rotation = Quaternion.AngleAxis(0, rotationAxis);
                
				float scale = 1;
				particleObject.transform.localScale = new Vector3(scale, scale, scale);
			}
		}
    
    }
    
     void ResetSubParticles()
	{
		if (Simulation.SimulationPointCount <= 0) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[Simulation.SimulationPointCount];

		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = new GameObject();
			particleObject.name = "Node" + i.ToString();
			
			particleObject.transform.parent = transform;
			particleObject.transform.localScale = new Vector3(1, 1, 1);
            //particleObject.GetComponent<Renderer>().enabled = false;
            
            particleObject.AddComponent<MetaballNode>();

			particlePool[i] = particleObject;
		}
	}
    
    void RemoveSubParticles()
	{
		if (particlePool != null)
		{
			foreach (var o in particlePool)
			{
				if (Application.isEditor)
				{
					DestroyImmediate(o);
				}
				else
				{
					Destroy(o);
				}
			}
			particlePool = null;
		}
	}
}
