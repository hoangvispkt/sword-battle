using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject PlayGame;
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Player;
    public void Play()
    {
        PlayGame.SetActive(false);
        Enemy.SetActive(true);
        Player.SetActive(true);
    }



}
