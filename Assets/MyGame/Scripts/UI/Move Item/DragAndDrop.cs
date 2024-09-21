using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using DamageNumbersPro;

public class DragAndDrop : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 shopPosition;
    private float moveSpeed = 20f;
    private int indexEnterBag = -1;
    private Vector3 bagPosition;
    private Vector3 oldBagPosition;
    private bool isBuy = false;
    private int oldIndexEnterBag = -1;

    public Status status;
    public enum Status
    {
        FREE, DRAGGING
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không tồn tại trên GameObject. Hãy thêm nó qua Unity Editor.");
        }

        shopPosition = transform.position;
    }

    private void OnMouseDown()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        status = Status.DRAGGING;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private async void OnMouseUp()
    {
        // check if swap
        if (indexEnterBag != -1 && isBuy && oldIndexEnterBag != -1)
        {
            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];
            if (current != null)
            {
                status = Status.FREE;
                rb.MovePosition(bagPosition);

                GameManager.Instance.UpdateWeaponUI(indexEnterBag - 1, this.gameObject);
                GameManager.Instance.UpdateWeaponUI(oldIndexEnterBag - 1, current);

                current.GetComponent<DragAndDrop>().oldIndexEnterBag = oldIndexEnterBag;
                current.GetComponent<DragAndDrop>().oldBagPosition = oldBagPosition;

                StartCoroutine(Move(current, oldBagPosition, false));

                oldIndexEnterBag = indexEnterBag;
                oldBagPosition = bagPosition;
                return;
            }

        }

        // shop -> shop
        if (indexEnterBag == -1 && !isBuy)
        {
            status = Status.FREE;
            StartCoroutine(Move(this.gameObject, shopPosition, false));
            return;
        }

        // bag -> bag
        if (indexEnterBag != -1 && isBuy)
        {
            status = Status.FREE;
            rb.MovePosition(bagPosition);

            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];
            GameManager.Instance.UpdateWeaponUI(indexEnterBag - 1, this.gameObject);
            // bag -> bag without weapon
            if (current == null)
            {
                if (oldIndexEnterBag != -1)
                {
                    GameManager.Instance.UpdateWeaponUI(oldIndexEnterBag - 1, null);
                }
                GameManager.Instance.UpdateShadow();
            }
            // bag -> bag with weapon
            else
            {
                StartCoroutine(Move(current, GameManager.Instance.storagePosition.position, false));
                current.GetComponent<DragAndDrop>().oldIndexEnterBag = -1;
                current.GetComponent<DragAndDrop>().oldBagPosition = GameManager.Instance.storagePosition.position;
            }

            oldIndexEnterBag = indexEnterBag;
            oldBagPosition = bagPosition;
            return;
        }

        // bag -> storage
        if (indexEnterBag == -1 && isBuy && oldIndexEnterBag != -1)
        {
            status = Status.FREE;
            GameManager.Instance.UpdateWeaponUI(oldIndexEnterBag - 1, null);
            StartCoroutine(Move(this.gameObject, GameManager.Instance.storagePosition.position, false));
            oldBagPosition = GameManager.Instance.storagePosition.position;
            oldIndexEnterBag = -1;
            return;
        }

        // shop -> bag (buy)
        if (indexEnterBag != -1)
        {
            List<AttributeOption> attributes = ItemManager.Instance.GetAttributes(this.gameObject);
            int price = (int)ItemManager.Instance.GetValue(attributes, WeaponAttribute.PRICE);
            if (GameManager.Instance.leftChar.gold < price)
            {
                GameManager.Instance.numberPrefabMesh.Spawn(this.gameObject.transform.position, "Out of gold!");
                status = Status.FREE;
                StartCoroutine(Move(this.gameObject, shopPosition, false));
                return;
            }
            GameManager.Instance.leftChar.gold -= price;
            GameManager.Instance.UpdateUI();

            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];
            GameManager.Instance.UpdateWeaponUI(indexEnterBag - 1, this.gameObject);
            // shop -> bag
            if (current == null)
            {
                //rb.MovePosition(bagPosition);
                StartCoroutine(Move(this.gameObject, bagPosition, true));
                GameManager.Instance.UpdateShadow();
            }
            // new weapon: shop -> bag
            // previous weapon: bag -> storage
            else
            {
                rb.MovePosition(bagPosition);
                StartCoroutine(Move(current, GameManager.Instance.storagePosition.position, true));
                current.GetComponent<DragAndDrop>().oldIndexEnterBag = -1;
                current.GetComponent<DragAndDrop>().oldBagPosition = GameManager.Instance.storagePosition.position;
            }

            status = Status.FREE;
            isBuy = true;
            oldIndexEnterBag = indexEnterBag;
            oldBagPosition = bagPosition;
            return;
        }

        // storage -> storage
        if (indexEnterBag == -1 && isBuy && oldIndexEnterBag == -1)
        {
            status = Status.FREE;
            StartCoroutine(Move(this.gameObject, GameManager.Instance.storagePosition.position, false));
            oldIndexEnterBag = -1;
            oldBagPosition = GameManager.Instance.storagePosition.position;
            return;
        }
    }

    private void Update()
    {
        if (status == Status.DRAGGING)
        {
            Vector3 mousePos = GetMouseWorldPos();
            rb.MovePosition(Vector3.Lerp(transform.position, mousePos, moveSpeed * Time.deltaTime));
        }
    }

    private IEnumerator Move(GameObject weapon, Vector3 to, bool isBuy)
    {
        while (Vector3.Distance(weapon.transform.position, to) > 0.5f)
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, to, moveSpeed * Time.deltaTime);
            yield return null;
        }

        weapon.transform.position = to;

        if (to == GameManager.Instance.storagePosition.position)
        {
            weapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        GameManager.Instance.UpdateShadow();

        if (isBuy)
        {
            this.gameObject.transform.SetParent(GameManager.Instance.items, true);

        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("bag"))
        {
            indexEnterBag = Int32.Parse(collision.tag.Split(new[] { "bag" }, StringSplitOptions.None)[1]);
            bagPosition = collision.transform.position;

            SpriteRenderer shadow = collision.GetComponent<SpriteRenderer>();
            if (shadow.color.a != 0f)
            {
                shadow.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("bag"))
        {
            int index = Int32.Parse(collision.tag.Split(new[] { "bag" }, StringSplitOptions.None)[1]);
            indexEnterBag = -1;

            SpriteRenderer shadow = collision.GetComponent<SpriteRenderer>();
            if (shadow.color.a != 0f) 
            { 
                shadow.color = new Color(0, 0, 0, 0.5f);
            }
        }
    }
}
