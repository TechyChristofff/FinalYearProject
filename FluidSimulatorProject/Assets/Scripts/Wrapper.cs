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
    
    #endregion 

    IntPtr SPH_System;
    
    public GameObject particle;
    
    float[,] Points;
    int pointCount;
    Dictionary<int,GameObject> allParticles;
    public int size = 500;
    /*
	[DllImport("FluidUnityPlugin", SetLastError = true)]
	private static extern int[,] fillArray(int size);

    */
    
	// Use this for initialization
	void Start () {
        
         MyDelegate callback_delegate = new MyDelegate( CallBackFunction );
 
        // Convert callback_delegate into a function pointer that can be
        // used in unmanaged code.
        IntPtr intptr_delegate =
            Marshal.GetFunctionPointerForDelegate(callback_delegate);
        
        // Call the API passing along the function pointer.
        SetDebugFunction( intptr_delegate );
        
        allParticles = new Dictionary<int, GameObject>();
        //SPH_System = InitOpenSPHSystem();
        bool test = InitInternalSystem();
        Debug.Log(test);
        pointCount = GetInternalLength();
        //pointCount = GetLength(SPH_System);
        Debug.Log(pointCount.ToString());
		//ArrayFillTest();
        Points = new float[pointCount,3];
        //GetPoints(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
        GetInternalPoints(Points,Points.GetLength(0), Points.GetLength(1) );
        Debug.Log(Points[0,0].ToString());
        foreach(double point in Points)
        {
            //Debug.Log(point.ToString());
        }
        for(int i = 0; i < pointCount; i++)
        {
            UnityEngine.Object tmp = Instantiate(particle, new Vector3(Points[i,0],Points[i,1],Points[i,2]), Quaternion.identity);
            /*UnityEngine.Object tmp = Instantiate(particle, new Vector3(GetInternalPoint(i,0),
                                                                        GetInternalPoint(i,1),
                                                                        GetInternalPoint(i,2)), 
                                                                        Quaternion.identity);*/
            tmp.name = "Sphere" + i.ToString();
            allParticles.Add(i,(GameObject)tmp);
        }
        Debug.Log("SPh state = " +InternalRunningState().ToString());
        InternalStartRunning();
	}
	
	private void ArrayFillTest() {
		var start = Time.realtimeSinceStartup;
		
		//int[,] tab = fillArray(size);
		Debug.Log( (Time.realtimeSinceStartup-start).ToString("f6") + " secs");
		
		start = Time.realtimeSinceStartup;
		int[,] array = new int[size,size];
		for(int i = 0; i < size; i++) {
			for(int j = 0; j < size; j++) {
				array[i,j] = i * size + j;
			}
		}
		Debug.Log( (Time.realtimeSinceStartup-start).ToString("f6") + " secs");
	}
	
	// Update is called once per frame
	void Update () {
        //SPH_System = Animate1(SPH_System);
        //GetPoints(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));

            InternalAnimate();
            //Debug.Log("SPh state = " +InternalRunningState().ToString());
            //Points = new float[pointCount,3];   
            GetInternalPoints(Points,Points.GetLength(0), Points.GetLength(1) );
            //Animate2(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
            //Debug.Log("Do Animate");
            //Debug.Log("Point 0,1 = " + Points[0,1].ToString());
            //Debug.Log("Point 3,1 = " + Points[3,1].ToString());
            //Debug.Log("Point 8,1 = " + Points[8,1].ToString());
            //Debug.Log("Point 10,2 = " + Points[10,2].ToString());
            
            for(int i = 0; i<GetInternalLength(); i++)
            {
                allParticles[i].transform.position = new Vector3((float)Points[i,0],-(float)Points[i,1],(float)Points[i,2]);
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



