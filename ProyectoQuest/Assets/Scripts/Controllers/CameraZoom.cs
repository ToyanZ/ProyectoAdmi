using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CameraController camController;

    [Space(20)]
    [SerializeField] private float ortographicSize = 2f;
    [SerializeField] private float defaultZoomIn = 0.5f;
    [SerializeField] private float defaultZoomOut = 0.5f;

    [Space(20)]
    [SerializeField] private float defaultZoomInTime = 0.5f;
    [SerializeField] private float defaultZoomOutTime = 0.5f;
    [SerializeField] private float defaultMiddlePauseTime = 0;
    [SerializeField] private float defaultResetZoomTime = 0.5f;

    private bool zooming = false;
    private bool zoomIn = false;
    private void Start()
    {
        camController.tCamera.orthographicSize = ortographicSize;
    }
    /*
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!zooming)
            {
                if (zoomIn) { StartCoroutine(ZoomBack()); }
                else { ZoomIn(); }

                zoomIn = !zoomIn;
            }
        }
    }*/

    public void ZoomInOut(float zoomIn = 0, float zoomOut = 0, float timeIn = 0, float timeOut = 0, float middlePauseTime = 0) { }
    
    
    public void ZoomIn(float zoomAmount = 0, float timeIn = 0, float timeOut = 0, float pauseTime = 0)
    {
        float finalZoomAmount = zoomAmount == 0 ? defaultZoomIn : zoomAmount;
        float finalTimeIn = timeIn == 0 ? defaultZoomInTime : timeIn;
        float finalTimeOut = timeOut == 0 ? defaultZoomOutTime : timeOut;
        float finalPuaseTime = pauseTime == 0 ? defaultMiddlePauseTime : pauseTime;

        if(!zooming) { StartCoroutine(Zoom(-finalZoomAmount, finalTimeIn, finalTimeOut, finalPuaseTime)); }
    }
    public void ZoomOut(float zoomAmount = 0, float timeIn = 0, float timeOut = 0, float pauseTime = 0)
    {
        float finalZoomAmount = zoomAmount == 0 ? defaultZoomOut : zoomAmount;
        float finalTimeIn = timeIn == 0 ? defaultZoomInTime : timeIn;
        float finalTimeOut = timeOut == 0 ? defaultZoomOutTime : timeOut;
        float finalPuaseTime = pauseTime == 0 ? defaultMiddlePauseTime : pauseTime;

        if (!zooming) { StartCoroutine(Zoom(-finalZoomAmount, finalTimeIn, finalTimeOut, finalPuaseTime)); }
    }
    private IEnumerator Zoom(float zoomAmount, float timeIn, float timeOut, float pauseTime)
    {
        zooming = true;

        float zoomValue = ortographicSize + zoomAmount;
        float time = 0;
        while (time < timeIn)
        {
            camController.tCamera.orthographicSize = Mathf.Lerp(ortographicSize, zoomValue, time / timeIn);
            time += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        /*
        yield return new WaitForSeconds(pauseTime);

        zoomValue = camController.tCamera.orthographicSize;
        time = 0;
        while (time < timeOut)
        {
            camController.tCamera.orthographicSize = Mathf.Lerp(zoomValue, ortographicSize, time / timeOut);
            time += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        */
        zooming = false;
    }

    private IEnumerator ZoomBack()
    {
        zooming = true;
       
        float time = 0;
        float start = camController.tCamera.orthographicSize;
        while (time < defaultResetZoomTime)
        {
            camController.tCamera.orthographicSize = Mathf.Lerp(start, ortographicSize, time / defaultResetZoomTime);
            time += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        zooming = false;
    }

}
