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
    
    public GameObject Play;
    public GameObject Pause;
    public GameObject Stop;
    public GameObject Save;
    public GameObject Go;
    
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
	// Use this for initialization
	void Start () {
        vec = new Vector3();
        Simulation = GetComponent<Wrapper>();
	    method = RenderMethod.Blob;
	}
    
	// Update is called once per frame
	void Update () {
	
	}
    
    void LateUpdate()
    {
        RealTime();
        //Pass Particles to renderer
        
    }
    
    public void UpdateParticleNumber()
    {
        particleNumberController.transform.GetChild(3).GetComponent<Text>().text = "Number of Particles = " + particleNumberController.GetComponent<Slider>().value.ToString();
    }
    
    public void GoPressed()
    {
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
    
    void RealTime()
    {
           
            
        switch (rendererController.GetComponent<Dropdown>().value)
        {
            case 0:
            {
                Simulation.enabled = true;
                MetaBallRenderer.SetActive(true);
            }break;
            case 1:
            {
                Simulation.enabled = true;
                ParticleRenderer.SetActive(true);
            }break;
            default:
            { 
            }break;
        }
        
       
        
        InGame.SetActive(true);
        Menu.SetActive(false);
        
        GameObject.Find("Camera").GetComponent<GhostFreeRoamCamera>().enabled = true;
    }
    
    void PreRendered()
    {
        
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
                    case RenderMethod.Particle:
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
	
    void ResetSubParticles(float[,] input)
	{
		if (input.Length <= 0 ) return;
		
		RemoveSubParticles();
		
		particlePool = new GameObject[input.Length];

		for (int i=0; i<input.Length; ++i)
		{
			GameObject particleObject = new GameObject();
			particleObject.name = "Particle"+i.ToString();
			
			particleObject.transform.parent = transform;
			particleObject.transform.localScale = new Vector3(1, 1, 1);

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
