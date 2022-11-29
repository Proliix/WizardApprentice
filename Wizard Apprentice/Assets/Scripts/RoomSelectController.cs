using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectController : MonoBehaviour
{
    [SerializeField] GameObject roomSelectObject;
    [SerializeField] float scrollSmoothness;
    [SerializeField] float scrollSpeed;
    float builtUpScroll;
    [SerializeField] Vector2 minMaxY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        builtUpScroll += Input.mouseScrollDelta.y * scrollSpeed * -1f;
        float distanceToScroll = (1 - Mathf.Pow(scrollSmoothness,Time.deltaTime))*builtUpScroll;
        builtUpScroll /= (Mathf.Pow(scrollSmoothness, Time.deltaTime));
        roomSelectObject.GetComponent<RectTransform>().localPosition -= new Vector3(0,distanceToScroll,0);
        roomSelectObject.transform.localPosition = new Vector2(0, Mathf.Clamp(roomSelectObject.transform.localPosition.y, minMaxY.x, minMaxY.y));
    }

    public void SetPosition(float newY)
    {
        builtUpScroll = 0;
        roomSelectObject.transform.localPosition = new Vector2(0, Mathf.Clamp(newY *-1f, minMaxY.x, minMaxY.y));

    }
}
