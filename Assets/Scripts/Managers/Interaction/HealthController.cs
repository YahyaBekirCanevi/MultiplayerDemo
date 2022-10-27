using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : NetworkBehaviour
{
    [SyncVar, SerializeField] private float maxHealth = 20;
    [SyncVar, SerializeField] private float health = 20;
    private Slider healthBar;
    private Text healthText;
    [SerializeField] private float timeToRevive;
    [SerializeField, ReadOnly] private float timeRemains = 0;
    public float Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, maxHealth);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner) GetComponent<HealthController>().enabled = false;
    }
    private void Update()
    {
        if (healthBar == null)
        {
            healthBar = GameManagementController.Instance.healthBar;
            healthText = healthBar.transform.GetComponentInChildren<Text>();
            health = maxHealth;
            UpdateHealthBar(health, maxHealth);
        }
        if (!base.IsOwner) return;
    }
    public void UpdateHealth(HealthController script, float amountToChange)
    {
        Health += amountToChange;
        UpdateHealthBar(script.health, script.maxHealth);
        UpdateObject(script);
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBar == null) return;
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