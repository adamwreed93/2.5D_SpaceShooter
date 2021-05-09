using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [SerializeField] private GameObject _leftTurretPrefab;
    [SerializeField] private GameObject _rightTurretPrefab;
    [SerializeField] private GameObject _shipCorePrefab;
    [SerializeField] private GameObject _bossSuperBeamPrefab;
    [SerializeField] private Animator _bossSuperBeamAnimator;

    public BoxCollider2D bossSuperBeamCollider;

    private bool _isBossSuperBeamActive = false;
    private int _fireBossSuperBeam = 0;

    void Start()
    {
        bossSuperBeamCollider = _bossSuperBeamPrefab.GetComponent<BoxCollider2D>();
        _bossSuperBeamAnimator = _bossSuperBeamPrefab.GetComponent<Animator>();

        StartCoroutine(BossSuperBeamStartRoutine());
    }

    private void Update()
    {
        if (!_isBossSuperBeamActive)
        {

        }
    }



    private IEnumerator BossSuperBeamPowerDownRoutine()
    {
        _bossSuperBeamPrefab.SetActive(true);
        _bossSuperBeamAnimator.SetBool("SuperBeamActive", true);
        _isBossSuperBeamActive = true;
        bossSuperBeamCollider.offset = new Vector3(0, 2.5f, 0);
        yield return new WaitForSeconds(3.0f);
        _bossSuperBeamAnimator.SetBool("SuperBeamActive", false);
        yield return new WaitForSeconds(1.0f);
        bossSuperBeamCollider.offset = new Vector3(0, 0, 0);
        _isBossSuperBeamActive = false;
        _bossSuperBeamPrefab.SetActive(false);
        StartCoroutine(BossSuperBeamStartRoutine());
    }

    private IEnumerator BossSuperBeamStartRoutine()
    {
        while (!_isBossSuperBeamActive)
        {
            yield return new WaitForSeconds(1);

            _fireBossSuperBeam = Random.Range(0, 6);

            if (_fireBossSuperBeam == 1)
            {
                StartCoroutine(BossSuperBeamPowerDownRoutine());
            }

        }
    }
}
