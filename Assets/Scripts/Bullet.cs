using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _shown = false;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _shown |= _renderer.isVisible;
        if (_shown && !_renderer.isVisible)
        {
            Destroy(this.gameObject);
        }
    }
}