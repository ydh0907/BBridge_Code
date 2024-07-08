using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button changeView;
    public CinemachineVirtualCamera cam3D;
    public LayerMask mask3D;
    private bool is2D = true;
    [SerializeField] private Button changeUISee;
    public LayerMask maskForce;
    public LayerMask maskNoForce;
    private bool isForce = true;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.GameStart());
        resetButton.onClick.AddListener(() => GameManager.Instance.GameReset());
        removeButton.onClick.AddListener(() => GameManager.Instance.RemoveBridge());
        quitButton.onClick.AddListener(() => GameManager.Instance.UnloadStage());
        changeView.onClick.AddListener(() => ChangeCamera());
        changeUISee.onClick.AddListener(() => ChangeView());
        ChangeView();
    }

    private void ChangeCamera()
    {
        if (!cam3D)
            cam3D = StageManager.Instance.currentStage.view3D;
        if (is2D)
        {
            cam3D.Priority = 20;
            Camera.main.orthographic = false;
            Camera.main.cullingMask = mask3D;
            is2D = false;
        }
        else
        {
            cam3D.Priority = -10;
            Camera.main.orthographic = true;
            is2D = true;
            if (isForce)
                Camera.main.cullingMask = maskForce;
            else
                Camera.main.cullingMask = maskNoForce;
        }
    }

    private void ChangeView()
    {
        if (!is2D) return;

        if (isForce)
        {
            Camera.main.cullingMask = maskNoForce;
            isForce = false;
        }
        else
        {
            Camera.main.cullingMask = maskForce;
            isForce = true;
        }
    }
}
