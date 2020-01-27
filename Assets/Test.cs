using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float minimum = 10.0F;
    public float maximum = 20.0F;

    [ContextMenu("Test")]
    private void Start()
    {
        Debug
            .Log(FindObjectOfType<LightProbeGroup>().name);
    }

    void Update()
    {
        // transform.position = new Vector3(Mathf.Lerp(minimum, maximum, Time.time), 0, 0);
    }
}
