using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    private void OnEnable()
    {
        StartCoroutine(showDelay());
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Max(pos.x, player.position.x);
        transform.position = pos;
    }

    private IEnumerator showDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Camera>().enabled = true;
    }
}
