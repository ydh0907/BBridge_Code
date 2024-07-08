using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private StageNodeUI stageNode;
    [SerializeField] private ThemeListSO themeList;
    [SerializeField] private RectTransform content;
    [SerializeField] private TextMeshProUGUI themeName;
    [SerializeField] private List<Button> themeButtons;
    [SerializeField] private Color selectColor;
    [SerializeField] private Color defaultColor;

    private List<StageNodeUI> nodes = new();
    private ThemeSO theme;

    private void Awake()
    {
        for (int i = 0; i < themeButtons.Count; i++)
        {
            int temp = i;
            themeButtons[temp].onClick.AddListener(() => SetSelectList(temp));
        }
        SetSelectList(0);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetSelectList(int themeIndex)
    {
        while (nodes.Count > 0)
        {
            StageNodeUI node = nodes[0];
            nodes.RemoveAt(0);
            Destroy(node.gameObject);
        }

        for (int i = 0; i < themeButtons.Count; i++)
        {
            if (i == themeIndex)
                themeButtons[i].image.color = selectColor;
            else
                themeButtons[i].image.color = defaultColor;
        }

        theme = themeList.list[themeIndex];
        themeName.text = theme.themeName;
        content.sizeDelta = new Vector2(0, theme.list.Count * 100 + 20);
        for (int i = 0; i < theme.list.Count; i++)
        {
            StageNodeUI node = Instantiate(stageNode, content);
            StageInfoSO info = theme.list[i];
            (bool, bool, bool, bool) clear = ClearInfoManager.ReadClearInfo(themeIndex, i);

            node.SetNode(info.stageName, themeIndex, i, clear.Item2, clear.Item3, clear.Item4);
            nodes.Add(node);
        }
    }
}
