using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private int x = 0;
    public int X
    {
        get => x;
        set => x = Mathf.Clamp(value, -1, 1);
    }

    public float speed = 10f;
    private Rigidbody rigid;
    [SerializeField] private WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] private Transform[] visuals = new Transform[4];

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        MeshCollider myCol = transform.Find("body").GetComponent<MeshCollider>();
        for (int i = 0; i < wheels.Length; i++)
        {
            Physics.IgnoreCollision(myCol, wheels[i]);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform wheelTrm = visuals[i];

            wheels[i].GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelTrm.position = position;
            wheelTrm.rotation = rotation;

            wheels[i].rotationSpeed = x * speed;
        }
    }

    public void Reset()
    {
        x = 0;
        foreach (WheelCollider wheel in wheels)
        {
            wheel.rotationSpeed = 0;
            wheel.transform.localPosition = new Vector3(wheel.transform.localPosition.x, 0.3f, wheel.transform.localPosition.z);
        }
    }
}
