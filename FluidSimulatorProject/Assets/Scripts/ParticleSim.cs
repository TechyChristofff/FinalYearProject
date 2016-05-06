using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Runtime.InteropServices;

public class ParticleSim : MonoBehaviour {
    
    Wrapper Simulation;
=======

public class ParticleSim : MonoBehaviour {
    
	SimulationController Controller;
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
    public bool mesh = false;
    
    public bool render = true;
	public Mesh particleMesh;
	public Material[] particleMaterials;
	public float meshScale = 100.0f;
	public Vector3 rotationAxis = Vector3.up;
    GameObject[] particlePool;
    Vector3 vec;
    int currentPoints;
<<<<<<< HEAD
=======
	
	string state;
	
	List<float[,]> Cache;
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3

	void Start () {
        
        vec = new Vector3();
<<<<<<< HEAD
        Simulation = GameObject.Find("Simulation").GetComponent<Wrapper>();
        
        //Simulation.ToggleSimulation();
        
        ResetSubParticles();
=======
		Controller = GameObject.Find("Controller").GetComponent<SimulationController>();
        
        Controller.PublicSimulation.InitialiseFluidSimulation();
        
        //Simulation.ToggleSimulation();
        
        ResetSubParticles(Controller.PublicSimulation.Points);
		Cache = new List<float[,]>();
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
	}
    
    void Update()
    {
<<<<<<< HEAD
        if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
            ResetSubParticles();
=======
        if(Input.GetKeyDown(KeyCode.R))
        {
            render = !render;
        }
        if(Input.anyKeyDown || currentPoints != Controller.PublicSimulation.SimulationPointCount)
            ResetSubParticles(Controller.PublicSimulation.Points);
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
    }
    
    void LateUpdate()
    {
<<<<<<< HEAD
        MeshRenderer();
        currentPoints = Simulation.SimulationPointCount;
    }
    
    void MeshRenderer()
    {
        if (particleMesh == null || Simulation.SimulationPointCount<= 0 || !mesh) return;
=======
		Render(Controller.PublicSimulation.Points);
		
        currentPoints = Controller.PublicSimulation.SimulationPointCount;
    }
	
    
	public void Render(float[,] input)
	{
		if (particleMesh == null || input.Length<= 0 || !mesh) return;
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
		
		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
	
        Particle[] particles = GetComponent<ParticleEmitter>().particles;
<<<<<<< HEAD
        
		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i >= Simulation.Points.Length)
=======
       
	    
		for (int i=0; i<input.Length; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i >= input.Length)
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
			{
				particleObject.GetComponent<Renderer>().enabled = false;
				
			}
			else
			{
				//float[] p = Point[i];
                //Debug.Log(Simulation.Points[i,0]);
<<<<<<< HEAD
                vec.Set((float)Simulation.Points[i,0],(float)Simulation.Points[i,1],(float)Simulation.Points[i,2]); 
=======
                vec.Set(input[i,0],input[i,1],input[i,2]); 
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
                //Debug.Log(vec);  
                particleObject.GetComponent<Renderer>().enabled = true;
                particleObject.GetComponent<Renderer>().receiveShadows = render;
                particleObject.transform.position = vec;
				particleObject.transform.rotation = Quaternion.AngleAxis(0, rotationAxis);
                
				float scale = 1 * meshScale;
				particleObject.transform.localScale = new Vector3(scale, scale, scale);
			}
		}
<<<<<<< HEAD
    
    }
      void ResetSubParticles()
	{
		if (particleMesh == null || Simulation.SimulationPointCount <= 0 || !mesh) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[Simulation.SimulationPointCount];

		for (int i=0; i<Simulation.SimulationPointCount; ++i)
=======
	}
	
    void ResetSubParticles(float[,] input)
	{
		if (particleMesh == null || input.Length <= 0 || !mesh) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[input.Length];

		for (int i=0; i<input.Length; ++i)
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
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

<<<<<<< HEAD
			particlePool[i] = particleObject;
=======
			particlePool[i] = particleObject;	
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
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
<<<<<<< HEAD
   
=======
>>>>>>> e77d3e4a6d8237737733f084b234d7bd3a8fbae3
}
