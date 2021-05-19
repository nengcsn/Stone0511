using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //射线碰撞的石头
    public Stone1 s;
    public Stack<Stone1> stoneStack = new Stack<Stone1>();
    // Start is called before the first frame update
    void Start()
    {
        s.a.transform.position = stoneStack.Peek().b.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
