using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectController : MonoBehaviour
{
    [SerializeField] GameObject roomSelectObject;
    [SerializeField] GameObject roomSelectDescription;
    [SerializeField] Vector2 roomSelectDescriptionPosition;
    [SerializeField] float scrollSmoothness;
    [SerializeField] float scrollSpeed;
    float builtUpScroll;
    [SerializeField] Vector2 minMaxY;
    [SerializeField] Vector2 lastMousePos;
    private Vector2 mousePosOnDown;
    private float heightOnMouseDown;
    private float totalMovement;
    private float totalTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        builtUpScroll += Input.mouseScrollDelta.y * scrollSpeed * -1f;
        builtUpScroll += Input.GetAxisRaw("Vertical") * scrollSpeed * -1f * 0.05f;
        if(Input.GetMouseButtonDown(0))
        {
            mousePosOnDown = Input.mousePosition;
            totalMovement = 0;
            totalTime = 0;
            heightOnMouseDown = (transform.position.y) - (minMaxY.y) - Input.mousePosition.y;
            builtUpScroll = 0;
        }
        float distanceToScroll = (1 - Mathf.Pow(scrollSmoothness,Time.unscaledDeltaTime))*builtUpScroll;
        if(Mathf.Abs(builtUpScroll) < 1f)
        {
            builtUpScroll = 0;
        }
        else
        {
            builtUpScroll /= (Mathf.Pow(scrollSmoothness, Time.deltaTime));
        }
        roomSelectObject.GetComponent<RectTransform>().localPosition -= new Vector3(0,distanceToScroll,0);
        if (Input.GetMouseButton(0))
        {
            totalMovement += Input.mousePosition.y - lastMousePos.y;
            totalTime += Time.deltaTime;
            roomSelectObject.transform.localPosition =  new Vector3(0, heightOnMouseDown + (Input.mousePosition.y));
            lastMousePos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            builtUpScroll = (Mathf.Sign(Input.mousePosition.y - mousePosOnDown.y) * Mathf.Pow(Mathf.Abs(Input.mousePosition.y - mousePosOnDown.y),0.5f)) / (Mathf.Max(0.5f,totalTime));
        }
        roomSelectObject.transform.localPosition = new Vector2(0, Mathf.Clamp(roomSelectObject.transform.localPosition.y, minMaxY.x, minMaxY.y));
        roomSelectDescription.transform.localPosition = roomSelectDescriptionPosition - ((Vector2)roomSelectObject.transform.localPosition / roomSelectObject.transform.localScale);
    }

    public void SetPosition(float newY)
    {
        builtUpScroll = 0;
        roomSelectObject.transform.localPosition = new Vector2(0, Mathf.Clamp(2*(newY *-1f), minMaxY.x, minMaxY.y));

    }
}
