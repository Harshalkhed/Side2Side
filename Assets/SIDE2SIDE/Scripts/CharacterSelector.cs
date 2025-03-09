using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharacterList characterList;
    [SerializeField] private GameObject[] charactersInScene;
    [SerializeField] private int showCharacter;
    [SerializeField] private int currentCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize array
        charactersInScene = new GameObject[characterList.GetCharacterLength()];
        
        //spawn all the characters in scene
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
    }
    
    
    //View previous character in list
    public void PreviousCharacter()
    {
        charactersInScene[showCharacter].SetActive(false); //disable current character
        showCharacter = (showCharacter - 1 + charactersInScene.Length) % charactersInScene.Length; //decrement character index
        charactersInScene[showCharacter].SetActive(true); //enable character
    }

    
    //set selected character
    public void SelectCharacter()
    {
        currentCharacter = showCharacter;
        PlayerPrefs.SetInt("CURRENT_CHARACTER", currentCharacter);
        SceneManager.LoadSceneAsync(1); //Load Game Scene after selecting character
    }
}
