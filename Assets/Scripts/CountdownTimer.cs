using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    TMP_Text timerText;

    public int startSeconds;

    public UnityEvent onTimeout;

    private void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }

    public void  StartCountdown()
    {
        StartCoroutine(CountdownCR(onTimeout));
    }

    IEnumerator CountdownCR(UnityEvent callback)
    {
        float timer = startSeconds;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "" + Mathf.RoundToInt(timer);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        onTimeout.Invoke();
    }

}
