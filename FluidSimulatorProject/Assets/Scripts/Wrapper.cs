using UnityEngine;
using System;
using System.Runtime.InteropServices;



public class Wrapper : MonoBehaviour {

    #region Internal Calls
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern bool InitInternalSystem(); //Initialise the system
    
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern void GetInternalPoints(float[,] array, int height, int width, float worldRatio); //Return the point data from the system [Point][x,y,z]
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern void InternalAnimate();//Animate the system
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern void InternalDispose();//Dispose of the system
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern int GetInternalLength();//Get the number of SimulationPoints in the system
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern void InternalStartRunning();//Start the system running/pause the system
      
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern int InternalRunningState();//Get the running state (0 = paused, 1 = running)
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern float GetInternalPoint(int id, int direction);//Get a specific point (point number, 0=x, 1=Y,2=Z)
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern bool InitVariableSystem(int initialParticles, int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, 
    float worldSizeY,float worldSizeZ,float wallDampening,float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, 
    float surfaceCoeffeciant);
    
    #if UNITY_IOS
    [DllImport("__Internal")]
    #else
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    #endif
    private static extern void AddParticle(float PosX, float PosY, float PosZ, float VelX, float VelY, float VelZ );
    
    /*
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern bool InitInternalSystem(); //Initialise the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void GetInternalPoints(float[,] array, int height, int width, float worldRatio); //Return the point data from the system [Point][x,y,z]
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void InternalAnimate();//Animate the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void InternalDispose();//Dispose of the system
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern int GetInternalLength();//Get the number of SimulationPoints in the system
    
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
    */
    
    
    /*
    [DllImport("__Internal")]
    private static extern bool InitInternalSystem(); //Initialise the system
    
    [DllImport("__Internal")]
    private static extern void GetInternalPoints(float[,] array, int height, int width, float worldRatio); //Return the point data from the system [Point][x,y,z]
    
    [DllImport("__Internal")]
    private static extern void InternalAnimate();//Animate the system
    
    [DllImport("__Internal")]
    private static extern void InternalDispose();//Dispose of the system
    
    [DllImport("__Internal")]
    private static extern int GetInternalLength();//Get the number of SimulationPoints in the system
    
    [DllImport("__Internal")]
    private static extern void InternalStartRunning();//Start the system running/pause the system
      
    [DllImport("__Internal")]
    private static extern int InternalRunningState();//Get the running state (0 = paused, 1 = running)
    
    [DllImport("__Internal")]
    private static extern float GetInternalPoint(int id, int direction);//Get a specific point (point number, 0=x, 1=Y,2=Z)
    
    [DllImport("__Internal")]
    private static extern bool InitVariableSystem(int initialParticles, int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, 
    float worldSizeY,float worldSizeZ,float wallDampening,float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, 
    float surfaceCoeffeciant);
    
    [DllImport("__Internal")]
    private static extern void AddParticle(float PosX, float PosY, float PosZ, float VelX, float VelY, float VelZ );
    */
   
    public delegate void MyDelegate(string str);
    
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    public static extern void SetDebugFunction( IntPtr fp );    
    
    #endregion 
    public bool managed;
    float[,] SimulationPoints;
    Vector3 vec = new Vector3();
    int width, height;
    public int initalParticles = 3000; //3000
    public int maxParticles = 30000; //30000
    public float kernelInput = 0.04f; //0.04
    public float massInput = 0.02f; //0.02
    public float gravX = 0f; //0
    public float gravY = -9.81f; //-9.81
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
         
        //InitialiseCallbackSystem();
        
        InitiliseSystem();
        
        GetInternalPoints(SimulationPoints,height, width , worldRatio);
        //Debug.Log(SimulationPoints[0,0].ToString());
        
        Debug.Log("SPh state = " +InternalRunningState().ToString());
        
	}
    
    public void InitiliseSystem()
    {
        bool test = false;
		if (!managed) {
			test = InitInternalSystem ();
		} else {
			test = InitVariableSystem (initalParticles, maxParticles, kernelInput, massInput, gravX, gravY, gravZ, worldSizeX, worldSizeY, worldSizeZ, wallDampening, restDencity, 
				gasConstant, viscosityInput, timeStep, surfaceNormals, surfaceCoeffeciant);
        }
        
        Debug.Log(test);
        
        Debug.Log(GetInternalLength().ToString());
        SimulationPoints = new float[GetInternalLength(),3];
        height = SimulationPoints.GetLength(0);
        width = SimulationPoints.GetLength(1);
    }
	
	// Update is called once per frame
	void Update () {
            
        int newPointCount = SimulationPoints.Length;
        if(Input.GetKey(KeyCode.M))
        {
            newPointCount++;
            Debug.Log("Particle Added");           
            AddParticle(0.6f, 0.6f,0.6f,0,0,0);
            UpdatePointArray();
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
                        Debug.Log("Block Added");
                        AddParticle(i,j,k,0,0,0); 
                    }    
                }
            }
            UpdatePointArray();
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
            InternalStartRunning();
	}
    
    void UpdatePointArray()
    {
        SimulationPoints = new float[GetInternalLength(),3];
        height = SimulationPoints.GetLength(0);
        width = SimulationPoints.GetLength(1);
    }
    
    void FixedUpdate()
    {
        InternalAnimate();   
        GetInternalPoints(SimulationPoints,height, width , worldRatio);
        Debug.Log("SPh state = " +InternalRunningState().ToString());
    }
    
    void InitialiseCallbackSystem()
    {
        MyDelegate callback_delegate = new MyDelegate( CallBackFunction );
        // Convert callback_delegate into a function pointer that can be
        // used in unmanaged code.
        IntPtr intptr_delegate =
            Marshal.GetFunctionPointerForDelegate(callback_delegate);       
        // Call the API passing along the function pointer.
        SetDebugFunction( intptr_delegate );
    }
    
    public float[,] Points {get {return SimulationPoints;}}
    
    public int SimulationPointCount{ get{
        return GetInternalLength();
    }}
    
    public void ToggleSimulation()
    {
        InternalStartRunning();
        Debug.Log("SPh state = " +InternalRunningState().ToString());
    }
    
    static void CallBackFunction(string str)
    {
        Debug.Log("::CallBack : " + str);
    }
    
    public Vector3 WorldToSimulationConversion(Vector3 position)
    {
        Vector3 output = position;
        
        return output;
    }
    
    public void AddNewParticle(Vector3 position, Vector3 velocity)
    {
         AddParticle(position.x, position.y ,position.z,velocity.x,velocity.y,velocity.z);
    }
    
    public bool AddShape(GameObject shape)
    {
        return false;
    }
   
}

