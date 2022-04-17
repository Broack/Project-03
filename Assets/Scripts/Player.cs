using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movement_Speed;
    [SerializeField] private Rigidbody2D _PlayerOri;

    private float _Movement;

    void Start()
    {
        _PlayerOri = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _Movement = Input.GetAxis("Horizontal") * _movement_Speed;
    }

    void FixedUpdate()
    {
        _PlayerOri.velocity = new Vector2(_Movement * Time.fixedDeltaTime, _PlayerOri.velocity.y);
    }
}
