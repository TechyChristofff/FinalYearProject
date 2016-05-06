using UnityEngine;
using System.Collections;

public class RenderSwitch : MonoBehaviour {

    public GameObject[] offObj;
    
    public GameObject onObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Switch ()
    {
        
        foreach (GameObject item in offObj)
        {
            item.SetActive(false);
        }
        
        onObj.SetActive(true);
    }
}
