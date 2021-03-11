using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragArea : MonoBehaviour
{
    private Camera _camera;

    private GameController _gameController;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        LaunchBulletIfNeeded();
    }

    private void OnMouseDrag()
    {
        LaunchBulletIfNeeded();
    }

    private void LaunchBulletIfNeeded()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        foreach (var hit in Physics.RaycastAll(ray))
        {
            if (hit.collider.gameObject.CompareTag("Tooth"))
            {
                _gameController.LaunchBulletIfNeeded(hit.point);
            }
        }
    }
}