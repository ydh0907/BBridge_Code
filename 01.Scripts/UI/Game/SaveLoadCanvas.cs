using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveLoadCanvas : MonoBehaviour
{
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private RectTransform content;
    [SerializeField] private SaveNodeUI nodePrefab;
    [SerializeField] private TMP_InputField input;
    private List<SaveNodeUI> items = new();

    public void ActiveWindow()
    {
        windowPanel.SetActive(true);

        SavedStageInfo.ReadAllStageInfo();
        List<StageFileInfo> allFile = SavedStageInfo.savedStages;

        int theme = GameManager.Instance.theme;
        int stage = GameManager.Instance.stage;
        while (items.Count > 0)
        {
            GameObject item = items[0].gameObject;
            items.RemoveAt(0);
            Destroy(item);
        }
        items.Clear();

        for (int i = 0; i < allFile.Count; i++)
        {
            StageFileInfo info = allFile[i];
            if (info.theme == theme && info.stage == stage)
            {
                SaveNodeUI node = Instantiate(nodePrefab, content);
                node.SetNode(info, this);
                items.Add(node);
            }
        }

        content.sizeDelta = new Vector2(0, items.Count * 100 + 20);
    }

    public void InactiveWindow()
    {
        while (items.Count > 0)
        {
            GameObject item = items[0].gameObject;
            items.RemoveAt(0);
            Destroy(item);
        }
        items.Clear();
        windowPanel.SetActive(false);
    }

    public void SaveCurrentBridge()
    {
        string name = input.text;
        if (string.IsNullOrEmpty(name))
            name = "Unknown";
        int theme = GameManager.Instance.theme;
        int stage = GameManager.Instance.stage;

        BridgeLoader.Instance.SaveBridge(BridgeLoader.Instance.BridgeToData(theme, stage, name));
        ActiveWindow();
    }
}
