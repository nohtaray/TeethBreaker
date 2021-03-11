using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooth : MonoBehaviour
{
    public int maxHp = 100;

    private Camera _camera;
    private GameController _gameController;
    private Material _material;
    private int _hp;
    private static readonly int MaterialId = Shader.PropertyToID("Tooth");


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _material = GetComponent<Renderer>().material;
        _hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        _material.color = Color.Lerp(Color.black, Color.white, (float) _hp / maxHp);
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
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

    private void OnCollisionEnter(Collision other)
    {
        _hp--;
    }

    private void OnDestroy()
    {
        // https://light11.hatenadiary.com/entry/2019/11/03/223241
        // TODO: ほかにも消さないといけないやつあるかも
        if (_material != null)
        {
            Destroy(_material);
        }
    }
}