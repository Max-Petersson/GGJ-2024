using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class WeaponsHandler : MonoBehaviour
{
    [SerializeField] private List<Sprite> _weapons = null;
    [SerializeField] private List<Sprite> _altWeaponSprites = null;
    [SerializeField] private float _weaponSwitchInterval = 3f;
    [SerializeField] private GameObject _raggeDollGameObject = null;
    [SerializeField] private Collider2D _baseBallCollider = null;
    [SerializeField] private Collider2D _grabCollider = null;

    [Header("VFX")]
    [SerializeField] private GameObject _smokeVFX = null;
    [SerializeField] private Transform _smokePoint = null;
    
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField] private StrikeBounceHandler _strikeBounceHandlerRef; 
    private float _startTime;
    private float _timer = 0;
    private Vector2 _mousePos;
    private bool _canInteract;

    private int _currentWeapon = 0;


    private enum Weapons
    {
        Hand,
        Bat,
        Gun
    }

    private void OnEnable()
    {
        EventManager.Instance.OnRelease += ChangeWeapon;
        EventManager.Instance.OnSwingBat += ChangeWeapon;
        EventManager.Instance.OnShoot += ChangeWeapon;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnRelease -= ChangeWeapon;
        EventManager.Instance.OnSwingBat -= ChangeWeapon;
        EventManager.Instance.OnShoot -= ChangeWeapon;
    }

    private void Awake()
    {
        _baseBallCollider.enabled = false;
    }

    private void Start()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        if (_grabCollider == null)
            _grabCollider = gameObject.AddComponent<CircleCollider2D>();
        
        _grabCollider.isTrigger = true;

        _spriteRenderer.sprite = _weapons[0];
        _spriteRenderer.sortingOrder = 10;

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
        
        var currentWeapon = _weapons.IndexOf(_spriteRenderer.sprite);

        switch (currentWeapon)
        {
            case (int)Weapons.Hand:
                _baseBallCollider.enabled = false;
                _grabCollider.enabled = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (Input.GetMouseButtonDown(0) && _canInteract)
                {
                    SetWeaponSprite(0, true);
                    _currentWeapon = _altWeaponSprites.IndexOf(_spriteRenderer.sprite);
                    EventManager.Instance.OnGrab?.Invoke();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    SetWeaponSprite(0, false);
                    _currentWeapon = _weapons.IndexOf(_spriteRenderer.sprite);
                }
                break;

            case (int)Weapons.Bat:
                _grabCollider.enabled = false;
                if (Input.GetMouseButtonDown(0))
                {
                    if (_baseBallCollider.enabled == false)
                    {
                        _baseBallCollider.enabled = true;
                    }
                    transform.rotation = Quaternion.Euler(0, 0, 30);
                }

                if (_canInteract)
                {
                    var velocity = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));
                    _currentWeapon = _weapons.IndexOf(_spriteRenderer.sprite);
                    _strikeBounceHandlerRef.AddBounceVelocity(velocity, 30);
                    //Instantiate(_smokeVFX, _smokePoint.position, Quaternion.identity);
                    EventManager.Instance.OnSwingBat?.Invoke();
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    transform.rotation = Quaternion.Euler(0, 0, -30);
                }
                break;

            case (int)Weapons.Gun:
                _baseBallCollider.enabled = false;
                transform.rotation = Quaternion.Euler(0, 0, 30);
                if (Input.GetMouseButtonDown(0))
                {
                    SetWeaponSprite(2, false);
                    _currentWeapon = _weapons.IndexOf(_spriteRenderer.sprite);
                    transform.rotation = Quaternion.Euler(0, 0, -30);

                    EventManager.Instance.OnShoot?.Invoke();
                }
                break;

            break;
        }
        

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = _mousePos;
      
    }

    private void ChangeWeapon()
    {
        

        var newWeapon = Random.Range(0, _weapons.Count);
        
        while (newWeapon == _currentWeapon)
        {
            newWeapon = Random.Range(0, _weapons.Count);
        }

        SetWeaponSprite(newWeapon, false);
    }

    private void SetWeaponSprite(int weapon, bool altArt)
    {
        if (altArt)
        {
            _spriteRenderer.sprite = _altWeaponSprites[weapon];
        }
        else
        {
            _spriteRenderer.sprite = _weapons[weapon];
        }
    }

}   
