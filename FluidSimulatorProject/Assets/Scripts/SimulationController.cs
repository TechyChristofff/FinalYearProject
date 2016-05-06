using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SimulationController : MonoBehaviour {
    
    public Vector3 CameraPos;
    public Quaternion CameraRot;
    
    public GameObject InGame;
    public GameObject Menu;
    
    public GameObject ParticleRenderer;
    public GameObject MetaBallRenderer;
    public GameObject Blob;
   
    public GameObject particleNumberController;    
    public GameObject rendererController;
    public GameObject managedController;
    public GameObject preRenderController;
    public GameObject gravityValue;
    public GameObject textureParticles;
    public GameObject particleMeshScaleController;
    public GameObject MetaBallWeighting;
    
    public GameObject Play;
    public GameObject Pause;
    public GameObject Stop;
    public GameObject Save;
    public GameObject Go;
    
    public Mesh particleMesh;
    public Material[] particleMaterials;
    
    public Transform NodeTree;
    StaticMetaballSeed MetaBallSeed;
    
    List<float[,]> PointCache;
    List<Mesh> MeshCache;
    GameObject[] particlePool;
    
    Vector3 vec;
    
    enum State
    {
        Pre,
        Realtime
    }
    
    enum RenderMethod
    {
        MetaBall,
        Particle,
        Mesh,
        Blob
    }
    
    State CurrentState;
    
    RenderMethod method;
    Wrapper Simulation;
    
    int currentPoints;
	// Use this for initialization
	void Start () {
        vec = new Vector3();
        Simulation = GetComponent<Wrapper>();
	    method = RenderMethod.Mesh;
        MetaBallSeed = MetaBallRenderer.GetComponent<StaticMetaballSeed>();
        
        Simulation.InitialiseFluidSimulation();
        
        ResetSubParticles(Simulation.Points,method);
        
        RealTime(method);
        
	}
    
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.R))
        {
            textureParticles.GetComponent<Toggle>().isOn = !textureParticles.GetComponent<Toggle>().isOn;
        }
	     if(Input.anyKeyDown || currentPoints != Simulation.SimulationPointCount)
            ResetSubParticles(Simulation.Points, method);
        
        
	}
    
    void LateUpdate()
    {
        RealTime(method);
        //Pass Particles to renderer
        currentPoints = Simulation.SimulationPointCount;
    }
    
    public void GoPressed()
    {
        switch (rendererController.GetComponent<Dropdown>().value)
        {
            case 0:
            {
                method = RenderMethod.MetaBall;
            }break;
            case 1:
            {
                method = RenderMethod.Mesh;
            }break;
            case 3:
            {
                method = RenderMethod.Blob;
                Blob.SetActive(true);
            }break;
            default:
            { method = RenderMethod.Mesh;
            }break;
        }
        
        StartSimulation();
        
    }
    
    void StartSimulation()
    {
        float tmpGrav;
        if(float.TryParse( gravityValue.GetComponent<InputField>().text,out tmpGrav))
            Simulation.gravY =  tmpGrav;
            
        Simulation.initalParticles = (int)particleNumberController.GetComponent<Slider>().value;
        Simulation.managed = managedController.GetComponent<Toggle>().isOn;
        
        Simulation.InitialiseFluidSimulation();
    }
    
    void RealTime(RenderMethod renderer)
    {
        switch (renderer)
        {
            case RenderMethod.Mesh:
            {
                
            }break;
            case RenderMethod.MetaBall:
            {
                MetaBallSeed.CreateMesh();
            }break;
            default:
            {
                
            }break;
        }
        
        ModifyPoints(Simulation.Points, renderer);
        
        GameObject.Find("Camera").GetComponent<GhostFreeRoamCamera>().enabled = true;
    }
    
    
    
    void ModifyPoints(float[,] input, RenderMethod renderer)
	{
		if (input.Length<= 0) return;
		
		if (Application.isEditor)
		{
			//ResetSubParticles();
		}
	
        Particle[] particles = GetComponent<ParticleEmitter>().particles;

		for (int i=0; i<input.Length; ++i)
		{
			GameObject particleObject = particlePool[i];
			if (i <= input.Length)
			{
				vec.Set(input[i,0],input[i,1],input[i,2]); 
                particleObject.transform.position = vec;
				particleObject.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                
                switch (renderer)
                {
                    case RenderMethod.Mesh:
                    {
                        particleObject.GetComponent<Renderer>().enabled = true;
                        particleObject.GetComponent<Renderer>().receiveShadows = textureParticles.GetComponent<Toggle>().isOn;
                        float tmpScale = particleMeshScaleController.GetComponent<Slider>().value;
                        particleObject.transform.localScale = new Vector3(tmpScale, tmpScale, tmpScale);
                    }break;
                    case RenderMethod.MetaBall:
                    {
                        
                    }break;
                    default:
                    {
                        
                    }break;
                }
			}
		}
	}
	
    void ResetSubParticles(float[,] input, RenderMethod renderer)
	{
		if (input.Length <= 0 ) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[input.Length];

		for (int i=0; i<input.Length; ++i)
		{
			GameObject particleObject = new GameObject();
			particleObject.name = "Particle"+i.ToString();
            
            switch (renderer)
            {
                case RenderMethod.Mesh:
                    {
                        MeshFilter mf = particleObject.AddComponent<MeshFilter>();
			            mf.mesh = particleMesh;
			            MeshRenderer mr = particleObject.AddComponent<MeshRenderer>();
                        if (textureParticles.GetComponent<Toggle>().isOn)
                        {
                            mr.materials = particleMaterials;
                        }
                        
                        particleObject.transform.parent = transform;
                        
                        particleObject.GetComponent<Renderer>().enabled = false;
                        
                    }break;
                    case RenderMethod.MetaBall:
                    {
                        particleObject.transform.parent = NodeTree;
                        particleObject.AddComponent<MetaballNode>().baseRadius = MetaBallWeighting.GetComponent<Slider>().value;
                        
                    }break;
                    default:
                    {
                        
                    }break;
            }
			
			
			float tmpScale = particleMeshScaleController.GetComponent<Slider>().value;
            particleObject.transform.localScale = new Vector3(tmpScale, tmpScale, tmpScale);

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
    
    public Wrapper PublicSimulation{
        get
        {
            return Simulation;
        }
    }
    
    public void StopPressed()
    {
        GameObject.Find("Camera").transform.position = CameraPos;
        GameObject.Find("Camera").transform.rotation = CameraRot;
        GameObject.Find("Camera").GetComponent<GhostFreeRoamCamera>().enabled = false;
        InGame.SetActive(false);
        Menu.SetActive(true);
    }
    
    public void UpdateParticleNumber()
    {
        particleNumberController.transform.GetChild(3).GetComponent<Text>().text = "Number of Particles = " + particleNumberController.GetComponent<Slider>().value.ToString();
    }
}
