using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface IDamage //피격
{
    void TakeDamage(float dmg);
}
interface ILive //생사여부
{
    bool IsLive { get; }
}
interface IBattle : IDamage, ILive
{

}



[System.Serializable]
public struct BattelState
{
    public float AttackRange;
    public float AttackPoint;
    public float AttackDelay;
    public float MaxHealthPoint;
    public float curHealthPoint;

    public float getHpValue()
    {
        return Mathf.Clamp(curHealthPoint, 0.0f, MaxHealthPoint) / (MaxHealthPoint == 0.0f ? 1.0f : MaxHealthPoint);
    }
}

public class BattleSystem : NaviMovement, IBattle
{
    public BattelState myBattleState;
    public UnityEvent deadAct;
    public UnityAction<float> changeHpAct;
    public Transform myTarget;

    float delrayTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsLive
    {
        get => myBattleState.curHealthPoint > 0.0f;
    }


    // 공격
    protected void OnAttack()
    {
        if (myAnim.GetBool("IsAttacking") == false)
        {
            if (delrayTimer >= myBattleState.AttackDelay)
            {
                delrayTimer = 0.0f;
                myAnim.SetTrigger("OnAttack");
            }
        }
    }
    public void OnAttackTarget()
    {

        if (Vector3.Distance(myTarget.position, transform.position) <= myBattleState.AttackRange)
        {
            myTarget.GetComponent<IDamage>()?.TakeDamage(myBattleState.AttackPoint);
        }
    }
    protected void BattleUpdate()
    {
        if (myAnim.GetBool("IsAttacking") == false)
        {
            delrayTimer += Time.deltaTime;
        }
    }


    //피격
    public void TakeDamage(float dmg)
    {
        myBattleState.curHealthPoint -= dmg;
        changeHpAct?.Invoke(myBattleState.getHpValue());

        if (myBattleState.curHealthPoint <= 0.0f)
        {
            myAnim.SetTrigger("OnDead");
            OnDead();
        }
        else
        {
            myAnim.SetTrigger("onDamage");
        }
    }
    protected virtual void OnDead()
    {

    }
}
