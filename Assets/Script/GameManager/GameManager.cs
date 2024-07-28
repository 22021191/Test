using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public Player player;
    [SerializeField] public Vector3 pos;
    public bool key;
    [SerializeField] public GameObject door;
    [SerializeField] public Sprite doorOpen;
    [SerializeField] public GameObject endGame;
    [Header("Orther")]
    [SerializeField] public Animator transitionAnim;
    public bool isTransition = false;
    [Header("Setting Panpel")]
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Sprite onSfx, onMusic;
    [SerializeField] private Sprite offSfx, offMusic;
    [SerializeField] private Slider sfxSlider, musicSlider;
    [Header("Main Menu")]
    [SerializeField] private GameObject settingPanpel;
    [SerializeField] private GameObject mainMenu;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PopDownAllPanpel();
        /*transitionAnim.SetBool("Start", false);
        transitionAnim.SetBool("End", true);*/
        transitionAnim.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
    #region Main Menu

    public void NewGame()
    {
        StartCoroutine(TransitionScene("Game"));

    }



    public void PopDownAllPanpel()
    {
        mainMenu.SetActive(false);
        settingPanpel.SetActive(false);
        
    }

    public void PopUpSettingPanpel()
    {
        PopDownAllPanpel();
        mainMenu.SetActive(true);
        settingPanpel.SetActive(true);
    }

    #endregion
    #region Setting
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        if (!AudioManager.Instance.musicSource.mute)
        {
            musicButton.image.overrideSprite = onMusic;
        }
        else
        {
            musicButton.image.overrideSprite = offMusic;
        }
    }
    public void ToggleSfx()
    {
        AudioManager.Instance.ToggleSfx();
        if (!AudioManager.Instance.sfxSource.mute)
        {
            sfxButton.image.overrideSprite = onSfx;
        }
        else
        {
            sfxButton.image.overrideSprite = offSfx;
        }
    }

    public void SliderMusic()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SliderSfx()
    {
        AudioManager.Instance.SfxVolume(sfxSlider.value);
    }

    #endregion
    #region Orther
    IEnumerator TransitionScene(string sceneName)
    {
        transitionAnim.gameObject.SetActive(true);
        transitionAnim.SetBool("Start", true);
        transitionAnim.SetBool("End", false);
        yield return new WaitForSeconds(1.5f);
        PopDownAllPanpel();
        yield return new WaitForSeconds(2);
        transitionAnim.SetBool("Start", false);
        transitionAnim.SetBool("End", true);
        SceneManager.LoadScene(sceneName);
        AudioManager.Instance.PlayMusic("Music");
        yield return new WaitForSeconds(2f);
        transitionAnim.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void RePlay()
    {
        player.ResetData(pos);
        player.EnableControls();
        AudioManager.Instance.PlaySfx("Die");
    }
    public IEnumerator Transition(Vector3 pos)
    {
        yield return new WaitForSeconds(0.5f);

        transitionAnim.gameObject.SetActive(true);
        transitionAnim.SetBool("Start", true);
        transitionAnim.SetBool("End", false);
        yield return new WaitForSeconds(1.5f);
        PopDownAllPanpel();
        yield return new WaitForSeconds(0.5f);
        transitionAnim.SetBool("Start", false);
        transitionAnim.SetBool("End", true);
        yield return new WaitForSeconds(2f);

        transitionAnim.gameObject.SetActive(false);
    }
    #endregion
   
}
