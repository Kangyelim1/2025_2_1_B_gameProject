using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI 요소들")]
    public GameObject quesUI;
    public Text questTitleText;
    public Text questDescriptionText;
    public Text quesProgressText;
    public Button completButton;

    [Header("퀘스트 목록")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0; 

    void UpdateQuestUI()
    {
        if (currentQuest == null) return;
        
        if (questTitleText != null)
        {
            questTitleText.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (quesProgressText != null)
        {
           questTitleText.text = currentQuest.GetProgressText();
        }
    }

    public void StartQuest(QuestData quest)
    {
        if(quest == null) return;

        currentQuest = quest;
        currentQuest.Initalize();
        currentQuest.isActive = true;

        Debug.Log("퀘스트 시작: " + questTitleText);
        UpdateQuestUI();
        if(quesUI != null)
        {
            quesUI.SetActive(true);
        }
    }

    void CheckDeliveryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);

        if(distance <= currentQuest.deliveryRadius)
        {
          if(currentQuest.currentProgresss == 0)
            {
              currentQuest.currentProgresss = 1;
            }
        }
        else
        {
            currentQuest.currentProgresss = 0;
        }
    }

    public void AddCollectProgress(string itemTag)
    {
        if(currentQuest == null || !currentQuest.isActive) return;

        if(currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
           currentQuest.currentProgresss++;
            Debug.Log("아이템 수집 : " + itemTag);
        }
    }

    public void AddInterectProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;
        if (currentQuest.questType == QuestType.Interect && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgresss++;
            Debug.Log("상호 작용 완료 : " + objectTag);
        }
    }

    public void CompleteCurrentQuest()
    {
        if(currentQuest == null || !currentQuest.isActive) return;

        Debug.Log("퀘스트 완료! " + currentQuest.rewardMessage);

        if(completButton != null)
        {
           completButton.gameObject.SetActive(false);
        }

        currentQuestIndex++;
        if(currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            currentQuest = null;
            if (quesUI != null)
            {
                quesUI.gameObject.SetActive(false);
            }
        }
    }

    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgress();
        }

        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
           currentQuest.isCompleted = true;
            if (completButton != null)
            {
                completButton.gameObject.SetActive(true);
            }
        }
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if(completButton != null)
        {
            completButton.onClick.AddListener(CompleteCurrentQuest);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentQuest != null && currentQuest.isActive)
        {
            CheckDeliveryProgress();
            UpdateQuestUI();
        }
    }
}
