using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    public static Game instance;
    public  List<Transform> SheepList;

    public Transform[] Spawnpoints;
    public GameObject Wolf;
    int Wave = 1;
    bool running;

   [SerializeField] Text WaveText,wavetext2;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (!running)
        {
            StartCoroutine(WaveLoop());
        }

    }

    public void AddSheepToArray(Transform other)
    {
        SheepList.Add(other);

    }

    public int GetWave()
    {
        return Wave;
    }
    IEnumerator WaveLoop()
    {
        running = true;
        Debug.Log("WaitTime");
        yield return new WaitForSeconds(5.0f);
        Debug.Log("the wave " + Wave + " has started");
        WaveText.text = "Wave " + Wave;
        wavetext2.text = WaveText.text;
        WaveText.GetComponent<Animator>().Play("Pop");
        for (int i = 0; i < Wave; i++)
        {
            Instantiate(Wolf, Spawnpoints[Random.Range(0, Spawnpoints.Length)].position, Quaternion.identity);
        }
        yield return new WaitForSeconds(10.0f + Wave * 2);
        Wave++;
        running = false;
    }
    
}
