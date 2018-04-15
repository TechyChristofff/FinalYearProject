using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Testing : MonoBehaviour {

	
	bool testing = false;
	public GameObject Average;
	public GameObject Min;
	public GameObject Max;
	public GameObject TestTime;
	public GameObject Current;
	
	public GameObject Initial;
	public GameObject Renderer;
	
	public GameObject Pause;
	
	public GameObject Simulation;
	public GameObject Particle;
	public GameObject Metaball;
	public GameObject Blob;
	
	float min = float.MaxValue;
	float max = float.MinValue;
	float avg = 0;
	float noOfFrames;
	float totFrame;
	float testTime = 0;
	
	float FPS;

	public GameObject[] cameras;
	
	// Use this for initialization
	void Start () {
		#if UNITY_IOS
		Application.targetFrameRate = 120;
		#endif
	
	}
	
	// Update is called once per frame
	void Update () { 
		FPS = 1 / Time.unscaledDeltaTime;
		if(testing)
		{
			
			
			
			if(testTime>1)
			{
				noOfFrames++;
				
				if(FPS < min)
					min = FPS;
				
				if(FPS>max)
					max = FPS;
					
				avg = totFrame/noOfFrames;
				totFrame += FPS;
			}

			testTime += Time.unscaledDeltaTime;
		}
	
	}
	void LateUpdate()
	{
		Average.GetComponent<Text>().text = "Avg = " + avg.ToString();
		Min.GetComponent<Text>().text = "Min = " + min.ToString();
		Max.GetComponent<Text>().text = "Max = " + max.ToString();
		TestTime.GetComponent<Text>().text = "Time = " + testTime.ToString();
		Current.GetComponent<Text>().text = "FPS = " + FPS.ToString();
	}
	
	public void ToggleTest()
	{
		testing = !testing;
		
		
		if(!Simulation.activeSelf)
		{
			switch (Initial.GetComponent<Dropdown>().value)
			{
				case 0:
				{
					Simulation.GetComponent<Wrapper>().initalParticles = 500;
				}break;
				case 1:
				{
					Simulation.GetComponent<Wrapper>().initalParticles = 2500;
				}break;
				case 2:
				{
					Simulation.GetComponent<Wrapper>().initalParticles = 5000;
				}break;
				case 3:
				{
					Simulation.GetComponent<Wrapper>().initalParticles = 10000;
				}break;
				default:
				break;
			}
			
			Simulation.SetActive(true);
			
			switch (Renderer.GetComponent<Dropdown>().value)
			{
				case 0:
				{
					Particle.SetActive(true);
				}break;
				case 1:
				{
					Metaball.SetActive(true);
				}break;
				case 2:
				{
					Blob.SetActive(true);
				}break;
				default:
				break;
			}
			
			Pause.SetActive(true);
		}
	}

	public void SetCameraNumber(int cam)
	{
		if(cameras.Length > cam)
		{
			foreach(GameObject camera in cameras)
			{
				camera.SetActive(false);
			}
			cameras[cam].SetActive(true);
		}
	}
	
	public void Restart()
	{
		SceneManager.LoadScene("FluidSimulator");
	}
}
