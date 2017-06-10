using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {
    private Vector2 offset = Vector2.zero;
    private float speed = 0.5f;
    new private Renderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset.y = (offset.y + speed * Time.deltaTime) % 1;
        renderer.material.mainTextureOffset = offset;
    }
}
