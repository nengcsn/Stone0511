using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class start : MonoBehaviour
{
    public GameObject Line;
    private Button But;
    // Start is called before the first frame update
    void Start()
    {
      But = this.GetComponent<Button>();
        But.onClick.AddListener(btn_event);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void btn_event()
    {
        Line.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
