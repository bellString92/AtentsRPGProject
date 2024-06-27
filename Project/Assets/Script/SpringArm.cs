using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : CameraProperty
{
    public LayerMask crashMask;
    public float rotSpeed = 10.0f;
    public Vector2 rotRange = new Vector2(-60, 80);
    public float zoomSpeed = 3.0f;
    Vector3 curRot;
    float camDist;
    public Vector2 zoomRange = new Vector2(0.5f, 8.0f);
    public float camOffset = 0.5f;
    float desireDist;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 curRot = transform.localRotation.eulerAngles;
        desireDist = camDist = -myCam.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Mouse X");
        float y = -Input.GetAxis("Mouse Y");

        transform.parent.Rotate(Vector3.up * x * rotSpeed);
        //transform.Rotate(Vector3.right * y * (rotSpeed/2));

        curRot.x = Mathf.Clamp(curRot.x + y * rotSpeed, rotRange.x, rotRange.y); //최소치 최대치
        transform.localRotation = Quaternion.Euler(curRot);

        desireDist = Mathf.Clamp(desireDist - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, zoomRange.x, zoomRange.y);

        camDist = Mathf.Lerp(camDist, desireDist, Time.deltaTime * 2.0f);


        Ray ray = new Ray(transform.position, -transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, camDist + camOffset, crashMask))
        {
            camDist = hit.distance - camOffset;
        }

        myCam.transform.localPosition = new Vector3(0, 0, -camDist);

    }

}
