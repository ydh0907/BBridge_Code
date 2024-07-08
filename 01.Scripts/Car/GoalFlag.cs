using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    public Car target;
    [HideInInspector] public Stage stage;
    [HideInInspector] public bool goal = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent(out Car car))
        {
            if (car == target)
            {
                goal = true;
                car.StopMovement();
                stage.Goal();
            }
        }
    }
}
