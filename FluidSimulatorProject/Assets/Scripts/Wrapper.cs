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
    private static extern void GetInternalPoints(float[,] array, int height, int width); //Return the point data from the system [Point][x,y,z]
    
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
    private static extern bool InitVariableSystem(int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, float worldSizeY,float worldSizeZ,float wallDampening,float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, float surfaceCoeffeciant, float poly6Val1, float poly6Val2,float poly6Val3, float spikyVal1,float spikyVal2,float viscoVal1, float grad6Polyval1,float grad6Polyval2,float grad6Polyval3,float viscoVal2, float lpcVal1, float lpcVal2, float lpcVal3);
    
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
    public float poly6Val1 = 315.0f; //315
    public float poly6Val2 = 64.0f; //64
    public float poly6Val3 = 9.0f; //9 
    public float spikyVal1 = -45.0f; //-45
    public float spikyVal2 = 6.0f; //6
    public float viscoVal1 = 45.0f; //45 
    public float viscoVal2 = 6.0f; //6
    public float grad6Polyval1 = -945.0f; //-945
    public float grad6Polyval2 = 32.0f; //32
    public float grad6Polyval3 = 9.0f; //9
    public float lpcVal1 = -945.0f; //-945 
    public float lpcVal2 = 8.0f; //8 
    public float lpcVal3 = 9.0f; //9
    
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
        if(!managed)
        {
            test = InitInternalSystem();
        }else
        {
            test = InitVariableSystem(maxParticles, kernelInput,  massInput,  gravX,  gravY,  gravZ,  worldSizeX,  worldSizeY, worldSizeZ, wallDampening, restDencity, gasConstant,  viscosityInput,  timeStep,  surfaceNormals,  surfaceCoeffeciant,  poly6Val1,  poly6Val2, poly6Val3,  spikyVal1, spikyVal2, viscoVal1,  grad6Polyval1, grad6Polyval2, grad6Polyval3, viscoVal2,  lpcVal1,  lpcVal2,  lpcVal3);
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
        GetInternalPoints(Points,height, width );
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
	void Update () {

            InternalAnimate();   
            GetInternalPoints(Points,height, width );
             
            for(int i = 0; i<pointCount; i++)
            {
                vec.Set((float)Points[i,0],-(float)Points[i,1],(float)Points[i,2]);   
                allParticles[i].transform.position = (vec + parentPos);
            }
	}
    
    static void CallBackFunction(string str)
    {
        Debug.Log("::CallBack : " + str);
    }
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern IntPtr InitSPHSystem(int maxParticles);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern IntPtr InitOpenSPHSystem();

    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void DisposeSPHSystem(IntPtr pObject);

    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern IntPtr Animate(IntPtr pObject);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void Animation(IntPtr pObject);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern IntPtr Animate1(IntPtr pObject);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void Animate2 (IntPtr pObject, double[,] arrayin, int height, int width);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern int GetLength(IntPtr pObject);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void GetPoints(IntPtr pObject, float[,] array, int height, int width);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void ChangePoints(IntPtr pObject, double[,] array, int height, int width);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern int GetRunState(IntPtr pObject);
    
    [DllImport("FluidUnityPlugin", SetLastError = true)]
    private static extern void StartRunning(IntPtr pObject);
}



