using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotLine : MonoBehaviour
{
    [SerializeField] private GameObject dotObj;

    [SerializeField] private Vector2 startPos;

    [SerializeField] private Vector2 square;

    [SerializeField] private int dotNum;

    [SerializeField] private float wide;

    private bool first = true;

    private List<GameObject> clone = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
            if (first)
            {
                startPos = managerAccessor.Instance.dataMagager.MouseWorldChange();
                first = false;
            }    
        }
        else
            first = true;

        if (Input.GetMouseButton(0))
        {
            Vector2 usePos = startPos;

            square.x = startPos.x - Input.mousePosition.x;
            square.y = startPos.y - Input.mousePosition.y;

            wide = (square.x * 2 + square.y * 2) / dotNum;
            square += startPos;

            int a = 0;
            clone.Clear();

            for (int i = 0; i < dotNum; i++)
            {
                if (usePos.x < square.x && usePos.y == startPos.y)
                {
                    usePos.x += wide;

                    if (usePos.x > square.x)
                    {
                        usePos.y += usePos.x - square.x;
                        usePos.x = square.x;
                    }
                }
                else if (usePos.y < square.y && usePos.x == square.x)
                {
                    usePos.y += wide;

                    if (usePos.y > square.y)
                    {
                        usePos.x -= usePos.y - square.y;
                        usePos.y = square.y;
                    }
                }
                else if (usePos.x > startPos.x && usePos.y == square.y)
                {
                    usePos.x -= wide;

                    if (usePos.x < startPos.x)
                    {
                        usePos.y -= startPos.x - usePos.x;
                        usePos.x = startPos.x;
                    }
                }
                else if (usePos.y > startPos.y && usePos.x == startPos.x)
                {
                    usePos.y -= wide;

                    if (usePos.y < startPos.y)
                    {
                        usePos.x -= startPos.y - usePos.y;
                        usePos.y = startPos.y;
                    }
                }

                
                clone.Add(Instantiate(dotObj));
                clone[a].transform.position = usePos;
                a++;
            }

        }
    }
}
