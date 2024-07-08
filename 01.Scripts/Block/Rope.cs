using System.Collections.Generic;
using UnityEngine;

public class Rope : ABlock
{
    [SerializeField] private float ropeSize = 0.25f;
    [SerializeField] private Transform middleRope;
    [SerializeField] private List<Rigidbody> ropedRigidList; // +1 (right is here)
    [SerializeField] private List<HingeJoint> ropedHingeList;
    [SerializeField] private List<MeshRenderer> forceUI;
    private Color defaultColor;
    public Color breakColor = Color.red;

    public override void Init()
    {
        left = transform.Find("Left");
        right = transform.Find("Right");
        leftRigid = left.GetComponent<Rigidbody>();
        rightRigid = right.GetComponent<Rigidbody>();
    }

    public override void SetPositionAndScale(Transform point, Transform target)
    {
        target.position = point.position;
    }

    public override void Connect()
    {
        Vector2 vec = right.position - left.position;
        Vector2 dir = vec.normalized;
        float dis = vec.magnitude;
        int count = 0;

        for (float i = dis; i > 0; i -= ropeSize)
            count++;

        HingeJoint lastJoint = left.gameObject.AddComponent<HingeJoint>();
        lastJoint.autoConfigureConnectedAnchor = false;
        lastJoint.connectedAnchor = new Vector3(-0.5f, 0, 0);
        lastJoint.axis = Vector3.forward;
        ropedRigidList.Add(leftRigid);
        ropedHingeList.Add(lastJoint);
        for (int i = 1; i <= count; i++)
        {
            float size = dis > ropeSize * i ? ropeSize : dis - ropeSize * (i - 1);
            Vector2 position = (Vector2)lastJoint.transform.position + dir * size;
            if (i == 1)
                position -= dir * size * 0.5f;
            Vector3 scale = new Vector3(size, middleRope.localScale.y, middleRope.localScale.z);
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            Transform mid = Instantiate(middleRope, transform);
            mid.SetPositionAndRotation(position, rotation);
            mid.localScale = scale;

            Rigidbody rb = mid.GetComponent<Rigidbody>();
            lastJoint.connectedBody = rb;

            lastJoint = mid.gameObject.AddComponent<HingeJoint>();
            lastJoint.autoConfigureConnectedAnchor = false;
            lastJoint.connectedAnchor = new Vector3(-1, 0, 0);
            lastJoint.axis = Vector3.forward;
            ropedRigidList.Add(rb);
            ropedHingeList.Add(lastJoint);
            forceUI.Add(mid.Find("Frame_1/ForceUI").GetComponent<MeshRenderer>());
        }
        lastJoint.connectedAnchor = new Vector3(-0.125f, 0, 0);
        lastJoint.connectedBody = rightRigid;
        ropedRigidList.Add(rightRigid);

        defaultColor = forceUI[1].material.color;
    }

    protected override void FixedUpdate()
    {
        if (ropedHingeList.Count > 0)
        {
            float per = ropedHingeList[0].currentForce.sqrMagnitude / (breakForce * breakForce);
            for (int i = 0; i < forceUI.Count; i++)
            {
                forceUI[i].material.color = Color.Lerp(defaultColor, breakColor, per);
            }

            if (
                ropedHingeList[0].currentForce.sqrMagnitude > breakForce * breakForce ||
                ropedHingeList[0].currentTorque.sqrMagnitude > breakTorque * breakForce
                )
            {
                ropedHingeList[0].breakForce = 0;
                ropedHingeList[0].breakTorque = 0;
                foreach (var v in ropedHingeList)
                    Destroy(v);
                ropedHingeList.Clear();
            }
        }
    }

    public override void Active(bool active)
    {
        for (int i = 0; i < ropedRigidList.Count; i++)
        {
            ropedRigidList[i].isKinematic = !active;
        }
    }
}
