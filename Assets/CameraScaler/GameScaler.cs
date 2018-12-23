using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class GameScaler : MonoBehaviour
{
    private Camera m_Camera;
    public Camera Camera { get { return m_Camera != null ? m_Camera : m_Camera = GetComponent<Camera>(); } }

    public float CameraHeigth { get { return Camera.orthographicSize * 2; } }
    public float CameraWidth { get { return CameraHeigth * Camera.aspect; } }

    public float CameraTargetWidth = 5;
    public float MaxCameraAspect = 1;
    public float MinCameraAspect = 0;

    private int m_CurrentScreenWidth = -1;
    private int m_CurrentScreenHeight = -1;

    public bool IsCameraChanged
    {
        get
        {
            var value = Screen.width != m_CurrentScreenWidth || Screen.height != m_CurrentScreenHeight;
            if (value)
            {
                m_CurrentScreenWidth = Screen.width;
                m_CurrentScreenHeight = Screen.height;
            }
            return value;
        }
    }

    void Update() { if (IsCameraChanged) FitCamera(); }
    void OnValidate() { FitCamera(); }

    void OnDrawGizmosSelected()
    {
        // Draw max height
        var posX = transform.position.x + CameraTargetWidth / 2;
        var posY = transform.position.x + (CameraTargetWidth / MaxCameraAspect / 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(-posX, posY), new Vector2(posX, posY));
        Gizmos.DrawLine(new Vector2(-posX, -posY), new Vector2(posX, -posY));
    }

    private void FitCamera()
    {
        // Calculate new orthoSize
        var aspect = Mathf.Clamp(Camera.aspect, MinCameraAspect, MaxCameraAspect);
        var newOrtoSize = CameraTargetWidth / aspect / 2;
        Camera.orthographicSize = newOrtoSize;
    }
}