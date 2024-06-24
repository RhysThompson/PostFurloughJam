using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public GameObject Body;
    public GameObject Projectile;
    private GameObject Target;
    private GameObject Player;
    private Animator Anim;
    private HealthManager HM;

    public float MoveSpeed = 3;
    public int AttackDamage = 10;
    public float AttackDelay = 2.0f;
    public float ProjectileDelay = 4.0f;
    public int SoulValue = 1;
    public string EnemyTag = "Enemy";

    public bool IsHostileToPlayer = true;
    public bool CanMelee = true;
    public int MeleeRange = 3;
    public int ProjectileRange = 12;
    public bool IsAOE = false;
    private float CurrentAttackDelay = 0;
    private bool IsAttacking;

    private float TargetDistance = 0;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PickTarget();

        Anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PickTarget();

        if (!IsAttacking && Target)
        {
            Vector3 targetPos = Target.transform.position;
            Body.transform.LookAt(Target.transform.position);
            Quaternion rot = Body.transform.rotation;
            rot.x = 0;
            rot.z = 0;
            Body.transform.rotation = rot;

            float dist = Vector3.Distance(this.transform.position, Target.transform.position);
            if (dist > MeleeRange)
            {
                if (Projectile && dist < ProjectileRange)
                {
                    GameObject projectile = Instantiate(Projectile, this.transform.position, Body.transform.rotation);
                    projectile.GetComponent<DamageOnContact>().OwnerTag = this.tag;
                    IsAttacking = true;
                    CurrentAttackDelay = ProjectileDelay;
                    Anim.SetTrigger("Attack");
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                    this.GetComponent<Rigidbody>().velocity = Body.transform.TransformDirection(new Vector3(0, 0, MoveSpeed));
            }
            else
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Anim.SetTrigger("Attack");
                IsAttacking = true;
                CurrentAttackDelay = AttackDelay;

                if (CanMelee)
                {
                    if (IsAOE)
                    {
                        GameObject[] targets = GameObject.FindGameObjectsWithTag(EnemyTag);

                        foreach (GameObject t in targets)
                        {
                            if (t == this.gameObject)
                                continue;

                            dist = Vector3.Distance(this.transform.position, t.transform.position);
                            if (dist < MeleeRange)
                            {
                                t.GetComponent<HealthManager>().TakeDamage(AttackDamage);
                            }
                        }
                    }
                    else
                        Target.GetComponent<HealthManager>().TakeDamage(AttackDamage);
                }
            }
        }
        if (IsAttacking && CurrentAttackDelay <= 0)
            IsAttacking = false;

        CurrentAttackDelay -= Time.deltaTime;
    }
    public void SelfDestruct()
    {
        Player.GetComponent<PlayerScript>().AddSoul(SoulValue);
        Destroy(this.gameObject);
    }

    private void PickTarget()
    {
        TargetDistance = 999999;

        if (IsHostileToPlayer)
        {
            Target = Player;
            TargetDistance = Vector3.Distance(this.transform.position, Player.transform.position);
        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag(EnemyTag);

        float dist = 0;
        foreach (GameObject t in targets)
        {
            if (t == this.gameObject)
                continue;

            dist = Vector3.Distance(this.transform.position, t.transform.position);
            if(dist < TargetDistance)
            {
                TargetDistance = dist;
                Target = t;
            }
        }
    }
}
