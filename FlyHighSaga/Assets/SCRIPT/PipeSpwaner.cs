using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpwaner : MonoBehaviour
{
    [SerializeField]private GameObject Prefab_Pipe;
    [SerializeField]private float SpwanIntervels = 3f;
    [SerializeField]private float HightRange = 0.45f;

    private float Timer;
    void Start()
    {
        PipeSpwan();
    }

    void Update()
    {
        if (Timer > SpwanIntervels)
        {
            PipeSpwan();
            Timer = 0;
        }
        Timer += Time.deltaTime;
    }
    private void PipeSpwan()
    {
        Vector3 SpwanPos = transform.position + new Vector3(0, Random.Range(-HightRange, HightRange));
        GameObject Pipe = Instantiate(Prefab_Pipe, SpwanPos, Quaternion.identity);
        Destroy(Pipe, 15f);
    }
}
