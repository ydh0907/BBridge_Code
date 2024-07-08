using UnityEngine;

public class DefaultBlock : ABlock
{
    [HideInInspector] public FixedJoint connect;
    public MeshRenderer forceUILeft;
    public MeshRenderer forceUIRight;
    private Color defaultColor;
    public Color breakColor = Color.red;

    public override void Active(bool active)
    {
        leftRigid.isKinematic = !active;
        rightRigid.isKinematic = !active;
    }

    public override void Connect()
    {
        connect = left.gameObject.AddComponent<FixedJoint>();
        connect.connectedBody = rightRigid;
        connect.breakForce = Mathf.Infinity;
        connect.breakTorque = Mathf.Infinity;
        leftRigid.mass = left.localScale.x * mass;
        rightRigid.mass = right.localScale.x * mass;
        defaultColor = forceUILeft.material.color;
    }

    protected override void FixedUpdate()
    {
        if (connect)
        {
            float per = connect.currentForce.sqrMagnitude / (breakForce * breakForce);
            forceUILeft.material.color = Color.Lerp(defaultColor, breakColor, per);
            forceUIRight.material.color = Color.Lerp(defaultColor, breakColor, per);

            if (connect.currentForce.sqrMagnitude > breakForce * breakForce ||
                connect.currentTorque.sqrMagnitude > breakTorque * breakTorque)
            {
                connect.breakForce = 0;
                connect.breakTorque = 0;
            }
        }
    }
}