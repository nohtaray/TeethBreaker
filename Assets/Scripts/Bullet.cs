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
        // 一度表示されたあと、下に消えたとき消す
        var visible = _renderer.isVisible;
        if (_shown && !visible && transform.position.y < 0)
        {
            Destroy(gameObject);
        }

        _shown |= visible;
    }

    private void OnDestroy()
    {
        // https://light11.hatenadiary.com/entry/2019/11/03/223241
        // TODO: ほかにも消さないといけないやつあるかも
        if (_renderer != null)
        {
            Destroy(_renderer);
        }
    }
}