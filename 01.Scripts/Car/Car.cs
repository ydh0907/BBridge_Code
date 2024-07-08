using UnityEngine;

public class Car : MonoBehaviour
{
    public enum CarType
    {
        Default
    }

    private Vector2 startPos;

    public Rigidbody rigidCompo { get; private set; }
    public CarMovement moveCompo { get; private set; }

    public int tempID = -1;
    public CarType type = CarType.Default;

    private void Awake()
    {
        startPos = transform.position;
        rigidCompo = GetComponent<Rigidbody>();
        moveCompo = GetComponent<CarMovement>();
    }

    public void SetMovement(bool front = true)
    {
        rigidCompo.isKinematic = false;
        int x = front ? 1 : -1;
        moveCompo.X = x;
    }

    public void StopMovement()
    {
        moveCompo.X = 0;
    }

    public void Reset()
    {
        rigidCompo.isKinematic = true;
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        moveCompo.Reset();
    }
}
