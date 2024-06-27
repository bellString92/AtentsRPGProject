using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperty : MonoBehaviour
{
    Camera _cam = null;
    protected Camera myCam
    {
        get
        {
            if (_cam == null)
            {
                _cam = GetComponentInChildren<Camera>();
            }
            return _cam;
        }
    }
}
