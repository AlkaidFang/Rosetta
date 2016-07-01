using UnityEngine;
using System.Collections;

public class Other : MonoBehaviour {
	public GameObject go;

	// Use this for initialization
	void Start () {
		Base b = go.GetComponent<Base> ();
		Debug.Log(b.GetName ());
		TestFather TF = (TestFather)b;
		Debug.Log(TF.GetName() + TF.name);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
