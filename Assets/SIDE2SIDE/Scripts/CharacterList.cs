using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Scriptable object made from this script will act as sort of a "database" for character list

[CreateAssetMenu]
public class CharacterList : ScriptableObject
{
    public GameObject[] characters;

    public int GetCharacterLength()
    {
        return characters.Length;
    }
    

    public GameObject GetCharacter(int characterIndex)
    {
        return characters[characterIndex];
    }
}
