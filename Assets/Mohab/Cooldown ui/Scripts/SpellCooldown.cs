using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    [Header("UI items for Spell Cooldown")]
    [Tooltip("Tooltip example")]
    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private TMP_Text textCooldown;
    [SerializeField]
    private Image imageEdge;

    private bool isCoolDown;
    private float cooldownTime;
    private float cooldownTimer;

    private StateManager stateManager;

    void Start()
    {
        GameObject sm = GameObject.Find("StateManager");  
        stateManager = sm.GetComponent<StateManager>();

        textCooldown.gameObject.SetActive(false);
        imageEdge.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    void Update()
    {
        if (stateManager != null)
        {
            return;
        }

        ApplyCooldown();
    }

    void ApplyCooldown()
    {
        if (isCoolDown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0.0f)
            {
                isCoolDown = false;
                textCooldown.gameObject.SetActive(false);
                imageEdge.gameObject.SetActive(false);
                imageCooldown.fillAmount = 0.0f;
            }
            else
            {
                textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
                imageCooldown.fillAmount = cooldownTimer / cooldownTime;
                imageEdge.transform.localEulerAngles = new Vector3(0, 0, 360.0f * (cooldownTimer / cooldownTime));
            }
        }
    }

    public void StartCooldown(float cooldown)
    {
        isCoolDown = true;
        cooldownTimer = cooldown;
        textCooldown.gameObject.SetActive(true);
        imageEdge.gameObject.SetActive(true);
    }
}
