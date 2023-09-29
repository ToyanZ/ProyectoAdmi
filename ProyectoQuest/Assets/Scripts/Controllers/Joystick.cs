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




    //-------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        state = State.Drag;
    }
    public void OnDrag(PointerEventData eventData)
    {
        state = State.Drag;
        
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        //    touchPos.z = 0;
        //    dragging = true;
        //}

        if (Input.GetMouseButton(0))
        {
            touchPos = Input.mousePosition; //Camera.main.ScreenToViewportPoint();
            touchPos.z = 0;
            dragging = true;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        state = State.EndDrag;
        dragging = false;
    }




    //------------
    public void OnPointerUp(PointerEventData eventData)
    {
        state = State.Up;
    }




    public Vector2 GetDirection()
    {
        Vector2 direction = (handler.position - holder.position);
        direction = smoothing ? direction.normalized * (direction.magnitude / movementRadius) : direction.normalized;
        return dragging ? direction : Vector2.zero;
    }

    public Vector2 GetDirectionInt()
    {
        Vector2 direction = (handler.position - holder.position);
        return direction.normalized * (direction.magnitude / movementRadius);
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




    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawLine(touchPos, holder.position);
    //    Gizmos.DrawWireSphere(touchPos, 10);
    //}
}
