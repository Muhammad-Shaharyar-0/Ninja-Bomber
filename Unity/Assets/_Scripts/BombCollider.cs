using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollider : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {

        gameObject.GetComponent<Collider>().isTrigger = false;
    }
 

}
