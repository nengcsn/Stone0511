using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone1 : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    private void Awake()
    {
        a.GetComponent<BoxCollider>().enabled = false;
        b.GetComponent<BoxCollider>().enabled = false;
        a.GetComponent<MeshRenderer>().enabled = false;
        b.GetComponent<MeshRenderer>().enabled = false;
    }
}
