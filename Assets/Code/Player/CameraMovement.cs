using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Variables")]
    [SerializeField][Range(0f,5f)] private float X_AxisLagSpeed = 2f;
    [SerializeField][Range(0f,5f)] private float Y_AxisLagSpeed = 2f;
    [SerializeField] private float CameraDistanceFromPlayer = -5f;
    [SerializeField] private float FieldOfView = 100f;
    [SerializeField] private GameObject CameraAttachedActor;
    [SerializeField] private Camera PossesedCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PossesedCamera.fieldOfView = FieldOfView;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCamera();
    }

    private void OnDrawGizmos()
    {

    }

    private void MoveCamera()
    {

    }
}
