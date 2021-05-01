using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRotator : MonoBehaviour
{
    [SerializeField] private Transform _heatSeakingMissile;


    void Start()
    {
        _heatSeakingMissile = GameObject.FindGameObjectWithTag("HeatSeeker").GetComponent<Transform>();
    }

   
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _heatSeakingMissile.transform.rotation.z);
    }
}
