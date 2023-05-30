using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Sheep") && !other.GetComponent<SheepAI>().Fenced)
        {
            other.gameObject.GetComponent<SheepAI>().Fenced = true;
            other.gameObject.GetComponent<SheepAI>().UnRope();
            
            Game.instance.SheepList.RemoveAt(Game.instance.SheepList.IndexOf(other.gameObject.transform) );
            FindObjectOfType<ScoreCount>().AddScore(25);

        }
    }
}
