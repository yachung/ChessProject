using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(_mainCamera.transform.position);
    }
}