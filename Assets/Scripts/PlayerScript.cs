using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SummonAbility
{
    public Slider CooldownSlider;
    public RawImage CooldownImage;

    public GameObject Summon;

    public float SummonDelay = 3f;

    public float CurrentSummonDelay = 0;
    public int SoulCost = 1;
    private int NumSouls = 0;

    public void Setup()
    {
        CooldownSlider.maxValue = SummonDelay * 100;
        UpdateCooldownSlider();
    }

    private void UpdateCooldownSlider()
    {
        CooldownSlider.value = CurrentSummonDelay * 100;
        CooldownImage.color = (CurrentSummonDelay <= 0 && NumSouls >= SoulCost) ? Color.white : Color.red;
    }

    public void UpdateSouls(int souls)
    {
        NumSouls = souls;
        UpdateCooldownSlider();
    }

    public void UpdateTimer()
    {
        if (CurrentSummonDelay > 0)
        {
            CurrentSummonDelay -= Time.deltaTime;
            UpdateCooldownSlider();
        }
    }
}

public class PlayerScript : MonoBehaviour
{
    public GameObject Body;
    public GameObject Fireball;

    public SummonAbility[] SummonSpells;

    public GameObject DeathImage;
    public TMPro.TMP_Text SoulCounter;
    public TMPro.TMP_Text ScoreCounter;

    private Camera Cam;
    private int MoveSpeed = 8;

    public float AttackDelay = 0.5f;
    private float CurrentAttackDelay = 0;
    private int NumSouls = 0;
    private int Score = 0;
    private bool Dead = false;
    private bool InStartState = true;

    void Start()
    {
        Cam = Camera.main;
        foreach(SummonAbility s in SummonSpells)
            s.Setup();
    }

    void Update()
    {
        if(Dead)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }
        if (!InStartState)
            PlayerMovement();
        PlayerActions();
    }

    void OnGUI()
    {
        if (Dead || InStartState)
            return;

        LookAtMouse();
    }

    void PlayerMovement()
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W))
            y = 1;
        else if (Input.GetKey(KeyCode.S))
            y = -1;
        if (Input.GetKey(KeyCode.D))
            x = 1;
        else if (Input.GetKey(KeyCode.A))
            x = -1;

        this.GetComponent<Rigidbody>().velocity = new Vector3(x * MoveSpeed, 0, y * MoveSpeed);
    }

    void PlayerActions()
    {
        if (CurrentAttackDelay > 0)
            CurrentAttackDelay -= Time.deltaTime;

        foreach (SummonAbility s in SummonSpells)
            s.UpdateTimer();

        if (CurrentAttackDelay <= 0 && Input.GetMouseButtonDown(0))
        {
            Vector3 pos = this.transform.position;
            pos.y = 1;
            GameObject fireball = Instantiate(Fireball, pos, Body.transform.rotation);
            fireball.GetComponent<DamageOnContact>().OwnerTag = this.tag;
            CurrentAttackDelay = AttackDelay;
        }

        if (InStartState)
            return;

        for (int i = 0; i < SummonSpells.Length; i++)
        {
            if (SummonSpells[i].CurrentSummonDelay <= 0 && NumSouls >= SummonSpells[i].SoulCost && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Vector3 spawnPosition = this.transform.position;
                if (Random.Range(0, 2) == 0)
                {
                    spawnPosition.x += Random.Range(0, 2) == 0 ? 5 : -5;
                    spawnPosition.z += Random.Range(-5, 5);
                }
                else
                {
                    spawnPosition.z += Random.Range(0, 2) == 0 ? 5 : -5;
                    spawnPosition.x += Random.Range(-5, 5);
                }
                Instantiate(SummonSpells[i].Summon, spawnPosition, Quaternion.identity);
                AddSoul(-SummonSpells[i].SoulCost);
                SummonSpells[i].CurrentSummonDelay = SummonSpells[i].SummonDelay;
                break;
            }
        }
    }

    void LookAtMouse()
    {
        Vector3 point = new();
        Event currentEvent = Event.current;
        Vector2 mousePos = new();

        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = Cam.pixelHeight - currentEvent.mousePosition.y;

        point = Cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Cam.nearClipPlane));
        point.y = 1;
        Body.transform.LookAt(point);
    }

    public void AddSoul(int num)
    {
        NumSouls += num;
        Score += num;
        foreach (SummonAbility s in SummonSpells)
            s.UpdateSouls(NumSouls);
        SoulCounter.text = "Souls: " + NumSouls;
        ScoreCounter.text = "Score: " + Score;
    }

    public void SelfDestruct()
    {
        Time.timeScale = 0;
        DeathImage.SetActive(true);
        Dead = true;
    }

    public void EndStartState()
    {
        InStartState = false;
    }
}
