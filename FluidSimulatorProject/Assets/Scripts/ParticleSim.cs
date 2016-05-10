using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

public class ParticleSim : MonoBehaviour {
    
	Wrapper Simulation;
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
		Simulation = GameObject.Find("Simulation").GetComponent<Wrapper>();
        ResetSubParticles(Simulation.Points);
		Cache = new List<float[,]>();
	}
    
    void Update()
    {
		if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
			ResetSubParticles(Simulation.Points);
		
        if(Input.GetKeyDown(KeyCode.R))
        {
            render = !render;
        }
    }
    
    void LateUpdate()
    {

        MeshRenderer();
		currentPoints = Simulation.SimulationPointCount;
    }
    
    void MeshRenderer()
    {
		if (particleMesh == null || Simulation.SimulationPointCount<= 0 || !mesh) return;

		Render(Simulation.Points);
		
		currentPoints = Simulation.SimulationPointCount;
    }
	
    
	public void Render(float[,] input)
	{
		if (particleMesh == null || input.Length<= 0 || !mesh) return;

		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
		
		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i >= input.Length)
			{
				particleObject.GetComponent<Renderer>().enabled = false;
			}
			else
			{
				
                vec.Set(Simulation.Points[i,0],Simulation.Points[i,1],Simulation.Points[i,2]); 
                particleObject.transform.position = vec;
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
			
			particleObject.GetComponent<Renderer>().enabled = true;
            particleObject.GetComponent<Renderer>().receiveShadows = render;
			particleObject.transform.rotation = Quaternion.AngleAxis(0, rotationAxis);
			
			float scale = 1 * meshScale;
			particleObject.transform.localScale = new Vector3(scale, scale, scale);
			
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
