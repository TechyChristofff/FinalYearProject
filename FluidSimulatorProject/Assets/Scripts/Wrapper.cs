using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Wrapper : MonoBehaviour {

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
        allParticles = new Dictionary<int,GameObject>();
        SPH_System = InitOpenSPHSystem();
        pointCount = GetLength(SPH_System);
        Debug.Log(pointCount.ToString());
		//ArrayFillTest();
        Points = new float[pointCount,3];
        GetPoints(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
        Debug.Log(Points[0,0].ToString());
        foreach(double point in Points)
        {
            //Debug.Log(point.ToString());
        }
        for(int i = 0; i < pointCount; i++)
        {
            UnityEngine.Object tmp = Instantiate(particle, new Vector3(Points[i,0],Points[i,1],Points[i,2]), Quaternion.identity);
            tmp.name = "Sphere" + i.ToString();
            allParticles.Add(i,(GameObject)tmp);
        }
        Debug.Log("SPh state = " +GetRunState(SPH_System).ToString());
        StartRunning(SPH_System);
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
	void FixedUpdate () {
        //SPH_System = Animate1(SPH_System);
        GetPoints(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
        Debug.Log("Point 0,0 = " + Points[0,0].ToString());
        //Animate2(SPH_System, Points, Points.GetLength(0), Points.GetLength(1));
        Debug.Log("Do Animate");
        Debug.Log("SPh state = " + GetRunState(SPH_System).ToString());
        Debug.Log("Point 0,0 = " + Points[0,0].ToString());
        //Debug.Log("Point 3,1 = " + Points[3,1].ToString());
        //Debug.Log("Point 8,1 = " + Points[8,1].ToString());
        //Debug.Log("Point 10,2 = " + Points[10,2].ToString());
	    for(int i = 0; i<pointCount; i++)
        {
            allParticles[i].transform.position.Set((float)Points[i,0],(float)Points[i,1],(float)Points[i,2]);
        }
	}
}
