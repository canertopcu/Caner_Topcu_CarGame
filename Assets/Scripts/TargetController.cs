using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    // Start is called before the first frame update
    public void Show(bool state)
    {
        GetComponent<Renderer>().enabled = state;
    }

    
}
