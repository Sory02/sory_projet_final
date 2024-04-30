using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime;
        
    }
}
