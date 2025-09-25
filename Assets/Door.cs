using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door : InteractableObject
{

    [Header("문 설정")]
    public bool isOpen = false;
    public Vector3 openPosition;
    public float openSpeed = 2f;

    private Vector3 closedPosition;

    protected override void Start()
    {
        base.Start();
        objectName = "문";
        interactipnText = "[E]문 열기";
        interactionType = InteractionType.Building;

        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.right * 3f;
    }

    protected override void AccessBuilding()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            interactipnText = "[E]문 닫기";
            StartCoroutine(MoveDoor(closedPosition));
        }
        else
        {
            interactipnText = "[E]문 열기";
            StartCoroutine(MoveDoor(closedPosition));
        }
    }

    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, openSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
