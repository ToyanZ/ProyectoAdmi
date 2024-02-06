using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraTracking : MonoBehaviour
{
    public CameraController camController;
    [Space(20)]
    [Range(0f, 10f)][SerializeField] private float deadZoneX = 5;
    [Range(0f, 10f)][SerializeField] private float deadZoneY = 7;

    [Space(5)]
    [Range(0f, 2f)][SerializeField] private float offsetX = 0;
    [Range(0f, 2f)][SerializeField] private float offsetY = 0;

    [Space(10)]
    [Range(0f, 1f)][SerializeField] private float smoothingX = 0.3f;
    [Range(0f, 1f)][SerializeField] private float smoothingY = 0.3f;
    [Range(0f, 1f)][SerializeField] private float smoothingGoTo = 0.3f;

    [Space(5)]
    [Range(0f, 1f)][SerializeField] private float relativeSmoothing = 0.3f;

    [Space(5)]
    public AnimationCurve animationCurve;

    [Space(20)]
    [SerializeField] private bool trackingEnabled = true;

    private Bounds bounds;
    private Vector3 offset;
    private Vector3 absVelocityX = Vector3.zero;
    private Vector3 absVelocityY = Vector3.zero;
    private Vector3 relVelocityX = Vector3.zero;
    private Vector3 relVelocityY = Vector3.zero;
    private float z;
    
    private Vector2 refVelocity = Vector2.zero;
    private bool goingTo = false;

    private void Start()
    {
        camController.absolutePosition = Vector3.zero;
        camController.relativePosition = Vector3.zero;
        z = camController.tCamera.transform.position.z;
    }
    private void FixedUpdate()
    {
        if (!HasTarget()) return;

        if (trackingEnabled) Track();
    }

    private bool HasTarget()
    {
        if (camController.target != null) return true;

        camController.target = FindObjectOfType<Target>().transform;
        return false;
    }
    private void Track()
    {
        bounds = new Bounds(camController.target.position, new Vector3(0.1f, 0.1f, 0.1f));
        bounds.Encapsulate(camController.target.position);
        //Add other targets if multiplayer


        float x = 0;
        float y = 0;
        if (OutOfBounds(bounds.center.x, camController.absolutePosition.x, deadZoneX))
        {
            x = Vector3.SmoothDamp(camController.absolutePosition, bounds.center, ref absVelocityX, relativeSmoothing).x;
            camController.absolutePosition = new Vector3(x, camController.absolutePosition.y, z);
        }
        if (OutOfBounds(bounds.center.y, camController.absolutePosition.y, deadZoneY))
        {
            y = Vector3.SmoothDamp(camController.absolutePosition, bounds.center, ref absVelocityY, relativeSmoothing).y;
            camController.absolutePosition = new Vector3(camController.absolutePosition.x, y, z);
        }


        offset = new Vector3(offsetX, offsetY, 0);
        camController.relativePosition = camController.absolutePosition + offset;
        x = Vector3.SmoothDamp(camController.tCamera.transform.position, camController.relativePosition, ref relVelocityX, smoothingX).x;
        y = Vector3.SmoothDamp(camController.tCamera.transform.position, camController.relativePosition, ref relVelocityY, smoothingY).y;

        camController.tCamera.transform.position = new Vector3(x, y, z);
    }
    private bool OutOfBounds(float targetPos, float relativePos, float deadZone)
    {
        bool less = targetPos < relativePos - (deadZone / 2);
        bool more = targetPos > relativePos + (deadZone / 2);
        return less || more;
    }


    public void StopTracking()
    {
        trackingEnabled = false;
    }
    public void StartTracking()
    {
        trackingEnabled = true;
    }

    public void GoTo(GameObject go)
    {
        if (!goingTo) StartCoroutine(GoToIE(go));
    }
    IEnumerator GoToIE(GameObject go)
    {
        goingTo = true;
        Vector3 start = camController.tCamera.transform.position;
        Vector3 end = go.transform.position;
        float time = 0;
        while(time < 0.5f)
        {
            time += Time.deltaTime;
            Vector2 dir = Vector2.Lerp(start, end, animationCurve.Evaluate(time / 0.5f));
            camController.tCamera.transform.position = new Vector3(dir.x, dir.y, start.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        goingTo = false;
    }

    public void GoTo(List<GameObject> go)
    {
        if (!goingTo) StartCoroutine(GoToIE(go));
    }
    IEnumerator GoToIE(List<GameObject> go)
    {
        goingTo = true;

        List<GameObject> list = new List<GameObject>(go);

        foreach (GameObject go2 in list)
        {
            Vector3 start = camController.tCamera.transform.position;
            Vector3 end = go2.transform.position;
            float time = 0;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                Vector2 dir = Vector2.Lerp(start, end, animationCurve.Evaluate(time / 0.5f));
                camController.tCamera.transform.position = new Vector3(dir.x, dir.y, start.z);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            yield return null;
        }

        
        goingTo = false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (camController.target == null)
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(deadZoneX, deadZoneY));
        }
        else
        {
            //Vector2 offset = new Vector2(offsetX, offsetY);
            Gizmos.DrawWireCube(camController.absolutePosition, new Vector2(deadZoneX, deadZoneY));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(camController.relativePosition, new Vector2(deadZoneX, deadZoneY));

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

        }
    }
}
