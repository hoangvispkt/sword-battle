using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.Jobs;
using TMPro;

public class DragAndDrop : MonoBehaviour
{
    //public bool isEquip = false;
    //public bool isBuy = false;
    //public bool isEnter = false;
    //private Vector3 offset;
    //private Vector3 targetPosition;
    //private Rigidbody2D rb;
    //private float dragSpeed = 20f;
    //private Vector3 currentPosition;

    //private bool isMovingToStorage = false;
    //private GameObject movingWeapon;  // Vũ khí đang được di chuyển về storage
    //private SpriteRenderer bagRenderer;
    //public Status status;
    //private int bagIndex = -1;
    //private int oldBagIndex = -1;

    //public enum Status
    //{
    //    FREE, DRAGGING, RETURNING
    //}

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();

    //    if (rb == null)
    //    {
    //        Debug.LogError("Rigidbody2D không tồn tại trên GameObject. Hãy thêm nó qua Unity Editor.");
    //    }

    //    currentPosition = transform.position;
    //}

    //private void OnMouseDown()
    //{
    //    status = Status.DRAGGING;
    //    isEquip = false;
    //    offset = transform.position - GetMouseWorldPos();
    //    this.oldBagIndex = bagIndex;
    //}

    //private void OnMouseUp()
    //{
    //    if (isEnter && !isEquip)
    //    {
    //        if (bagIndex != -1)
    //        {
    //            // Nếu đã có weapon gắn vào bag này thì di chuyển nó về storage
    //            if (GameManager.Instance.bagAssignments[bagIndex - 1] != null)
    //            {
    //                GameObject previousWeapon = GameManager.Instance.bagAssignments[bagIndex - 1];
    //                MoveToStorage(previousWeapon);

    //                // Cập nhật lại enteredBags và bagAssignments cho weapon cũ
    //                GameManager.Instance.enteredBags[oldBagIndex - 1] = null;
    //                GameManager.Instance.bagAssignments[oldBagIndex - 1] = null;
    //            }

    //            // Gắn weapon mới vào bag
    //            rb.MovePosition(targetPosition);
    //            status = Status.FREE;
    //            isEquip = true;
    //            isBuy = true;

    //            // Cập nhật weapon vào mảng bagAssignments
    //            GameManager.Instance.bagAssignments[bagIndex - 1] = this.gameObject;
    //            GameManager.Instance.enteredBags[bagIndex - 1] = this.bagRenderer;  // Lưu bag tại vị trí tương ứng (bagIndex - 1)

    //            // Reset màu của tất cả các bag
    //            ResetAllBagColors();
    //        }
    //    }
    //    else
    //    {
    //        status = Status.RETURNING;
    //        isEquip = false;
    //        ResetAllBagColors();
    //    }
    //}

    //private void Update()
    //{
    //    if (status == Status.DRAGGING)
    //    {
    //        Vector3 mousePos = GetMouseWorldPos();
    //        Vector3 targetPos = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, transform.position.z);
    //        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime));
    //    }
    //    else if (status == Status.RETURNING)
    //    {
    //        if (isBuy) currentPosition = targetPosition;
    //        rb.MovePosition(Vector3.Lerp(transform.position, currentPosition, dragSpeed * Time.deltaTime));

    //        if (Vector3.Distance(transform.position, currentPosition) < 0.01f)
    //        {
    //            status = Status.FREE;
    //        }
    //    }

    //    // Di chuyển weapon về storage từ từ
    //    if (isMovingToStorage && movingWeapon != null)
    //    {
    //        movingWeapon.transform.position = Vector3.Lerp(movingWeapon.transform.position, GameManager.Instance.storagePosition.position, dragSpeed * Time.deltaTime);

    //        // Kiểm tra nếu weapon đã đến gần vị trí storage
    //        if (Vector3.Distance(movingWeapon.transform.position, GameManager.Instance.storagePosition.position) < 0.01f)
    //        {

