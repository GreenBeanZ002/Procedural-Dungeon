using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int GetDamage()    {        return PLAYER_DAMAGE;    }
    [Header("Walking Animation")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private Sprite upSprite;
    [SerializeField]
    private Sprite downSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;
    [SerializeField]
    private Sprite attackSpriteUp1;
    [SerializeField]
    private Sprite attackSpriteDown1;
    [SerializeField]
    private Sprite attackSpriteLeft1;
    [SerializeField]
    private Sprite attackSpriteRight1;
    [SerializeField]
    private Sprite attackSpriteUp2;
    [SerializeField]
    private Sprite attackSpriteDown2;
    [SerializeField]
    private Sprite attackSpriteLeft2;
    [SerializeField]
    private Sprite attackSpriteRight2;

    [SerializeField]
    private GameObject attackObj;

    private int orientation;

    [SerializeField]
    protected int PLAYER_DAMAGE = 3;

    public int playerDamage => PLAYER_DAMAGE;

    void Awake()
    {
        
    }

    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(xMove, yMove, 0);

        transform.position += movement * moveSpeed * Time.deltaTime;

        animate();
    }

    private void animate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            orientation = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            orientation = 2;
            gameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            orientation = 3;
            gameObject.GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            orientation = 4;
            gameObject.GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(AttackRoutine(orientation, gameObject.GetComponent<SpriteRenderer>()));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack(orientation);
        }
    }
    void attack(int direction)
    {
        if (direction == 1) // up
        {
            GameObject temp = Instantiate(attackObj, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f, gameObject.transform.position.z), Quaternion.identity);
            Destroy(temp, 0.1f);
        }
        if (direction == 2) // down
        {
            GameObject temp = Instantiate(attackObj, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.4f, gameObject.transform.position.z), Quaternion.identity);
            Destroy(temp, 0.1f);
        }
        if (direction == 3) // left
        {
            GameObject temp = Instantiate(attackObj, new Vector3(gameObject.transform.position.x - 0.4f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Destroy(temp, 0.1f);
        }
        if (direction == 4) // right 
        {
            GameObject temp = Instantiate(attackObj, new Vector3(gameObject.transform.position.x + 0.4f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            Destroy(temp, 0.1f);
        }




    }

    IEnumerator AttackRoutine(int direction, SpriteRenderer spriteRend)
    {


        if (direction == 1)
        {
            spriteRend.sprite = attackSpriteUp1;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = attackSpriteUp2;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = upSprite;
        }
        if (direction == 2)
        {
            spriteRend.sprite = attackSpriteDown1;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = attackSpriteDown2;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = downSprite;
        }
        if (direction == 3)
        {
            spriteRend.sprite = attackSpriteLeft1;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = attackSpriteLeft2;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = leftSprite;
        }
        if (direction == 4)
        {
            spriteRend.sprite = attackSpriteRight1;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = attackSpriteRight2;
            yield return new WaitForSeconds(0.1f);
            spriteRend.sprite = rightSprite;
        }
    }

    public void movePlayerToPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
