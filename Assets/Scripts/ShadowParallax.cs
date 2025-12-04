using UnityEngine;
using UnityEngine.UIElements;

public class ShadowParallax : MonoBehaviour
{
    private float length;
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<Image>().worldBound.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
