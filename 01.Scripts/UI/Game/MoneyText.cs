using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxText;
    [SerializeField] private TextMeshProUGUI curText;

    private void Start()
    {
        maxText.text = MoneyManager.Instance.maxMoney.ToString();
        curText.text = MoneyManager.Instance.currentMoney.ToString();

        MoneyManager.Instance.OnValueChanged += HandleMoneyChanged;
        MoneyManager.Instance.OnMaxValueSet += HandleMaxValueSet;
    }

    private void HandleMaxValueSet()
    {
        maxText.text = MoneyManager.Instance.maxMoney.ToString();
    }

    private void HandleMoneyChanged()
    {
        curText.text = MoneyManager.Instance.currentMoney.ToString();
    }
}
