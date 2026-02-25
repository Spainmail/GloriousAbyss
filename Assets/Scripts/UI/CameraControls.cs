using Unity.Cinemachine;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Panning")]
    public float _panMultiplier;
    
    [Header("Zooming")]
    public CinemachineCamera _camera;
    public float _zoomMin;
    public float _zoomMax;
    public float _zoomSpeed;

    private void Start()
    {
        _camera = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector3 touch0PrevPosition = touch0.position - touch0.deltaPosition;
            Vector3 touch1PrevPosition = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0PrevPosition - touch1PrevPosition).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * _zoomSpeed);
        }
    }

    public void Zoom(float newZoomValue)
    {
        _camera.Lens.FieldOfView = Mathf.Clamp(_camera.Lens.FieldOfView - newZoomValue, _zoomMin, _zoomMax);
    }
}
