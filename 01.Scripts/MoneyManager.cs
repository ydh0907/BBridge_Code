using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [field: SerializeField] public int maxMoney { get; private set; } = 10000;
    [field: SerializeField] public int currentMoney { get; private set; } = 0;
    [field: SerializeField] public bool isOver { get; private set; } = false; // don't touch this

    public Action OnValueChanged = null;
    public Action OnMaxValueSet = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Is Multiple Manager!");
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        if (StageManager.Instance)
            maxMoney = StageManager.Instance.currentStageInfo.targetMoney;
        OnMaxValueSet?.Invoke();
    }

    public void IncMoney(int value)
    {
        currentMoney += value;
        OnValueChanged?.Invoke();
        isOver = currentMoney > maxMoney;
    }

    public void DecMoney(int value)
    {
        currentMoney -= value;
        OnValueChanged?.Invoke();
        isOver = currentMoney > maxMoney;
    }

    public void LoadStageData()
    {
        currentMoney = 0;
        maxMoney = StageManager.Instance.currentStageInfo.targetMoney;
    }
}
