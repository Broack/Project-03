using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Player_Walk_Speed;
    public float JumpVelocity;
    public float DoubleJump;
    public Rigidbody2D rb;
    public KeyCode JumpKey;
    public LayerMask ground;
    public Collider2D footCollider;

    //Bash
    [Header("Bash")]
    [SerializeField] private float Radius;
    [SerializeField] GameObject _BashableObj;
    private bool NearToBashableObj;
    private bool isChosingDir;
    private bool isBashing;
    [SerializeField] private float BashPower;
    [SerializeField] private float BashTime;
    [SerializeField] private GameObject Arrow;
    Vector3 BashDir;
    private float BashTimeReset;

    private float Dir;
    private float DoubleJumpTimer;
    private bool _isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        BashTimeReset = BashTime;
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        _isGrounded = footCollider.IsTouchingLayers(ground);

        if (isBashing == false)
        {
            rb.velocity = new Vector2(Player_Walk_Speed * direction * Time.fixedDeltaTime, rb.velocity.y);
        }
        

        if (direction != 0f)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);
        }

        if (Input.GetKeyDown(JumpKey))
        {
            if (_isGrounded || DoubleJumpTimer > 0f)
            {
                Jump();
             
            }
        }

        //doubleJumptimer
        if (_isGrounded)
        {
            DoubleJumpTimer = DoubleJump;
        }
        else
        {
            if( DoubleJumpTimer > 0f)
            {
                DoubleJumpTimer -= Time.deltaTime;
            }
        }

        Bash();
    }


    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpVelocity * Time.fixedDeltaTime);
        //Debug.Log("Jump");
    }

    void Bash()
    {
        NearToBashableObj = false;

        RaycastHit2D[] Rays = Physics2D.CircleCastAll(transform.position, Radius, Vector3.forward);
        foreach(RaycastHit2D ray in Rays)
        {
            if (ray.collider.tag == "Bashable")
            {
                NearToBashableObj = true;
                _BashableObj = ray.collider.transform.gameObject;
                break;
            }
        }

        if (NearToBashableObj)
        {
            _BashableObj.GetComponent<SpriteRenderer>().color = Color.red;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Time.timeScale = 0;
                _BashableObj.transform.localScale = new Vector2(1.4f, 1.4f);
                Arrow.SetActive(true);
                Arrow.transform.position = _BashableObj.transform.position;
                isChosingDir = true;
            }
            else if (isChosingDir && Input.GetKeyUp(KeyCode.Mouse1))
            {
                Time.timeScale = 1f;
                _BashableObj.transform.localScale = new Vector2(1, 1);
                isChosingDir = false;
                isBashing = true;
                rb.velocity = Vector2.zero;
                transform.position = _BashableObj.transform.position;
                BashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                BashDir.z = 0;
               if (BashDir.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                BashDir = BashDir.normalized;
                _BashableObj.GetComponent<Rigidbody2D>().AddForce(-BashDir * 50, ForceMode2D.Impulse);
                Arrow.SetActive(false);
            }

        }
        else if (_BashableObj != null)
        {
            _BashableObj.GetComponent<SpriteRenderer>().color = Color.white;
        }

        //Bashing
        if (isBashing)
        {
            if (BashTime > 0)
            {
                BashTime -= Time.deltaTime;
                rb.velocity = BashDir * BashPower * Time.deltaTime;
            }
            else
            {
                isBashing = false;
                BashTime = BashTimeReset;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
