using UnityEngine;
using System.Collections;

public class colisao : MonoBehaviour {
    public int intencidade;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(intencidade, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("qualquer coisa");
        
        GetComponent<Rigidbody2D>().AddForce(new Vector3(-(2*intencidade), 0, 0));
        intencidade *= (-1);
    }

}
