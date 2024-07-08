using UnityEngine;

public class DetectingBreak : MonoBehaviour
{
    private void OnJointBreak(float breakForce)
    {
        BlockManager.Instance.isBreak = true;
    }
}
