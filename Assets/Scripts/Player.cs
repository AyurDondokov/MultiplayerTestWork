using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviour, IPunObservable
{
    [Header("Values")]
    [SerializeField] private int speed;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private int health;

    [Header("GunSettings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPointUp;
    [SerializeField] private Transform gunPointDown;
    [SerializeField] private Transform gunPointRight;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshPro NickNameText;
    private Button shootBtn;

    [Header("Types of body")]
    [SerializeField] private GameObject LookRight;
    [SerializeField] private GameObject LookUp;
    [SerializeField] private GameObject LookDown;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator an;
    private PhotonView photonView;
    private VariableJoystick js; 

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        if (stream.IsWriting)
        {
            stream.SendNext(moveDirection);
        }
        else 
        {
            moveDirection = (Vector2)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        NickNameText.SetText(photonView.Owner.NickName);

        if (photonView.IsMine)
        {
            js = FindObjectOfType<VariableJoystick>();
            shootBtn = GameObject.Find("ShootBtn").GetComponent<Button>();
            shootBtn.onClick.AddListener(Shoot);

        }
    }

    private void Update()
    {
        if (photonView.IsMine) 
        {
            moveDirection = js.Direction.normalized;
        }
        Animation();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move() 
    {
        rb.velocity = moveDirection * speed;
    }
    private void Animation()
    {
        if (moveDirection.x != 0)
        {
            spriteTransform.rotation = (moveDirection.x > 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            if (angle > 45 && angle <= 135)
            {
                LookTo("Up");
            }
            else if (angle > -135 && angle <= -45)
            {
                LookTo("Down");
            }
            else
            {
                LookTo("Right");
            }

            an.SetBool("Run", true);
        }
        else 
        {
            an.SetBool("Run", false);
        }
    }

    private void LookTo(string look) 
    {
        LookUp.SetActive(look == "Up");
        LookDown.SetActive(look == "Down");
        LookRight.SetActive(look == "Right");
    
    }

    public void Shoot()
    {
        if (LookUp.activeSelf)
            PhotonNetwork.Instantiate(bulletPrefab.name, gunPointUp.position, Quaternion.identity)
                .GetComponent<Bullet>().SetStartValues(Vector2.up);
        else if (LookDown.activeSelf)
            PhotonNetwork.Instantiate(bulletPrefab.name, gunPointDown.position, Quaternion.identity)
                .GetComponent<Bullet>().SetStartValues(Vector2.down);
        else if (spriteTransform.rotation == Quaternion.Euler(0, 0, 0))
            PhotonNetwork.Instantiate(bulletPrefab.name, gunPointRight.position, Quaternion.identity)
                .GetComponent<Bullet>().SetStartValues(Vector2.right);
        else if (spriteTransform.rotation == Quaternion.Euler(0, 180, 0))
            PhotonNetwork.Instantiate(bulletPrefab.name, gunPointRight.position, Quaternion.identity)
                .GetComponent<Bullet>().SetStartValues(Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() && !collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            health -= 1;
            if (health <= 0)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("Lobby");
            }
        }
    }

}
