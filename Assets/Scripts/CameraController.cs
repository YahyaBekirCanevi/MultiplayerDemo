using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float xClamp = 45f;
    [SerializeField] private Vector3 offset = Vector3.up;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -6);
    private Camera cam;
    private float MouseX { get; set; }
    private float MouseY { get; set; }
    Vector3 velocity = Vector3.zero;
    public Transform Player { get; set; }
    private void Awake()
    {
        cam = transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update()
    {
        if (!GameManagementController.Instance.isMenuOpen)
        {
            GetInputs();
            Rotate();
        }
    }
    private void LateUpdate()
    {
        if (Player) FollowPlayer();
    }
    private void GetInputs()
    {
        MouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        MouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        MouseY = Mathf.Clamp(MouseY, -xClamp, xClamp);
    }
    private void FollowPlayer()
    {
        Vector3 desiredPosition = Player.position + offset;
        transform.position = desiredPosition;

        cam.transform.localPosition = cameraOffset;
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(MouseY, MouseX, 0);
    }
}
