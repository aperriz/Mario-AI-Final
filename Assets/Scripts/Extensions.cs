using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default", "Ground");

    public static bool Raycast(this Rigidbody2D rb, Vector2 dir)
    {
        if (rb.isKinematic) return false;

        float radius = 0.25f;
        float dist = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(rb.position, radius, dir, dist, layerMask);

        return hit.collider != null && hit.rigidbody != rb;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDir)
    {
        Vector2 dir = other.position - transform.position;
        return Vector2.Dot(dir, testDir) > 0.25f;
    }
}
