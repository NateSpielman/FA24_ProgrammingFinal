using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABB : PhysicsCollider
{
    // TODO: YOUR CODE HERE
    public float[] halfwidths => new float[] {transform.localScale.x/2, transform.localScale.y/2, transform.localScale.z/2};
    
    public override Shape shape => Shape.AABB;
}
