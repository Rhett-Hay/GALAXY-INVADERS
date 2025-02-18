using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    // ID numbers for each powerup: 0-Triple Shot 1-Speed 2-Shield 3-Ammo
    // 4-Health
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _powerClip;
    [SerializeField] private float _volume;

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerClip, new Vector3(0, 0, -10f), _volume);
            Debug.Log(_powerClip.name);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoBoostActive();
                        break;
                    case 4:
                        player.HealthBoostActive();
                        break;
                    case 5:
                        player.TripleShot2Active();
                        break;
                    default:
                        Debug.Log("Default value!");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
