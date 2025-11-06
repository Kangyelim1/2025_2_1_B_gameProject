using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoinItem : InteractableObject
{
    [Header("µ¿Àü ¼³Á¤")]
    public int coinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "µ¿Àü";
        interactipnText = "[E] µ¿Àü È¹µæ";
        interactionType = InteractionType.Item; 
    }

    protected override void CollectItem()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.AddCollectProgress(questTag);
        }

        AchievementManager.instance?.UpdateProgress(AchievementType.CollectCoins, coinValue);


        Destroy(gameObject);
    }
}
