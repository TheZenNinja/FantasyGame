using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;

    public AnimationCurve sizeCurve;
    private static float showTime = 0.5f;
    private float showTimer;

    public static DamagePopup Create(int dmg, Vector3 pos, float offset = 0)
    {
        float rnd = Random.Range(-offset, offset);
        Vector3 offsetPos = Vector3.one * rnd;
        offsetPos.y = rnd / 2;

        DamagePopup dp = Instantiate(Resources.Load<GameObject>("Damage Popup"), pos + offsetPos, Quaternion.identity).GetComponentInChildren<DamagePopup>();
        dp.SetData(dmg);
        return dp;
    }
    public void SetData(int dmg)
    {
        text = GetComponentInChildren<TextMeshPro>();

        text.text = dmg.ToString();
    }

    private void Awake()
    {
        showTimer = showTime;
        transform.LookAt(Camera.main.transform.position);
    }
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);

        if (showTimer > 0)
        {
            transform.localScale = Vector3.one * sizeCurve.Evaluate(1 - (showTimer / showTime));

            showTimer -= Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }
}
