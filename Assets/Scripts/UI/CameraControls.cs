using Unity.Cinemachine;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Panning")]
    public float _panMultiplier;
    public Vector3 _previousPos;
    public Vector3 _newMovement;
    
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
        //This panning business is freaky. Revisit at a later point.

        //if (Input.GetMouseButton(0))
        //{
        //    Touch touch0 = Input.GetTouch(0);
        //    Vector3 currentPos = touch0.position - touch0.deltaPosition;

        //    if (_previousPos != currentPos)
        //    {
        //        _newMovement = transform.forward * (_previousPos.y - currentPos.y) + transform.right * (_previousPos.x - currentPos.x);
        //        //_newMovement.z = _newMovement.x;
        //        //_newMovement.x = transform.position.x;
        //        transform.position += _newMovement * _panMultiplier * Time.unscaledDeltaTime;
        //    }

        //    _previousPos = currentPos;
        //}
        //else _previousPos = Vector3.zero;

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
