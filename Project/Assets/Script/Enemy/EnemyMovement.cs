using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : NaviMovement
{
    public enum State { Create, Normal, Battle, Death }
    public State myState = State.Create;
    Vector3 startPos;
    Coroutine coRoam = null;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        OnChangeState(State.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StopRoamCorutine()
    {
        if (coRoam != null)
        {
            StopCoroutine(coRoam);
            coRoam = null;
        }
    }

    void OnChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                StopMoveingCoroutine();
                StopRoamCorutine();
                coRoam = StartCoroutine(Roaming());
                break;
            case State.Battle:

                break;
            case State.Death:

                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.Normal:
                break;
            case State.Battle:
                break;
            case State.Death:
                break;
        }
    }

    IEnumerator Roaming()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        Vector3 dir = Vector3.forward;
        dir = Quaternion.Euler(0, Random.Range(0.0f, 360), 0) * dir;
        dir = dir * Random.Range(1.0f, 5.0f);
        Vector3 pos = startPos + dir;

        MoveToPos(pos, () =>
        {
            StopRoamCorutine();
            coRoam = StartCoroutine(Roaming());
        });

    }
}
