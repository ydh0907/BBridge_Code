using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public Stage currentStage = null;
    public StageInfoSO currentStageInfo;
    public ThemeListSO themeList;

    public int theme { get; private set; } = -1;
    public int stage { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    public void LoadStage(int theme, int stage)
    {
        if (currentStage)
            Destroy(currentStage);

        currentStageInfo = themeList.list[theme].list[stage];
        currentStage = Instantiate(currentStageInfo.map, transform);

        this.theme = theme;
        this.stage = stage;
    }

    public void UnloadStage()
    {
        if (currentStage)
            Destroy(currentStage.gameObject);

        currentStageInfo = null;
        currentStage = null;

        theme = -1;
        stage = -1;
    }
}
