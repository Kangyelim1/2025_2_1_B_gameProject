using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : InteractableObject
{
    protected virtual void Start()
    {
        base.Start();
        objectName = "기계";
        interactipnText = "[E]기계 작동";
        interactionType = InteractionType.Machine;
    }

    protected override void OperateMachine()
    {
        StartCoroutine(DoOperateMachine());
    }

    IEnumerator DoOperateMachine()
    {
        for(int i = 0; i < 50; i++)
        {
          transform.Rotate(new Vector3(0, 0.1f, 0), 30);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.1f);
    }
}
   

