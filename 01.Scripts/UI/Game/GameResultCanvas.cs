using UnityEngine;
using UnityEngine.UI;

public class GameResultCanvas : MonoBehaviour
{
    [SerializeField] private GameObject ResultPanel;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button ExitButton;

    [SerializeField] private Image Money;
    [SerializeField] private Image Break;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failColor = Color.red;

    private void Awake()
    {
        if (!GameManager.Instance)
            return;

        ContinueButton.onClick.AddListener(() =>
        {
            ResultPanel.SetActive(false);
            GameManager.Instance.GameReset();
        });
        ExitButton.onClick.AddListener(() =>
        {
            GameManager.Instance.AutoSave();
            GameManager.Instance.UnloadStage();
        });
        GameManager.Instance.OnGameClear += ActivePanel;
    }

    private void OnDestroy()
    {
        if (!GameManager.Instance)
            return;

        GameManager.Instance.OnGameClear -= ActivePanel;
    }

    public void ActivePanel(bool clear, bool money, bool isBreak)
    {
        ResultPanel.SetActive(true);
        Money.color = money ? successColor : failColor;
        Break.color = isBreak ? successColor : failColor;
    }
}
