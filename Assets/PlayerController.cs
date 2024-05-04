using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float speedMagnification = 10;
    public Animator playerAnimator;
    public Transform playerCamera;
    public float cameraOffsetY = 10f;
    public float cameraOffsetZ = -10f;

    private Rigidbody rb;
    private Vector3 movingVelocity;
    private Vector3 latestPositon;
    private Vector3 movingDirecion;
    private bool isRunning;

    void Start()
    {
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // キーの入力を受け取る
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        isRunning = x != 0f || z != 0f;
        playerAnimator.SetBool("run", isRunning);

        movingDirecion = new Vector3(x, 0, z);
        movingDirecion.Normalize();
        movingVelocity = movingDirecion * speedMagnification;

        // カメラの位置更新
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraOffsetY, transform.position.z + cameraOffsetZ);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(movingVelocity.x, rb.velocity.y, movingVelocity.z);
        Rotate();
    }

    void Rotate()
    {
        Vector3 differenceDis = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(latestPositon.x, 0, latestPositon.z);
        latestPositon = transform.position;
        if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
        {
            if (movingDirecion == new Vector3(0, 0, 0)) return;
            Quaternion rot = Quaternion.LookRotation(differenceDis);
            rot = Quaternion.Slerp(rb.transform.rotation, rot, 0.2f);
            this.transform.rotation = rot;
        }
    }
}