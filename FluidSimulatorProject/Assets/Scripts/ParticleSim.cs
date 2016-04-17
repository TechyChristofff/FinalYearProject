using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ParticleSim : MonoBehaviour {
    
	SimulationController Controller;
    public bool mesh = false;
    
    public bool render = true;
	public Mesh particleMesh;
	public Material[] particleMaterials;
	public float meshScale = 100.0f;
	public Vector3 rotationAxis = Vector3.up;
    GameObject[] particlePool;
    Vector3 vec;
    int currentPoints;
	
	string state;
	
	List<float[,]> Cache;

	void Start () {
        
        vec = new Vector3();
		Controller = GameObject.Find("Controller").GetComponent<SimulationController>();
        
        Controller.PublicSimulation.InitialiseFluidSimulation();
        
        //Simulation.ToggleSimulation();
        
        ResetSubParticles(Controller.PublicSimulation.Points);
		Cache = new List<float[,]>();
	}
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            render = !render;
        }
        if(Input.anyKeyDown || currentPoints != Controller.PublicSimulation.SimulationPointCount)
            ResetSubParticles(Controller.PublicSimulation.Points);
    }
    
    void LateUpdate()
    {
		Render(Controller.PublicSimulation.Points);
		
        currentPoints = Controller.PublicSimulation.SimulationPointCount;
    }
	
	
	
    
	public void Render(float[,] input)
	{
		if (particleMesh == null || input.Length<= 0 || !mesh) return;
		
		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
	
        Particle[] particles = GetComponent<ParticleEmitter>().particles;
       
	    
		for (int i=0; i<input.Length; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i >= input.Length)
			{
				particleObject.GetComponent<Renderer>().enabled = false;
				
			}
			else
			{
				//float[] p = Point[i];
                //Debug.Log(Simulation.Points[i,0]);
                vec.Set(input[i,0],input[i,1],input[i,2]); 
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
	
    void ResetSubParticles(float[,] input)
	{
		if (particleMesh == null || input.Length <= 0 || !mesh) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[input.Length];

		for (int i=0; i<input.Length; ++i)
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
