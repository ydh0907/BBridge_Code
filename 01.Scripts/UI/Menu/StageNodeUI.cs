using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageNodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private Button stageButton;
    [SerializeField] private Image clear;
    [SerializeField] private Image money;
    [SerializeField] private Image isBreak;
    [SerializeField] private Color success = Color.green;
    [SerializeField] private Color fail = Color.red;

    public void SetNode(string stageName, int themeIndex, int stageIndex, bool clear, bool money, bool isBreak)
    {
        Debug.Log($"{themeIndex}-{stageIndex} : {stageName}");
        this.stageName.text = stageName;
        stageButton.onClick.AddListener(() => GameManager.Instance.LoadStage(themeIndex, stageIndex));
        this.clear.color = clear ? success : fail;
        this.money.color = money ? success : fail;
        this.isBreak.color = isBreak ? success : fail;
    }
}
