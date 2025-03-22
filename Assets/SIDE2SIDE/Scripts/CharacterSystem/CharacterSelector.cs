using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharacterList characterList;
    [SerializeField] private GameObject[] charactersInScene;
    [SerializeField] private int showCharacter;
    [SerializeField] private int currentCharacter;
    
    //Character Selector Camera Effects
    [SerializeField] private Camera cam;
    [SerializeField] private Transform gameCameraPos;
    [SerializeField] private Transform charSelectorCameraPos;
    [SerializeField] private float camTransitionSpeed;
    public bool isGameOn;
    
    //UI Stuff
    [SerializeField] private RawImage unlockImage;
    [SerializeField] private Button selectCharacterButton;
    [SerializeField] private RectTransform charSelectPanel;
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private RectTransform tapToStart;
    [SerializeField] private RectTransform titleUI;

    [SerializeField] private GameObject playerObj;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize array
        charactersInScene = new GameObject[characterList.GetCharacterLength()];
        
        //spawn all the characters in scene  //GOTTA OPTIMIZE THIS 'CUZ NOW CHAR-SELECTOR is in GAME ITSELF...
        for (int i = 0; i < characterList.GetCharacterLength(); i++)
        {
            charactersInScene[i] = Instantiate(characterList.GetCharacter(i), transform);

            //enable only the first character, and disable all others
            if (i == 0)
            {
                charactersInScene[i].SetActive(true);
            }
            else
            {
                charactersInScene[i].SetActive(false);
            }
        }
        
        SwitchToCharSelector(false);
        HandleLockUI();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    

    //View Next character in list
    public void NextCharacter()
    {
        charactersInScene[showCharacter].SetActive(false); //disable current character
        showCharacter = (showCharacter + 1) % charactersInScene.Length; //increment character index
        charactersInScene[showCharacter].SetActive(true); //enable character
        
        HandleLockUI();
    }
    
    
    //View previous character in list
    public void PreviousCharacter()
    {
        charactersInScene[showCharacter].SetActive(false); //disable current character
        showCharacter = (showCharacter - 1 + charactersInScene.Length) % charactersInScene.Length; //decrement character index
        charactersInScene[showCharacter].SetActive(true); //enable character
        
        HandleLockUI();
    }

    
    //set selected character
    public void SelectCharacter()
    {
        currentCharacter = showCharacter;
        PlayerPrefs.SetInt("CURRENT_CHARACTER", currentCharacter);
        // SceneManager.LoadSceneAsync(1); //Load Game Scene after selecting character
        
        SwitchToCharSelector(true);
    }

    public void SwitchToCharSelector(bool isGame)
    {
        isGameOn = isGame;
        if (isGame)
        {
            
            if (Vector3.Distance(cam.transform.position, gameCameraPos.position) > 0.25f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, gameCameraPos.position, camTransitionSpeed * Time.unscaledTime);
                cam.transform.localRotation = gameCameraPos.localRotation;
            }
            else
            {
                tapToStart.gameObject.SetActive(true);
                titleUI.gameObject.SetActive(true);
                charSelectPanel.gameObject.SetActive(false);
                playerObj.SetActive(true);
                charactersInScene[showCharacter].SetActive(false);
            }
        }
        else
        {
            if (Vector3.Distance(cam.transform.position, charSelectorCameraPos.position) > 0.25f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, charSelectorCameraPos.position, camTransitionSpeed * Time.unscaledTime);
                cam.transform.localRotation = charSelectorCameraPos.localRotation;
            }
            else
            {
                tapToStart.gameObject.SetActive(false);
                titleUI.gameObject.SetActive(false);
                charSelectPanel.gameObject.SetActive(true);
                pausePanel.gameObject.SetActive(false);
                playerObj.SetActive(false);

                charactersInScene[showCharacter].SetActive(true);
                Debug.Log("Player: " + playerObj.activeSelf);
            }
        }
    }

    void HandleLockUI()
    {
        if (characterList.characters[showCharacter].GetIsLock())
        {
            //Character is Locked
            unlockImage.enabled = true;
            selectCharacterButton.enabled = false;
        }
        else
        {
            //Character is Unlocked
            unlockImage.enabled = false;
            selectCharacterButton.enabled = true;
        }
    }
}
