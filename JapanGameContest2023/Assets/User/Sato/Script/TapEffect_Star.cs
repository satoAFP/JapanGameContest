using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect_Star : MonoBehaviour
{
    [SerializeField, Header("Á‚¦‚é‚Ü‚Å‚ÌƒtƒŒ[ƒ€")] private int killTime;
    [SerializeField, Header("‰Šú‘¬“x")] private int startSpeed;

    [System.NonSerialized] public Vector2 movePower;

    private int frameCount = 0;
    [SerializeField]private Vector2 dicreasePower = new Vector2(0, 0);

    private bool first = true;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (frameCount < killTime)
        {
            if(first)
            {
                movePower.x /= 2000;
                movePower.y /= 2000;

                dicreasePower.x = movePower.x / killTime;
                dicreasePower.y = movePower.y / killTime;

                first = false;

                Debug.Log(movePower.ToString("F10"));
            }

            movePower.x -= dicreasePower.x;
            movePower.y -= dicreasePower.y;

            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();  // rigidbody‚ğæ“¾
            rb.AddForce(new Vector2(movePower.x, movePower.y));  // —Í‚ğ‰Á‚¦‚é


            frameCount++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
