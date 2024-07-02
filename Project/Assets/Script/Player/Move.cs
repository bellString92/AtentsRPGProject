using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : AnimatorProperty
{
    public LayerMask enemyMask;
    Vector2 inputDir = Vector2.zero;
    Vector2 desireDiir = Vector2.zero;
    bool IsComboCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        desireDiir.x = Input.GetAxis("Horizontal");
        desireDiir.y = Input.GetAxis("Vertical");

        inputDir = Vector2.Lerp(inputDir, desireDiir, Time.deltaTime * 10.0f);

        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Roll");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            myAnim.SetFloat("Run", 2);
        }
        else
        {
            myAnim.SetFloat("Run", 1);
        }
        if (Input.GetMouseButton(0))
        {
            myAnim.SetTrigger("BasicCombo");
        }
    }

    public void ComboCheckStart()
    {
        StartCoroutine(ComboCheck());
    }

    IEnumerator ComboCheck()
    {
        myAnim.SetBool("ComboCheck", false);
        IsComboCheck = true;
        while (IsComboCheck)
        {
            if (Input.GetMouseButton(0))
            {
                myAnim.SetBool("ComboCheck", true);
            }
            IsComboCheck = false;
            yield return null; 
        }
    }

    public void ComboCheckEnd()
    {
        myAnim.SetBool("ComboCheck", true);
    }
}
