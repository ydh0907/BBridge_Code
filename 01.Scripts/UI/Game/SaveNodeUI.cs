using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveNodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveName;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    public void SetNode(StageFileInfo info, SaveLoadCanvas owner)
    {
        saveName.text = info.name;
        loadButton.onClick.AddListener(() =>
        {
            BridgeLoader.Instance.DataToBridge(BridgeLoader.Instance.LoadBridge(info));
            owner.InactiveWindow();
        });
        deleteButton.onClick.AddListener(() =>
        {
            BridgeLoader.Instance.DeleteSavedBridge(info);
            owner.ActiveWindow();
        });
    }
}
