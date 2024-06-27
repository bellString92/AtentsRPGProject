using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : AnimatorProperty
{
    public LayerMask crashMask;
    Vector3 deltaPosition = Vector3.zero;
    Quaternion deltaRotation = Quaternion.identity;

    float radius = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        CapsuleCollider col = transform.parent.GetComponent<CapsuleCollider>();
        if (col != null)
        {
            radius = col.radius;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.parent.position, deltaPosition.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, deltaPosition.magnitude, crashMask))
        {
            transform.parent.position += ray.direction * (hit.distance - radius);
        }
        else
        {
            transform.parent.position += deltaPosition;
        }
        transform.parent.rotation *= deltaRotation;
        deltaPosition = Vector3.zero;
        deltaRotation = Quaternion.identity;
    }
    private void OnAnimatorMove()
    {
        deltaPosition += myAnim.deltaPosition;
        deltaRotation *= myAnim.deltaRotation;
    }
}