using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class NaviMovement : Movement
{ 
    NavMeshPath myPath;
    Coroutine move = null;


    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    protected void DrawPath() //네비게이션을 이용해 위치 이동
    {
        if (myPath != null)
        {
            for (int i = 0; i < myPath.corners.Length - 1; ++i)
            {
                Debug.DrawLine(myPath.corners[i], myPath.corners[i + 1], Color.red);
            }
        }
    }
    public new void MoveToPos(Vector3 pos, UnityAction done)
    {
        if (myPath == null) myPath = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, myPath))
        {
            switch (myPath.status)
            {
                case NavMeshPathStatus.PathComplete:
                case NavMeshPathStatus.PathPartial:
                    if (move != null)
                    {
                        StopCoroutine(move);
                        move = null;
                    }
                    move = StartCoroutine(MovingByPath(myPath.corners, done));
                    break;
                case NavMeshPathStatus.PathInvalid:
                    break;
            }
        }
        else
        {
            done?.Invoke();
        }
    }
    IEnumerator MovingByPath(Vector3[] path, UnityAction done)
    {
        int curIdx = 1;
        while (curIdx < path.Length)
        {
            yield return base.MoveToPos(path[curIdx++],null);
        }
        done?.Invoke();
    }

    // 추적
    public void FollowTarget(Transform target, CheckAction<float> checkAct,
        UnityAction act)
    {
        if (coMove != null)
        {
            StopCoroutine(coMove);
            coMove = null;
        }
        coMove = StartCoroutine(FollowingTarget(target, checkAct, act));
    }
   
}
