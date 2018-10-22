using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    List<Card> currentDeck;
	
    // Use this for initialization
	void Start () {
        currentDeck = new List<Card>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public List<Card> GetDeck()
    {
        return currentDeck;
    }
}
