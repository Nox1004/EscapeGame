using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame;

[RequireComponent(typeof(BoxCollider))]
public class ClearBox : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Clear");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerUI.s_Instance.changeUI.Activate(1);
        }
    }
}