    //            // Enable lại collider sau khi đã đến vị trí storage
    //            BoxCollider2D weaponCollider = movingWeapon.GetComponent<BoxCollider2D>();
    //            if (weaponCollider != null)
    //            {
    //                weaponCollider.enabled = true;
    //            }

    //            isMovingToStorage = false;  // Dừng di chuyển khi đã tới vị trí
    //            movingWeapon = null;        // Reset vũ khí di chuyển
    //        }
    //    }
    //}

    //private Vector3 GetMouseWorldPos()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = Camera.main.nearClipPlane;
    //    return Camera.main.ScreenToWorldPoint(mousePos);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    int bagIndex = GetBagIndex(collision.tag);  // Lấy chỉ số của bag (1-7)
    //    this.bagIndex = bagIndex;

    //    if (bagIndex != -1)
    //    {
    //        SpriteRenderer bagRenderer = collision.GetComponent<SpriteRenderer>();

    //        if (bagRenderer != null)
    //        {
    //            bagRenderer.color = Color.cyan;
    //            this.bagRenderer = bagRenderer;
    //        }

    //        targetPosition = collision.transform.position;
    //        isEnter = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    int bagIndex = GetBagIndex(collision.tag);  // Lấy chỉ số của bag (1-7)
    //    this.bagIndex = -1;

    //    if (bagIndex != -1 && !isEquip)
    //    {
    //        SpriteRenderer bagRenderer = collision.GetComponent<SpriteRenderer>();

    //        if (bagRenderer != null)
    //        {
    //            bagRenderer.color = Color.white;
    //            this.bagRenderer = null;  // Xóa bag khỏi mảng
    //        }

    //        isEnter = false;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    int bagIndex = GetBagIndex(collision.tag);  // Lấy chỉ số của bag (1-7)
    //    this.bagIndex = bagIndex;

    //    if (bagIndex != -1)
    //    {
    //        isEnter = true;
    //    }
    //}

    //private void ResetAllBagColors()
    //{
    //    for (int i = 0; i < GameManager.Instance.enteredBags.Length; i++)
    //    {
    //        if (GameManager.Instance.enteredBags[i] != null)
    //        {
    //            GameManager.Instance.enteredBags[i].color = Color.white;
    //        }
    //    }
    //}

    //private int GetBagIndex(string tag)
    //{
    //    switch (tag)
    //    {
    //        case "bag1": return 1;
    //        case "bag2": return 2;
    //        case "bag3": return 3;
    //        case "bag4": return 4;
    //        case "bag5": return 5;
    //        case "bag6": return 6;
    //        case "bag7": return 7;
    //        default: return -1;
    //    }
    //}

    //private void MoveToStorage(GameObject weapon)
    //{
    //    // Bắt đầu di chuyển weapon cũ về vị trí storage
    //    if (weapon != null && GameManager.Instance.storagePosition != null)
    //    {
    //        movingWeapon = weapon;
    //        isMovingToStorage = true;  // Kích hoạt di chuyển weapon về storage

