using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class Character : ScriptableObject
{
    public String characterName;
    public GameObject characterModel;
    
    [SerializeField] private bool isLocked;
    [SerializeField] private int unlockAtLevel; //character will be unlocked at this level
    

    public bool GetIsLock()
    {
        return isLocked;
    }
    

    public int GetUnlockLevel()
    {
        return unlockAtLevel;
    }
    

    public void UnlockCharacter()
    {
        isLocked = false;
    }
}
