using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int Damage = 10;
    public string OwnerTag;
    public bool HasLifeSpan = false;
    public float LifeSpan = 0;
    public Component[] ComponentsToDiable;
    public bool DestroyOnContact = false;
    public GameObject DamageSFX;
    public string[] TargetTags;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasLifeSpan)
        {
            LifeSpan -= Time.deltaTime;
            if (LifeSpan <= 0)
                SelfDestruct(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(OwnerTag))
            return;

        foreach (string tag in TargetTags)
        {
            if (other.CompareTag(tag))
                SelfDestruct(other);
        }
    }

    private void SelfDestruct(Collider other)
    {
        foreach (Component c in ComponentsToDiable)
        {
            Destroy(c);
        }
        if (other)
            other.SendMessage("TakeDamage", Damage, SendMessageOptions.DontRequireReceiver);

        if (DamageSFX)
            Instantiate(DamageSFX, this.transform.position, this.transform.rotation);
        if (DestroyOnContact)
            Destroy(this.gameObject);
    }
}
