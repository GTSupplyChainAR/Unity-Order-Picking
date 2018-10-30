using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour {

    GameObject book;
    bool isBookInfoActive = true;
    bool isShelfViewActive = false;
    GameObject shelf;

    // Use this for initialization
    void Start () {
        book = GameObject.Find("Book Info Display");
        shelf = GameObject.Find("Shelf");
        book.SetActive(isBookInfoActive);
        shelf.SetActive(isShelfViewActive);

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isBookInfoActive = !isBookInfoActive;
            isShelfViewActive = !isShelfViewActive;
            book.SetActive(isBookInfoActive);
            shelf.SetActive(isShelfViewActive); 
        }
	}
}
