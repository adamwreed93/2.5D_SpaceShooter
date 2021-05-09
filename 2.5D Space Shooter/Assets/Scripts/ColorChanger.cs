using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Player _player;
    private Renderer _spriteRenderer;

    [SerializeField] private Color _changeToDefaultColor;
    [SerializeField] private Color _changeToYellow;
    [SerializeField] private Color _changeToRed;

    [SerializeField] private int _shieldLife = 3;



    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_player != null)
        {
            switch (_shieldLife)
            {
                case 0: //if _shieldLife = 0.
                    _spriteRenderer.material.color = _changeToDefaultColor;
                    _player.DeactivateShield();
                    break;
                case 1: //if _shieldLife = 1.
                    _spriteRenderer.material.color = _changeToRed;
                    break;
                case 2: //if _shieldLife = 2.
                    _spriteRenderer.material.color = _changeToYellow;
                    break;
                case 3: //if _shieldLife = 3.
                    _spriteRenderer.material.color = _changeToDefaultColor;
                    break;
                default:
                    _player.DeactivateShield();
                    break;
            }
        }
    }

    public void DamageShield()
    {
        _shieldLife--;
    }

    public void Restoreshield()
    {
        _shieldLife = 3;
    }
}
