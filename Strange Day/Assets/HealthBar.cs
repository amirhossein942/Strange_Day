using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public const float MaxHealth = 9f;
    public Image healthBar;
    public PlayerController playerController;
    RectTransform canvas; 
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = playerController.Health / MaxHealth;

    }

    void LateUpdate()
    {


    }
}
