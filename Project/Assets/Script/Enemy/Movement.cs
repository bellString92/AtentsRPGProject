using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : AnimatorProperty
{
    public float moveSpeed = 1.0f;
    public float RotSpeed = 720.0f;
    protected Coroutine coMove = null;
    Coroutine coRotat = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovingToPos(Vector3 pos, UnityAction done) // 지정된 위치로 이동시키기 위한 함수
    {
        myAnim.SetBool("IsMoving", true);
        Vector3 moveDir = pos - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        coRotat = StartCoroutine(Rotating(moveDir));

        while (moveDist > 0.0f)
        {
            if (myAnim.GetBool("IsAttacking") == false)
            {
                float delta = moveSpeed * Time.deltaTime;
                if (delta > moveDist) delta = moveDist;
                transform.Translate(moveDir * delta, Space.World);
                moveDist -= delta;
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }
    IEnumerator Rotating(Vector3 dir) // 
    {
        float angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f)
        {
            rotDir = -1.0f;
        }
        while (angle > 0.0f)
        {
            if (myAnim.GetBool("IsAttacking") == false)
            {
                float delta = RotSpeed * Time.deltaTime;
                if (delta > angle) delta = angle;
                transform.Rotate(Vector3.up * rotDir * delta);
                angle -= delta;
            }
            yield return null;
        }
    }
    public void StopMoveingCoroutine()
    {
        if (coMove != null)
        {
            StopCoroutine(coMove);
            coMove = null;
        }
        if (coRotat != null)
        {
            StopCoroutine(coRotat);
            coRotat = null;
        }
        myAnim.SetBool("IsMoving", false);
    }

    public Coroutine MoveToPos(Vector3 pos, UnityAction done)
    {
        StopMoveingCoroutine();
        return coMove = StartCoroutine(MovingToPos(pos, done));
    }

}
