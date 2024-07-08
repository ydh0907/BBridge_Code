using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isSimulate = false;
    public BridgeData tempData = new() { info = { stage = -1, name = fileName } };

    public int theme => StageManager.Instance.theme;
    public int stage => StageManager.Instance.stage;
    public const string fileName = "AutoSave";

    public Action<bool, bool, bool> OnGameClear = null;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(transform.root);
    }

    public void LoadStage(int theme, int stage)
    {
        SceneManager.LoadScene(2);
        tempData.info.theme = theme;
        tempData.info.stage = stage;
        StageManager.Instance.LoadStage(theme, stage);
        StageManager.Instance.currentStage.Reset();
    }
    public void UnloadStage()
    {
        SceneManager.LoadScene(1);
        StageManager.Instance.UnloadStage();
        Reset();
    }

    public void RemoveBridge()
    {
        PointManager.Instance.DeleteAllPoint();
        BlockManager.Instance.DeleteAllBlock();
    }

    public void GameStart()
    {
        if (isSimulate) return;
        isSimulate = true;
        tempData = BridgeLoader.Instance.BridgeToData(theme, stage, fileName);
        PointManager.Instance.ActiveAll(true);
        BlockManager.Instance.ActiveAll(true);
        StageManager.Instance.currentStage.SetStart();
    }

    public void GameReset(bool load = true)
    {
        isSimulate = false;
        if (load)
            BridgeLoader.Instance.DataToBridge(tempData);
        StageManager.Instance.currentStage.Reset();
    }

    public void GameEnd()
    {
        isSimulate = false;
        bool isClear = true;
        bool isMoney = !MoneyManager.Instance.isOver;
        bool isBreak = !BlockManager.Instance.isBreak;
        OnGameClear?.Invoke(isClear, isMoney, isBreak);

        (bool, bool, bool, bool) info = ClearInfoManager.ReadClearInfo(theme, stage);
        ClearInfoManager.SaveClearInfo(theme, stage, info.Item2 || isClear, info.Item3 || isMoney, info.Item4 || isBreak);
    }

    private void Reset()
    {
        tempData.info.theme = -1;
        tempData.info.stage = -1;
        isSimulate = false;
    }

    public void AutoSave()
    {
        BridgeLoader.Instance.SaveBridge(tempData);
    }
}
