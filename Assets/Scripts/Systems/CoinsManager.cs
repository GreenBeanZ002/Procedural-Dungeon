using UnityEngine;
using TMPro;

public class coinsManager : MonoBehaviour
{
    public static coinsManager Instance { get; private set; }

    [SerializeField] private int coins = 0;
    [SerializeField] private GameObject coinManager;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        text.text = coins.ToString();
    }

    public void addCoins()
    {
        coinManager.GetComponent<AudioSource>().Play();
        coins++;
    }

    public bool removeCoins(int Amt, int Price)
    {
        if (coins >= Price)
        {
            coins -= Amt;
            return true;
        }
        return false;
    }

    public void addSpecificAmtOfCoins(int Amt)
    {
        coinManager.GetComponent<AudioSource>().Play();
        coins += Amt;
    }
}