using UnityEngine;
using System.Collections;

public class parabola : MonoBehaviour {

    public float c;
    public float b;
    private Transform t;
    private Vector3 a;
    private bool vez = true;
    // Use this for initialization
    void Awake()
    {

        t = GetComponent<Transform>();
        a = t.position;
        //Parabola();


    }

    void Parabola()
    {
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {


        for (float x = b; x >= -b; x = x - 0.01f)
        {
            float y = ((c * (x * x)) / (b * b)) - c;
            t.position = new Vector3(a.x + x - b, a.y + y, a.z);
            yield return new WaitForSeconds(0.02f);
        }
        t = GetComponent<Transform>();
        a = t.position;
        vez = true;


    }


    // Update is called once per frame
    void Update()
    {
        if (vez)
        {
            Parabola();
            vez = false;
        }

    }
}
