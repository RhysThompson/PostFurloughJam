using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 MoveSpeed = new Vector3(0, 0, 10);
    void FixedUpdate()
    {
        this.transform.Translate(MoveSpeed * Time.fixedDeltaTime, Space.Self);
    }
}
