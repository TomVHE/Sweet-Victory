using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class PickUp : MonoBehaviour
{
    public int damage = 10;

    [Header("The offset")]
    public Vector3 rotation;
    public Vector3 position;

    [HideInInspector]
    public bool equipped;

    private Transform _parent;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _thrown;
    
    private void Start()
    {
        _parent = transform.parent;

        if (_rigidbody == null)
        {
            if(GetComponent<Rigidbody>()){
                _rigidbody = GetComponent<Rigidbody>();
            }
            else {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        }

        if (_collider == null)
        {
            if(GetComponent<Collider>()){
                _collider = GetComponent<Collider>();
            }
            else {
                _collider = gameObject.AddComponent<MeshCollider>();
                MeshCollider test = _collider as MeshCollider;
                test.convex = true;
            }
        }

        if(transform.tag != "Pickup")
        {
            transform.tag = "Pickup";
        }
        
    }

    private void Update()
    {

    }

    public void Equip(Transform _newParent)
    {
        equipped = true;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
        _collider.enabled = false;
        transform.parent = _newParent;
        transform.localPosition = Vector3.zero + position;
        transform.localEulerAngles = Vector3.zero + rotation;
    }

    public void Unequip()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _collider.enabled = true;
        _thrown = true;
        transform.parent = _parent;
        equipped = false;
    }
}
