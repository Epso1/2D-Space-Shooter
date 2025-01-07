using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] public float bgVelocity = 2f;
    private Vector3 direction;

    void Start()
    {
        StartMoving();
    }

    void Update()
    {
        transform.position += direction * bgVelocity * Time.deltaTime;
    }

    public void StartMoving()
    {
        direction = -Vector3.right;
    }

    public void StopMoving()
    {
        direction = Vector3.zero;
    }

}
