using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    Vector2 dodgeVec;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    public float speed = 10;

    bool spaceDown;
    bool eDown;

    bool isDodge = false;
    public bool islift = false;

    public GameObject nearObject;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Dodge();
        Interact();
    }

    void GetInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        spaceDown = Input.GetButtonDown("Dodge");
        eDown = Input.GetButtonDown("Interact");
    }

    void Move()
    {
        if (isDodge)
        {
            inputVec = dodgeVec;
        }
        Vector2 nextVec = inputVec.normalized * speed * Time.deltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    void Dodge() // 회피
    {
        if (spaceDown)
        {
            isDodge = true;
            dodgeVec = inputVec;
            speed *= 2;
            Invoke("DodgeOut", 0.5f);
        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void Interact() // 주변 물건 들기, 내려놓기
    {
        if (!islift && eDown && nearObject != null)
        {
            if (nearObject.tag == "InteractableObject")
            {
                InteractableObject obj = nearObject.GetComponent<InteractableObject>();
                nearObject.transform.SetParent(transform);
                nearObject.transform.localPosition = new Vector2(0, 1.5f);
                islift = true;
            }
        }
        else if (islift && eDown)
        {
            {
                nearObject.transform.localPosition = new Vector2(0, 0);
                nearObject.transform.SetParent(null);
                islift = false;
            }
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (!islift && other.tag == "InteractableObject")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!islift && other.tag == "InteractableObject")
        {
            nearObject = null;
        }
    }

}
