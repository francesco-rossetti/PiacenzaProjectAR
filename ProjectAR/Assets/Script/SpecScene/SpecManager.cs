using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecManager : ChangeSceneManager {

    // Use this for initialization
    private GameObject Variable;
    private APIManager Api;
    void Start () {
       // Variable = GameObject.Find("GameVariable").gameObject;
        Api = new APIManager();
	}
	
	// Update is called once per frame
	void Update () {
        //Api.GetMonument(1);//(Variable.GetComponent<VariableScript>().idMonument);
	}
}
