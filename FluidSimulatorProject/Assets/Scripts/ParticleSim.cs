using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ParticleSim : MonoBehaviour {
    
    Wrapper Simulation;
    public bool mesh = false;
    
    public bool render = true;
	public Mesh particleMesh;
	public Material[] particleMaterials;
	public float meshScale = 100.0f;
	public Vector3 rotationAxis = Vector3.up;
    GameObject[] particlePool;
    Vector3 vec;
    int currentPoints;

	void Start () {
        
        vec = new Vector3();
        Simulation = GameObject.Find("Simulation").GetComponent<Wrapper>();
        
        //Simulation.ToggleSimulation();
        
        ResetSubParticles();
	}
    
    void Update()
    {
        if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
            ResetSubParticles();
    }
    
    void LateUpdate()
    {
        MeshRenderer();
        currentPoints = Simulation.SimulationPointCount;
    }
    
    void MeshRenderer()
    {
        if (particleMesh == null || Simulation.SimulationPointCount<= 0 || !mesh) return;
		
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
                particleObject.GetComponent<Renderer>().enabled = true;
                particleObject.GetComponent<Renderer>().receiveShadows = render;
                particleObject.transform.position = vec;
				particleObject.transform.rotation = Quaternion.AngleAxis(0, rotationAxis);
                
				float scale = 1 * meshScale;
				particleObject.transform.localScale = new Vector3(scale, scale, scale);
			}
		}
    
    }
      void ResetSubParticles()
	{
		if (particleMesh == null || Simulation.SimulationPointCount <= 0 || !mesh) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[Simulation.SimulationPointCount];

		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = new GameObject();
			particleObject.name = "ParticleMesh";

			MeshFilter mf = particleObject.AddComponent<MeshFilter>();
			mf.mesh = particleMesh;
			MeshRenderer mr = particleObject.AddComponent<MeshRenderer>();
            if (render)
            {
                mr.materials = particleMaterials;
            }
			
			particleObject.transform.parent = transform;
			particleObject.transform.localScale = new Vector3(meshScale, meshScale, meshScale);
            particleObject.GetComponent<Renderer>().enabled = false;

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
