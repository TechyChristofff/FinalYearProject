using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextEditor : MonoBehaviour {

    private string message;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void SetMessage(string input)
    {
        GetComponent<Text>().text = input;
    }
    
    public void SetNumber(float input)
    {
         GetComponent<Text>().text += " " + input.ToString();
    }
    
    public void SetNumber(int input)
    {
        GetComponent<Text>().text += " " + input.ToString();
    }
    
}
