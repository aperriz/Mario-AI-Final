using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SideCamera : MonoBehaviour
{
    public static Transform player;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Max(pos.x, player.position.x);
        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, transform.position.y, transform.position.z), GameManager.lerpTime);
    }
}
