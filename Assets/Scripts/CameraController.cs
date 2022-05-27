using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach script to camera and camera will follow the target

public class CameraController : MonoBehaviour
{
    //[SerializeField]
    //private Transform target;
    [SerializeField]
    private Vector3 targetOffset;
    [SerializeField]
    private float speed;

    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.Instance.playerChar.transform.position + targetOffset, speed * Time.deltaTime);
    }
}
