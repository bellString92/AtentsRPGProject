using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public LayerMask enemyMask;
    public UnityEvent<Transform> findAct;
    public UnityEvent lostAct;
    Transform myTarget = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & enemyMask) != 0)
        {
            if (myTarget == null)
            {
                myTarget = other.transform;
                findAct?.Invoke(other.transform);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (myTarget != null)
        {
            myTarget = null;
            lostAct?.Invoke();
        }
    }

    public void SetEnable(bool v)
    {
        gameObject.SetActive(v);
    }
}
