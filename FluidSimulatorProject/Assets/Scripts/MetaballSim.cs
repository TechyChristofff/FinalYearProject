using UnityEngine;
using System.Collections.Generic;

public class MetaballSim : MonoBehaviour {

	GameObject[] nodePool;
    public StaticMetaballSeed metaball;
	public float meshScale = 1;
    public float Weighting = 1;
    Vector3 vec;
    Wrapper Simulation;
    int currentPoints;
	// Use this for initialization
	void Start () {
	    vec = new Vector3();
        Simulation = GameObject.Find("Simulation").GetComponent<Wrapper>();
        Simulation.InitialiseFluidSimulation();
        
        ResetSubParticles();
        metaball.CreateMesh();
	}
    void Update()
    {
        if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
            ResetSubParticles();
    }
	// Update is called once per frame
	void LateUpdate () {
         MeshRenderer();
         currentPoints = Simulation.SimulationPointCount;
         //if(Simulation.IsRunning)
	        metaball.CreateMesh();
	}
    
    void MeshRenderer()
    {
        if (Simulation.SimulationPointCount<= 0) return;
		
		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
	
        
		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = nodePool[i];
			if (i <= Simulation.Points.Length)
			{
                vec.Set((float)Simulation.Points[i,0],(float)Simulation.Points[i,1],(float)Simulation.Points[i,2]); 
                particleObject.transform.position = vec;
			}
		}
    
    }
      void ResetSubParticles()
	{
		if (Simulation.SimulationPointCount <= 0) return;
		
		RemoveSubParticles();
		
		nodePool = new GameObject[Simulation.SimulationPointCount];

		for (int i=0; i<Simulation.SimulationPointCount; ++i)
		{
			GameObject particleObject = new GameObject();
			particleObject.name = "Node" + i.ToString();
			
			particleObject.transform.parent = transform; //Node root
			particleObject.transform.localScale = new Vector3(meshScale, meshScale, meshScale);
            
            particleObject.AddComponent<MetaballNode>().baseRadius = Weighting;

			nodePool[i] = particleObject;
		}
	}
    
    void RemoveSubParticles()
	{
		if (nodePool != null)
		{
			foreach (var o in nodePool)
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
			nodePool = null;
		}
	}
}
