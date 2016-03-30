using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Wrapper : MonoBehaviour {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MyDelegate(string str);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    public static extern void SetDebugFunction( IntPtr fp );    
    
    #region Internal Calls
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern bool InitInternalSystem(); //Initialise the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void GetInternalPoints(float[,] array, int height, int width, float worldRatio); //Return the point data from the system [Point][x,y,z]
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void InternalAnimate();//Animate the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void InternalDispose();//Dispose of the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern int GetInternalLength();//Get the number of points in the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void InternalStartRunning();//Start the system running/pause the system
      
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern int InternalRunningState();//Get the running state (0 = paused, 1 = running)
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern float GetInternalPoint(int id, int direction);//Get a specific point (point number, 0=x, 1=Y,2=Z)
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern bool InitVariableSystem(int initialParticles, int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, 
    float worldSizeY,float worldSizeZ,float wallDampening,float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, 
    float surfaceCoeffeciant);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void AddParticle(float PosX, float PosY, float PosZ, float VelX, float VelY, float VelZ );
    
    #endregion 
    public GameObject particle;
    public bool managed = false;
    
    float[,] Points;
    int pointCount;
    //Dictionary<int,GameObject> allParticles;
    List<GameObject> allParticles;
    
    Vector3 parentPos;
    
    Vector3 vec = new Vector3();
    int width, height;
    
    public int initalParticles = 3000; //3000
    public int maxParticles = 30000; //30000
    public float kernelInput = 0.04f; //0.04
    public float massInput = 0.02f; //0.02
    public float gravX = 0f; //0
    public float gravY = 6.8f; //6.8 
    public float gravZ = 0f; //0
    public float worldSizeX = 0.64f; //0.64
    public float worldSizeY = 0.64f; //0.64
    public float worldSizeZ = 0.64f; //0.64
    public float wallDampening = -0.5f; //-0.5
    public float restDencity = 1000.0f; //1000
    public float gasConstant = 1.0f; //1 
    public float viscosityInput = 6.5f; //6.5 
    public float timeStep = 0.003f; //0.003 
    public float surfaceNormals = 6.0f;//6 
    public float surfaceCoeffeciant = 0.1f;//0.1 
    public float worldRatio = 21.25f; //21.25
    
	// Use this for initialization
	void Start () {
         width = 0;
         height = 0;
         MyDelegate callback_delegate = new MyDelegate( CallBackFunction );
        // Convert callback_delegate into a function pointer that can be
        // used in unmanaged code.
        IntPtr intptr_delegate =
            Marshal.GetFunctionPointerForDelegate(callback_delegate);       
        // Call the API passing along the function pointer.
        SetDebugFunction( intptr_delegate );

        parentPos = this.GetComponentInParent<Transform>().position;
        
        //allParticles = new Dictionary<int, GameObject>();
        allParticles = new List<GameObject>();
        //SPH_System = InitOpenSPHSystem();
        
        bool test = false;
		if (!managed) {
			test = InitInternalSystem ();
		} else {
			test = InitVariableSystem (initalParticles, maxParticles, kernelInput, massInput, gravX, gravY, gravZ, worldSizeX, worldSizeY, worldSizeZ, wallDampening, restDencity, 
				gasConstant, viscosityInput, timeStep, surfaceNormals, surfaceCoeffeciant);
		}
        
        Debug.Log(test);
        pointCount = GetInternalLength();
        //pointCount = GetLength(SPH_System);
        Debug.Log(pointCount.ToString());
		//ArrayFillTest();
        Points = new float[pointCount,3];
        //GetPoints(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
        height = Points.GetLength(0);
        width = Points.GetLength(1);
        GetInternalPoints(Points,height, width , worldRatio);
        Debug.Log(Points[0,0].ToString());
        for(int i = 0; i < pointCount; i++)
        {
            UnityEngine.Object tmp = Instantiate(particle, new Vector3(Points[i,0],Points[i,1],Points[i,2]) + parentPos, Quaternion.identity);
            /*UnityEngine.Object tmp = Instantiate(particle, new Vector3(GetInternalPoint(i,0),
                                                                        GetInternalPoint(i,1),
                                                                        GetInternalPoint(i,2)), 
                                                                        Quaternion.identity);*/
            tmp.name = "Sphere" + i.ToString();
            //allParticles.Add(i,(GameObject)tmp);
            allParticles.Add((GameObject)tmp);
        }
        
        Debug.Log(parentPos);
        
        Debug.Log("SPh state = " +InternalRunningState().ToString());
        InternalStartRunning();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
            
            int newPointCount = GetInternalLength();
            if(Input.GetKey(KeyCode.M))
            {
                newPointCount++;
                UnityEngine.Object tmp = Instantiate(particle, new Vector3(0,0,0) + parentPos, Quaternion.identity);
                tmp.name = "Sphere" + newPointCount.ToString();
                allParticles.Add((GameObject)tmp);
                Debug.Log("Particle Added");
                
                AddParticle(0.6f, 0.6f,0.6f,0,0,0);
            }
            
            if(Input.GetKeyDown(KeyCode.N))
            {
                for(float i = 0.6f; i>0.4 ;i-=0.03f)
                {
                    for(float j = 0.6f; j>0.4 ;j-=0.03f)
                    {
                        for(float k = 0.6f; k>0.4 ;k-=0.03f)
                        {
                            newPointCount++;
                            UnityEngine.Object tmp = Instantiate(particle, new Vector3(0,0,0) + parentPos, Quaternion.identity);
                            tmp.name = "Sphere" + newPointCount.ToString();
                            allParticles.Add((GameObject)tmp);
                            Debug.Log("Block Added");
                           AddParticle(i,j,k,0,0,0); 
                        }    
                    }
                }
            }
            if(newPointCount!=pointCount)
            {
                Points = new float[newPointCount,3];
                height = Points.GetLength(0);
                width = Points.GetLength(1);
                pointCount = GetInternalLength();
            }
            InternalAnimate();   
            GetInternalPoints(Points,height, width , worldRatio);
            for(int i = 0; i<pointCount; i++)
            {
                vec.Set((float)Points[i,0],(float)Points[i,1],(float)Points[i,2]);   
                allParticles[i].transform.position = (vec + parentPos);
            }
           
            
            //Debug.Log(pointCount);
	}
    
    static void CallBackFunction(string str)
    {
        Debug.Log("::CallBack : " + str);
    }
    
    Vector3 WorldToSimulationConversion(Vector3 position)
    {
        Vector3 output = position;
        
        return output;
    }
    
    void AddNewParticle(Vector3 position, Vector3 velocity)
    {
         AddParticle(position.x, position.y ,position.z,velocity.x,velocity.y,velocity.z);
    }
    
    List <Vector3> AddShape(GameObject shape)
    {
        return new List<Vector3>();
    }
   
}



