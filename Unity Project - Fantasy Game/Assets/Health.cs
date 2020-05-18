using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHp;
    private int maxHp = 10;
    public float hpPercent = 1;

    public bool useHealthbar;
    public RectTransform hpBar, bgBar;
    public float bgPercent = 1;
    public bool bgPaused;
    public bool bgMoving;
    public float bgDelay = 1;
    public float bgSpeed = 2;

    private float barSize;

    void Start()
    {
        currentHp = maxHp;
        barSize = hpBar.sizeDelta.x;
    }

    private void Update()
    {
        if (!bgPaused)
        {
            if (Mathf.Abs(hpPercent - bgPercent) > 0.01f)
            {
                bgMoving = true;

                bgPercent = Mathf.Lerp(bgPercent, hpPercent, Time.deltaTime * bgSpeed);

                bgBar.sizeDelta = new Vector2((bgPercent - 0.01f) * barSize, bgBar.sizeDelta.y);
            }
            else
                bgPercent = hpPercent;
        }
        else
            bgMoving = false;
    }

    public void TakeDamage(float dmg)
    {
        TakeDamage(Mathf.RoundToInt(dmg));
    }

    public void TakeDamage(int dmg = 1)
    {
        currentHp -= 1;

        if (currentHp <= 0)
            currentHp = 0;

        hpPercent = (float)currentHp / maxHp;

        if (useHealthbar)
        {
            hpBar.sizeDelta = new Vector2(hpPercent * barSize, hpBar.sizeDelta.y);

            if (!bgPaused)
            {
                if (!bgMoving)
                StartCoroutine(bgPauseTimer(bgDelay));
                else
                StartCoroutine(bgPauseTimer(0.1f));
            }
        }
    }

    IEnumerator bgPauseTimer(float delay)
    {
        bgPaused = true;
        yield return new WaitForSeconds(delay);
        bgPaused = false;
    }
}
