using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : NetworkBehaviour
{
    [SyncVar] public float maxHealth = 20;
    [SyncVar, SerializeField] private float health = 10;
    private Slider healthBar;
    private Text healthText;
    [SerializeField] private float timeToRevive;
    [SerializeField] private float timeRemains = 0;
    public float Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, maxHealth);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<HealthController>().enabled = false;
        }
    }
    private void Awake()
    {
        healthBar = GameManagementController.Instance.playerHealthSlider;
        healthText = healthBar.transform.Find("HealthText").GetComponent<Text>();
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar(health, maxHealth);
    }
    private void Update()
    {
        if (!base.IsOwner) return;
        /* if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateHealth(this, -1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateHealth(this, 1);
        } */
    }
    public void UpdateHealth(HealthController script, float amountToChange)
    {
        Health += amountToChange;
        UpdateHealthBar(script.health, script.maxHealth);
        UpdateObject(script);
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.gameObject.SetActive(true);
        healthBar.value = currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
        if (currentHealth == 0)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    [ServerRpc]
    public void UpdateObject(HealthController script)
    {
        if (script.Health == 0 && timeRemains == 0)
        {
            StartCoroutine(WaitToRevive());
        }
    }
    private IEnumerator WaitToRevive()
    {

        while (timeRemains < timeToRevive)
        {
            timeRemains += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

}