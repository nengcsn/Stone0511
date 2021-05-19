using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_movement : MonoBehaviour
{
    //Set ray collides with the object
    private GameObject position;
    
    public Transform Top;
    public Stack<Stone1> stoneStack = new Stack<Stone1>();
    // Use this for initialization  
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //Create a ray when click the left mouse button  
        if (Input.GetMouseButtonDown(0))
        {
            //Defining rays 
            Ray m_ray;
            //Save collision information
            RaycastHit m_hit;
            //Create a ray from the camera that passes through a mouse point on the screen
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Judge whether the ray collides with the object

            if (Physics.Raycast(m_ray, out m_hit))
            {
                position = m_hit.transform.gameObject;

                //Vector3 a = A.transform.position;
                //Debug.Log(A);
                //Transform orm = a;
                var stone = position.GetComponent<Stone1>();
                Transform A = stone.a.transform;
                if (stoneStack.Count <= 0)
                {
                    A.transform.SetParent(Top);
                    A.transform.localPosition = new Vector3(0, 0, 0);
                    A.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    stoneStack.Push(stone);
                }
                else
                {
                    Stone1 ac = stoneStack.Peek();
                    Vector3 bpos = ac.b.transform.position;
                    A.transform.SetParent(Top);

                    A.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    A.transform.position = bpos;
                    stoneStack.Push(stone);
                }

            }
        }
      
    }

    
}
