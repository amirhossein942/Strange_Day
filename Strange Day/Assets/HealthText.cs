using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 0.5f;
    public TextMeshProUGUI TextMesh;

    public float speed = 500f;
    public float timeElapsed = 0.0f;
    public Vector3 direction = new Vector3(0, 1, 0);
    public Color32 StartColor;
    RectTransform rTrans;


    // Start is called before the first frame update
    void Start()
    {
        rTrans = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        rTrans.position += speed * Time.deltaTime * direction;

        

        Color color = new Color(StartColor.r, StartColor.g, StartColor.b, 1 - (timeElapsed / timeToLive));

        Color32 color32 = color;

        TextMesh.faceColor = color32;

        if (timeElapsed > timeToLive)
        { 
            Destroy(gameObject);
        }   
    }
}
