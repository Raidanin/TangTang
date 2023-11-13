using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public Slider exp;
    private GameObject player;
    private Player playerScripts;
    public Button restartButton;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScripts = player.GetComponent<Player>();

        restartButton.onClick.AddListener(OnRestartButtonClicked);
        restartButton.gameObject.SetActive(false);
            
     }

    // Update is called once per frame
    void Update()
    {
        exp.value = playerScripts.currentExp / playerScripts.maxExp;

        if (!player.gameObject.activeInHierarchy)
        {
            restartButton.gameObject.SetActive(true);   
           
        }

    }

    public void OnRestartButtonClicked()
    {
        // 씬을 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 시간 스케일을 다시 1로 설정
        Time.timeScale = 1;
    }

}
