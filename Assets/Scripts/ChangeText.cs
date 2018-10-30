using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

public class ChangeText : MonoBehaviour
{
    private int pathId = 1;
    private int bookNum = 0;
    private PathReader pr;
    private GameObject bookText;
    private GameObject book;
    private int maxLineChars = 5;
    private String[] words;
    String result = "";
    private int charCount;
    private int shelfHighlightNumber;
    bool isBookInfoActive = true;
    bool isShelfViewActive = false;
    GameObject shelf;
    Dictionary<string, int> row;
    int highlight_row;

    // Use this for initialization
    void Start()
    {
        pr = new PathReader(Path.Combine(Application.streamingAssetsPath, "pick-paths.json"));
        pr.setPathId(pathId);
        book = GameObject.Find("Book Info Display");
        shelf = GameObject.Find("Shelf");
        bookText = GameObject.Find("Book Text");
        book.SetActive(isBookInfoActive);
        shelf.SetActive(isShelfViewActive);
        shelf.GetComponent<ChangeShelfBlock>().init();

        //create row
        row = new Dictionary<string, int>();
        row.Add("A", 0);
        row.Add("B", 1);
        row.Add("C", 2);
        row.Add("D", 3);
        row.Add("E", 4);
        row.Add("F", 5);

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow) && bookNum < pr.getNumberOfBooksInPath() - 1)
        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0)) && bookNum < pr.getNumberOfBooksInPath() - 1)
        {
            BookWithLocation bookInfo = pr.getBookWithLocation(bookNum);
            TextMesh bookInfoText = bookText.GetComponent<TextMesh>();
            bookInfoText.text = "Title: ";
            wrapText(bookInfo.book.title);
            bookInfoText.text = bookInfoText.text + "\nAuthor: ";
            wrapText(bookInfo.book.author);

            if (!isShelfViewActive)
            {
                shelf.SetActive(true);
                shelf.GetComponent<ChangeShelfBlock>().highlightBlock(bookInfo.book.tag);
                shelf.SetActive(false);
            } else {
                shelf.GetComponent<ChangeShelfBlock>().highlightBlock(bookInfo.book.tag);
            }
            if (!isBookInfoActive)
            {
                book.SetActive(true);
                changeShelfHighlight(bookInfo.book.tag);
                book.SetActive(false);
            }
            else {
                changeShelfHighlight(bookInfo.book.tag);
            }
            bookNum += 1;
        }
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        if ((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick2Button1)))
        {
            isBookInfoActive = !isBookInfoActive;
            isShelfViewActive = !isShelfViewActive;
            book.SetActive(isBookInfoActive);
            shelf.SetActive(isShelfViewActive);
        }


    }

    void changeShelfHighlight(String tag) {
        //Get rid of old highlight
        String objectID = "row_" + highlight_row + "_block";
        Sprite greenBlock = Resources.Load<Sprite>("green_block");
        GameObject.Find(objectID).GetComponent<SpriteRenderer>().sprite = greenBlock;
        //Highlight new block
        string[] loc = tag.Split('-');
        highlight_row = row[loc[3]]; 
        objectID = "row_" + highlight_row + "_block";
        Sprite redBlock = Resources.Load<Sprite>("red_block");
        GameObject.Find(objectID).GetComponent<SpriteRenderer>().sprite = redBlock;
        GameObject.Find("shelf_number_text").GetComponent<TextMesh>().text = loc[3];
    } 

    void wrapText(String text)
    {
        charCount = 0;
        words = text.Split(" "[0]);
        result = "";

        for (int index = 0; index < words.Length; index++)
        {

            var word = words[index].Trim();

            if (index == 0)
            {
                result = words[0];
            }

            if (index > 0)
            {
                charCount += word.Length + 1;
                if (charCount <= maxLineChars)
                {
                    result += " " + word;
                }
                else
                {
                    charCount = 0;
                    result += "\n " + word;
                }
            }
        }
        bookText.GetComponent<TextMesh>().text = bookText.GetComponent<TextMesh>().text + result;
    }
}
