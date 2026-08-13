using UnityEngine;

public class Player : MonoBehaviour
{
    float moveX;
    float moveZ;

    bool jump;
    bool isJump;

    public float Speed;
    public float JumpPower;

    Vector3 moveVec;

    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Move();
        GetInput();
        Jump();
    }

    void GetInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(moveX, 0, moveZ).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;
    }

    void Jump()
    {
        if (jump && !isJump)
        {
            rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }
}