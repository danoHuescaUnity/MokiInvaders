using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField]
    private float timeToDestroy = 2.0f;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
