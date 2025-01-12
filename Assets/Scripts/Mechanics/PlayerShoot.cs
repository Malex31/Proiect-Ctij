using UnityEngine;
using Platformer.Mechanics;

public class PlayerShoot : MonoBehaviour
{
    PlayerControls controls;
    public Animator animator;

    public GameObject bullet;
    public Transform bulletHole;
    public float force = 200;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        // Subscribe to the Shoot action
        controls.Land.Shoot.performed += ctx => Fire();
    }

    void Fire()
    {
        animator.SetTrigger("shoot");
        GameObject go=Instantiate(bullet, bulletHole.position, bullet.transform.rotation);
        if (GetComponent<PlayerController>().isFacingRight)
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
        else
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);

    }
    
}