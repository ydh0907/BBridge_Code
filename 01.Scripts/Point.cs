using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Dictionary<ABlock, HingeJoint> connectedHinge = new();
    [HideInInspector] public int tempID = 0;
    [HideInInspector] public Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnJointBreak(float breakForce)
    {
        connectedHinge.Remove(null);
    }

    public void Connect(ABlock block)
    {
        HingeJoint joint = gameObject.AddComponent<HingeJoint>();

        float leftDis = Vector3.Distance(transform.position, block.left.position);
        float rightDis = Vector3.Distance(transform.position, block.right.position);
        bool left = leftDis < rightDis;

        Transform near = left ? block.left : block.right;
        block.SetPositionAndScale(transform, near);

        joint.connectedBody = left ? block.leftRigid : block.rightRigid;
        joint.axis = new Vector3(0, 0, 1);

        block.connectedPoints.Add(this);
        connectedHinge.Add(block, joint);
    }

    public void Disconnect(ABlock block)
    {
        Destroy(connectedHinge[block]);
        connectedHinge.Remove(block);

        if (connectedHinge.Count == 0)
            PointManager.Instance.DeletePoint(this);
    }

    public void Active(bool active)
    {
        rigid.isKinematic = !active;
    }
}
