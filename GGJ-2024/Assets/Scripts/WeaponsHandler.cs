using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private List<Sprite> _weapons;
    [SerializeField] private float _weaponSwitchInterval = 3f;
    [SerializeField] private GameObject _raggeDollGameObject = null;
    
    private SpriteRenderer _spriteRenderer = null;
    private Collider2D _clickCollider = null;
    private float _startTime;
    private float _timer = 0;
    private Vector2 _mousePos;
    private bool _canInteract;

    private enum Weapons
    {
        Hand,
        Bat,
        Gun
    }

    private void OnEnable()
    {
        EventManager.Instance.OnRelease += ChangeWeapon;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnRelease -= ChangeWeapon;
    }

    private void Start()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        if (_clickCollider == null)
            _clickCollider = gameObject.AddComponent<CircleCollider2D>();
        
        _clickCollider.isTrigger = true;

        _spriteRenderer.sprite = _weapons[0];

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Cursor.visible = false;

        transform.position = _mousePos;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = false;
    }

    private void OnTriggerEnter2D()
    {
        _canInteract = true;
    }

    private void OnTriggerExit2D()
    {
        _canInteract = false;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_canInteract)
        {
            var currentWeapon = _weapons.IndexOf(_spriteRenderer.sprite);

            switch (currentWeapon)
            {
                case (int)Weapons.Hand:
                    EventManager.Instance.OnGrab?.Invoke();
                    break;

                case (int)Weapons.Bat:
                    EventManager.Instance.OnSwingBat?.Invoke();
                    break;

                case (int)Weapons.Gun:
                    EventManager.Instance.OnShoot?.Invoke();
                    break;

                break;
            }
        }

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = _mousePos;
      
    }

    private void ChangeWeapon()
    {
        SetWeaponSprite(Random.Range(0, _weapons.Count));
    }

    private void SetWeaponSprite(int weapon)
    {
        _spriteRenderer.sprite = _weapons[weapon];
    }

}   
