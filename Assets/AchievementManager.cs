using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [Header("Achievement Settings")]
    public List<AchievementData> allAchievements = new List<AchievementData>();

    [Header("UI References")]
    public GameObject achievementPopupPrefab;
    public Transform popupParent;
    public GameObject achievementPanel;
    public Transform achievementListContent;
    public GameObject achievementSlotPrefab;

    private Dictionary<AchievementType, int> progressData = new Dictionary<AchievementType, int>();

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetAllAchievements(); // 시작시에 리셋 강제로 (테스트용) 나중에 배포시에는 지운다. (Forced reset on start (for testing). Delete later for deployment.)

        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType))) // //각 타입별 초기화 (Initialize by each type)
        {
            progressData[type] = 0;
        }

        LoadAchievements();
        UpdateAchievementUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetProgress(AchievementData achievement) // 진행도 가져오기 (Get progress)
    {
        if (achievement.isUnlocked) return 1f;

        int current = progressData.ContainsKey(achievement.achievementType) ? progressData[achievement.achievementType] : 0;

        return Mathf.Min(((float)current / achievement.requiredAmount), 1f);
    }

    public void UpdateAchievementUI() // 업적 UI 업데이트 함수 (Achievement UI Update Function)
    {
        if (achievementListContent == null || achievementSlotPrefab == null) return; // 두 내용 중 하나라도 없으면 오류 때문에 return (If either of the two contents is missing, return to prevent an error)

        // 기존 슬롯 제거 (Remove existing slots)
        foreach (Transform child in achievementListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            GameObject slot = Instantiate(achievementSlotPrefab, achievementListContent);
            AchievementSlot slotScript = slot.GetComponent<AchievementSlot>();

            if (slotScript != null)
            {  
                slotScript.SetAchievement(achievement, GetProgress(achievement));
            }
        }
    }

    void SaveAchievements() // //데이터 저장 (Save data)
    {
        foreach (var kvp in progressData)
        {
            PlayerPrefs.SetInt("Achievemnt_" + kvp.Key, kvp.Value);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            PlayerPrefs.SetInt("Unlocked_" + achievement.name, achievement.isUnlocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    void LoadAchievements() // //데이터 로드 (Load data)
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = PlayerPrefs.GetInt("Achievemnt_" + type, 0);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("Unlocked_" + achievement.name, 0) == 1;
        }
    }

    public void ResetAllAchievements() 
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
            PlayerPrefs.DeleteKey("Achievemnt_" + type);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Unlocked_" + achievement.name);
        }

        PlayerPrefs.Save();
        UpdateAchievementUI();
    }

    void ShowAchievementPopup(AchievementData achievement)
    {
        if(achievementPopupPrefab != null && popupParent != null)
        {
            GameObject popup = Instantiate(achievementPopupPrefab, popupParent);

            Text titleText = popup.transform.Find("Title")?.GetComponent<Text>();
            Text descText = popup.transform.Find("Description")?.GetComponent<Text>();

            if (titleText != null) titleText.text = "업적 달성"; // Achievement Unlocked (Korean)
            if (descText != null) descText.text = achievement.achivevmentName;

            Destroy(popup, 3.0f);
        }
    }

    void UnlockAchievement(AchievementData achievement) // //업적 언락 (Achievement Unlock)
    {
        achievement.isUnlocked = true;
        // 보상이 있는 업적일 경우 보상 로직을 여기에 넣는다. (If it's an achievement with a reward, put the reward logic here.)

        ShowAchievementPopup(achievement);
        UpdateAchievementUI();
    }

    public void UpdateProgress(AchievementType type, int amount = 1) // //진행도 업데이트 (Update progress)
    {
        progressData[type] += amount;

        foreach (AchievementData achievement in allAchievements) // //해당 타입이 모든 업적 체크 (Check all achievements of that type)
        {
            if (achievement.achievementType == type && !achievement.isUnlocked)
            {
                if (progressData[type] >= achievement.requiredAmount)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }
}
