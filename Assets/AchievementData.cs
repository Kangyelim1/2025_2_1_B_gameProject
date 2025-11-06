using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achivevment" , menuName = "Achiveement/Achievement Data")]
public class AchievementData : MonoBehaviour
{
    public string achivevmentName;
    public string description;
    public AchievementType achievementType;
    public int requiredAmount;       
    public int rewardCoins;       
    public bool isUnlocked;         
    public Sprite icon;
}
