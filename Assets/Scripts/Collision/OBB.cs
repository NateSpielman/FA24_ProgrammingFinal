using UnityEngine;
using System.Collections;

public class OBB : PhysicsCollider
{
    public float[] halfwidths => new float[] { transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2 };
    public override Shape shape => Shape.OBB;
}
