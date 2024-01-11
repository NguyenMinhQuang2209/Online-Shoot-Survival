using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private int damage = 1;
    private float speed = 1f;
    private Rigidbody2D rb;
    private Transform target = null;
    bool isInit = false;
    [SerializeField] private ItemName itemName;
    bool useCustomBullet = false;
    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!IsServer || !useCustomBullet)
        {
            return;
        }
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            if (isInit)
            {
                Destroy(gameObject);
            }
        }
    }
    public void BulletInit(int damage, float speed, float delayDieTime, Vector3 shootDir, ulong targetId = 0)
    {
        this.damage = damage;
        this.speed = speed;

        useCustomBullet = targetId != 0;

        if (targetId == 0)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            rb.AddForce(shootDir * speed, ForceMode2D.Force);
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.TryGetComponent<NetworkObject>(out var networkObject))
                {
                    if (networkObject.NetworkObjectId == targetId)
                    {
                        target = player.transform;
                        break;
                    }
                }
            }
        }
        isInit = true;
        Destroy(gameObject, delayDieTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsServer)
        {
            if (useCustomBullet)
            {
                if (collision.gameObject == target.gameObject)
                {
                    if (collision.gameObject.TryGetComponent<Health>(out var health))
                    {
                        health.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
    public string GetItemName()
    {
        return itemName.ToString();
    }
}
