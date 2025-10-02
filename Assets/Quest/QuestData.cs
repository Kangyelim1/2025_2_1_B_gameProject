using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quset", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("기본 정보")]
    public string questTitle = "세로운 퀘스트";

    [TextArea(2, 4)]
    public string description = "퀘스트 설명을 압력하세요";
    public Sprite questIcon;

    [Header("퀘스트 설정")]
    public QuestType questType;
    public int targetAmount = 1;

    [Header("배달 퀘스트용 (Delivery")]
    public Vector3 deliveryPosition;
    public float deliveryRadius = 3f;

    [Header("수집/상호작용 퀘스트용")]
    public string targetTag = "";

    [Header("보상")]
    public int experienceReward = 100;
    public string rewardMessage = "퀘스트 완료!";

    [Header("퀘스트 연결")]
    public QuestData nextQuest;

    [System.NonSerialized]public int currentProgresss = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    public void Initalize()
    {
        currentProgresss = 0;
        isActive = false;
        isCompleted = false;
    }

    public bool IsComplete()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return currentProgresss >= 1;

            case QuestType.Collect:
            case QuestType.Interect:
                return currentProgresss >= targetAmount;

            default:
                return false;
        }
    }

    public float GetProgressPercentage()
    {
        if(targetAmount <= 0) return 0f;
        return Mathf.Clamp01((float)currentProgresss / targetAmount);
    }

    public string GetProgressText()
    {
        switch(questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "배달 완료" : "목적지로 이동하세요";
            case QuestType.Collect:
            case QuestType.Interect:
                return $"{currentProgresss} / {targetAmount}";
            default:
                return "";
        }
    }
}