    //        // Disable collider trước khi di chuyển
    //        BoxCollider2D weaponCollider = weapon.GetComponent<BoxCollider2D>();
    //        if (weaponCollider != null)
    //        {
    //            weaponCollider.enabled = false;
    //        }
    //    }
    //}

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
        status = Status.DRAGGING;
    }

    private void OnMouseUp()
    {
        // check if swap
        if (indexEnterBag != -1 && isBuy && oldIndexEnterBag != -1)
        {
            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];
            if (current != null)
            {
                status = Status.FREE;
                rb.MovePosition(bagPosition);

                GameManager.Instance.bagAssignments[indexEnterBag - 1] = this.gameObject;
                GameManager.Instance.bagAssignments[oldIndexEnterBag - 1] = current;

                current.GetComponent<DragAndDrop>().oldIndexEnterBag = oldIndexEnterBag;
                current.GetComponent<DragAndDrop>().oldBagPosition = oldBagPosition;

                StartCoroutine(Move(current, oldBagPosition));

                oldIndexEnterBag = indexEnterBag;
                oldBagPosition = bagPosition;

                UpdateShadow();
                return;
            }

        }

        // check return to shop
        if (indexEnterBag == -1 && !isBuy)
        {
            status = Status.FREE;
            StartCoroutine(Move(this.gameObject, shopPosition));

            UpdateShadow();
            return;
        }

        // check move to another bag
        if (indexEnterBag != -1 && isBuy)
        {
            status = Status.FREE;
            rb.MovePosition(bagPosition);

            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];
            if (current == null)
            {
                GameManager.Instance.bagAssignments[indexEnterBag - 1] = this.gameObject;
                if (oldIndexEnterBag != -1)
                {
                    GameManager.Instance.bagAssignments[oldIndexEnterBag - 1] = null;
                }
            }
            else
            {
                StartCoroutine(Move(current, GameManager.Instance.storagePosition.position));
                GameManager.Instance.bagAssignments[indexEnterBag - 1] = this.gameObject;
                current.GetComponent<DragAndDrop>().oldIndexEnterBag = -1;
                current.GetComponent<DragAndDrop>().oldBagPosition = GameManager.Instance.storagePosition.position;
            }

            oldIndexEnterBag = indexEnterBag;
            oldBagPosition = bagPosition;

            UpdateShadow();
            return;
        }

        // check return to bag
        if (indexEnterBag == -1 && isBuy && oldIndexEnterBag != -1)
        {
            status = Status.FREE;
            StartCoroutine(Move(this.gameObject, GameManager.Instance.storagePosition.position));
            GameManager.Instance.bagAssignments[oldIndexEnterBag - 1] = null;
            oldIndexEnterBag = -1;
            oldBagPosition = GameManager.Instance.storagePosition.position;

            UpdateShadow();
            return;
        }

        // buy weapon
        if (indexEnterBag != -1)
        {
            GameObject current = GameManager.Instance.bagAssignments[indexEnterBag - 1];

            // equip
            if (current == null)
            {
                GameManager.Instance.bagAssignments[indexEnterBag - 1] = this.gameObject;
                rb.MovePosition(bagPosition);
            }
            // check return to storage
            else
            {
                rb.MovePosition(bagPosition);
                StartCoroutine(Move(current, GameManager.Instance.storagePosition.position));
                GameManager.Instance.bagAssignments[indexEnterBag - 1] = this.gameObject;
                current.GetComponent<DragAndDrop>().oldIndexEnterBag = -1;
                current.GetComponent<DragAndDrop>().oldBagPosition = GameManager.Instance.storagePosition.position;
            }

            status = Status.FREE;
            isBuy = true;
            oldIndexEnterBag = indexEnterBag;
            oldBagPosition = bagPosition;

            UpdateShadow();
            return;
        }

        if (indexEnterBag == -1 && isBuy && oldIndexEnterBag == -1)
        {
            status = Status.FREE;
            StartCoroutine(Move(this.gameObject, GameManager.Instance.storagePosition.position));
            oldIndexEnterBag = -1;
            oldBagPosition = GameManager.Instance.storagePosition.position;

            UpdateShadow();
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

    private IEnumerator Move(GameObject weapon, Vector3 to)
    {
        while (Vector3.Distance(weapon.transform.position, to) > 0.01f)
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, to, moveSpeed * Time.deltaTime);
            yield return null;
        }

        weapon.transform.position = to;
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
            shadow.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.StartsWith("bag"))
        {
            int index = Int32.Parse(collision.tag.Split(new[] { "bag" }, StringSplitOptions.None)[1]);
            indexEnterBag = -1;

            SpriteRenderer shadow = collision.GetComponent<SpriteRenderer>();
            shadow.color = new Color(0, 0, 0, 0.5f);
        }
    }

    private void UpdateShadow()
    {
        for (int i = 0; i < GameManager.Instance.bagAssignments.Length; i++)
        {
            if (GameManager.Instance.bagAssignments[i] == null)
            {
                GameManager.Instance.bags[i].SetActive(true);
            }
            else
            {
                GameManager.Instance.bags[i].SetActive(false);
            }
        }
    }
}
