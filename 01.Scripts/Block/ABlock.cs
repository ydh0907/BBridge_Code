using System.Collections.Generic;
using UnityEngine;

public abstract class ABlock : MonoBehaviour
{
    public BlockType type;
    public float breakForce = 100;
    public float breakTorque = 100;
    public float mass = 1;
    [field: SerializeField] public int costPU { get; protected set; } = 100;
    public int cost => (int)(left.localScale.x * costPU);

    [HideInInspector] public Rigidbody leftRigid;
    [HideInInspector] public Rigidbody rightRigid;
    [HideInInspector] public Transform left;
    [HideInInspector] public Transform right;
    [HideInInspector] public List<Point> connectedPoints = new();
    private Color originColor;

    public virtual void Init()
    {
        left = transform.Find("Left");
        right = transform.Find("Right");
        leftRigid = left.GetComponent<Rigidbody>();
        rightRigid = right.GetComponent<Rigidbody>();
    }

    public virtual void SetPositionAndScale(Transform point, Transform target)
    {
        Vector3 position = (point.position + transform.position) * 0.5f;
        Vector3 scale = new Vector3
            (Vector3.Distance(point.position, transform.position), target.localScale.y, target.localScale.z);
        target.position = position;
        target.localScale = scale;
    }

    public abstract void Connect();

    protected abstract void FixedUpdate();

    public abstract void Active(bool active);
}
