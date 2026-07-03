using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 7f;
    private Rigidbody2D rb;



    void Start()
    {
        

        rb =  GetComponent<Rigidbody2D>();


    }


    void Update()
    {

        float move = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);


        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump);

        }



    }
}
