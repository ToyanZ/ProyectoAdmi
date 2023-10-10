using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum State { Enter, Click, Down, BeginDrag, Drag, EndDrag, Up, Exit, None}
    public State state = State.None;
    public RectTransform holder;
    public RectTransform handler;
    public float movementRadius = 100;
    public bool smoothing = false;
    
    
    Vector3 touchPos = Vector3.zero;
    Vector2 initPos = Vector2.zero;

    bool dragging = false;

    private void Start()
    {
        initPos = handler.localPosition;
    }
    private void Update()
    {
        if (dragging)
        {
            Vector2 distance = touchPos - holder.position;

            bool outOfRange = distance.magnitude > movementRadius;
            if (outOfRange) touchPos = ((Vector2)holder.position + (distance.normalized * movementRadius)) * new Vector3(1, 1, 0);
            handler.position = touchPos;
        }
        else
        {
            handler.localPosition = initPos;
        }
    }


    //------------
    public void OnPointerDown(PointerEventData eventData)
    {
        state = State.Down;
        //Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }


    public void ResetPos()
    {
        dragging = false;
        handler.localPosition = initPos;
    }

    //-------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        state = State.Drag;
    }
    public void OnDrag(PointerEventData eventData)
    {
        state = State.Drag;

        switch (GameManager.instance.buildType)
        {
            case GameManager.BuildType.Pc:
                if (Input.GetMouseButton(0))
                {
                    touchPos = Input.mousePosition; //Camera.main.ScreenToViewportPoint();
                    touchPos.z = 0;
                    dragging = true;
                }
                break;
            case GameManager.BuildType.Phone:
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    touchPos = touch.position;
                    touchPos.z = 0;
                    dragging = true;
                    //touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                }
                break;
        }
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        state = State.EndDrag;
        dragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        state = State.Up;
    }




    public Vector2 GetDirectionRaw()
    {
        Vector2 direction = (handler.position - holder.position);
        direction = smoothing ? direction.normalized * (direction.magnitude / movementRadius) : direction.normalized;
        return dragging ? direction : Vector2.zero;
    }
    public Vector2 GetDirection()
    {
        Vector2 direction = (handler.position - holder.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        float y = angle > 45 && angle < 135 ? 1 : angle > 225 && angle < 315 ? -1 : 0;
        float x = angle <= 45 || angle >= 315 ? 1 : angle >= 135 && angle <= 225 ? -1 : 0;

        return dragging ? new Vector2(x, y) : Vector2.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        state = State.Click;
        //Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        state = State.Enter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = State.Exit;
    }

}
