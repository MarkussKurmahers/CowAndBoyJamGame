using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreCount : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject ScoreSound;
   [SerializeField] Text ScoreText;
    int Score=0;
   public void AddScore(int amount)
    {
        Score += amount;
        ScoreText.text = Score.ToString();
        animator.Play("Pop");
        Instantiate(ScoreSound, transform.position, Quaternion.identity);
    }
}
