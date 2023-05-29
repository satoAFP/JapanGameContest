using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayerController : MonoBehaviour
{
    [SerializeField]private GameObject textMeshObj;

    [SerializeField] private int sortingnum = 0;//ここで描画順を決められる

    // Start is called before the first frame update
    void Start()
    {
        //テキストメッシュの描画順変更
        textMeshObj.gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingnum;
    }

    // Update is called once per frame
    void Update()
    {

    }
}