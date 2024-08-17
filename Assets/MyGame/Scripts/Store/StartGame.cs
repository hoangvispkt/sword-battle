using UnityEngine;

public class StartGame : MonoBehaviour
{
    public bool isPlay=false;
    [SerializeField] GameObject PlayGame;
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Player;
    public void Play()
    {
        isPlay = true;
        PlayGame.SetActive(false);
        Enemy.SetActive(true);
        Player.SetActive(true);
    }



}
