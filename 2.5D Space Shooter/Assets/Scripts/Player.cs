using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private int _life = 3;
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _fireRate = 0;
    private float _canFire = -1f; //The -1 starts _canFire below 0. So long as Time.time is higher than _canFire, you can shoot!  

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

   
    void Update()
    {
        Movement();
        Boundries();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0); //Creates the Vector3 "direction" for use within this method.

        transform.Translate(direction * _speed * Time.deltaTime);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
    }

    void Boundries()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp (transform.position.y, -3.8f, 0), 0); //Mathf.Clamp is used here to create a min and max value for Y as it prevents the value from going any higher or lower.
       
        if(transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        _life -= 1;

        if (_life < 1)
        {
            Destroy(gameObject);
        }
    }
}
