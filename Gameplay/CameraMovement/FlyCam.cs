using UnityEngine;

public class FlyCam : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float pitchMin = -80f;
    public float pitchMax = 80f;

    private float yaw = 0f;
    private float pitch = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            move += transform.forward;
        if (Input.GetKey(KeyCode.S))
            move -= transform.forward;
        if (Input.GetKey(KeyCode.A))
            move -= transform.right;
        if (Input.GetKey(KeyCode.D))
            move += transform.right;

        move = move.normalized * moveSpeed * Time.deltaTime;
        transform.position += move;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
