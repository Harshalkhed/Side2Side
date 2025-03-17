using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlocker : MonoBehaviour
{
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private CharacterList characterList;

    private Dictionary<int, int> levelToCharacterMap = new Dictionary<int, int>();
    private int charToUnlock; // Index of next character to unlock

    void Start()
    {
        gamePlayManager = FindObjectOfType<GamePlayManager>();
        LoadCharacterMap();
        LoadCharToUnlock();
    }

    void LoadCharacterMap()
    {
        for (int i = 0; i < characterList.characters.Length; i++)
        {
            int unlockLevel = characterList.characters[i].GetUnlockLevel();
            if (!levelToCharacterMap.ContainsKey(unlockLevel))
            {
                levelToCharacterMap.Add(unlockLevel, i);
            }
        }
    }

    void LoadCharToUnlock()
    {
        // Get stored value OR find the next locked character dynamically
        charToUnlock = PlayerPrefs.GetInt("NextCharacterToUnlock", FindNextUnlockableCharacter());
        Debug.Log($"Loaded charToUnlock: {charToUnlock}");
    }

    int FindNextUnlockableCharacter()
    {
        int currentStage = gamePlayManager.GetCurrentStage();

        // Find the first character that should be unlocked at or after this stage
        foreach (var entry in levelToCharacterMap)
        {
            if (entry.Key >= currentStage) // Key = unlock level
            {
                return entry.Value; // Value = character index
            }
        }
        return characterList.characters.Length; // If all are unlocked, return out-of-bounds
    }

    public void OnLevelCompleted()
    {
        int currentStage = gamePlayManager.GetCurrentStage();

        if (charToUnlock < characterList.characters.Length && 
            characterList.characters[charToUnlock].GetUnlockLevel() <= currentStage)
        {
            // Unlock the character
            characterList.characters[charToUnlock].UnlockCharacter();
            Debug.Log($"Unlocked {characterList.characters[charToUnlock].characterName}");

            // Move to next character unlock
            charToUnlock = FindNextUnlockableCharacter();
            PlayerPrefs.SetInt("NextCharacterToUnlock", charToUnlock);
            PlayerPrefs.Save();
        }
    }
}
