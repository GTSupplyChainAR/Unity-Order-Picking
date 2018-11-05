using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class NavigationController : MonoBehaviour {


    // data model;
    private string selectedUsername = "";
    private int selectedPhase = 0; // 0 indicates training, 1 indicates testing
    private int selectedPathId = 1;
    private int selectedBookNum = 0;
    private PathReader pr;
    private const string url = "http://eye";
    /* view style config
    private Color selected_color = Color.blue;
    private Color unselected_color = Color.white;*/

    // views
    GameObject userSelectionView;
    GameObject phaseSelectionView;
    GameObject pathIdSelectionView;
    GameObject bookInfoView;
    GameObject shelfView;
    GameObject completionView;

    // active view
    GameObject currentActiveView;

    // Use this for initialization
    void Start () {
        // data model init
        pr = new PathReader(Path.Combine(Application.streamingAssetsPath, "pick-paths.json"));
        pr.setPathId(selectedPathId);
        userSelectionView = GameObject.Find("User Selection View");
        userSelectionView.SetActive(true);
        phaseSelectionView = GameObject.Find("Phase Selection View");
        phaseSelectionView.SetActive(false);
        pathIdSelectionView = GameObject.Find("PathId Selection View");
        pathIdSelectionView.SetActive(false);
        bookInfoView = GameObject.Find("Book Info View");
        bookInfoView.SetActive(false);
        shelfView = GameObject.Find("Shelf View");
        shelfView.SetActive(false);
        completionView = GameObject.Find("Completion View");
        completionView.SetActive(false);
        currentActiveView = userSelectionView;


    }
    private void postdata() {
        /*WWWForm form = new WWWForm();
        form.AddField("userId", selectedUsername);
        form.AddField("userId", selectedUsername);
        form.AddField("userId", selectedUsername);
        
        StartCoroutine(Upload(form));*/
    }
    private IEnumerator Upload(WWWForm form) {
        var download = UnityWebRequest.Post(url, form);
        yield return download.SendWebRequest();
        if (download.isNetworkError || download.isHttpError)
        {

        }
        else {

        }
    }

    /*if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick2Button0)) && bookNum < pr.getNumberOfBooksInPath() - 1)
    {
    }*/
    private void userSelectionControl() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            userSelectionView.GetComponent<UserSelectionView>().selectNext();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            userSelectionView.GetComponent<UserSelectionView>().selectLast();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedUsername = userSelectionView.GetComponent<UserSelectionView>().getSelectedUser();
            currentActiveView.SetActive(false);
            phaseSelectionView.SetActive(true);
            // clear next selection
            selectedPhase = 0;
            phaseSelectionView.GetComponent<PhaseSelectionView>().setPhase(selectedPhase);
            currentActiveView = phaseSelectionView;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            // no action
        }
    }

    private void phaseSelectionControl()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            phaseSelectionView.GetComponent<PhaseSelectionView>().selectTesting();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            phaseSelectionView.GetComponent<PhaseSelectionView>().selectTraining();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedPhase = phaseSelectionView.GetComponent<PhaseSelectionView>().getSelectedPhase();
            currentActiveView.SetActive(false);
            pathIdSelectionView.SetActive(true);
            currentActiveView = pathIdSelectionView;
            // setup next selection
            pathIdSelectionView.GetComponent<PathIdSelectionView>().setPhase(selectedPhase);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // go back to user selection
            currentActiveView.SetActive(false);
            userSelectionView.SetActive(true);
            currentActiveView = userSelectionView;
        }
    }

    private void pathIdSelectionControl() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pathIdSelectionView.GetComponent<PathIdSelectionView>().selectNext();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pathIdSelectionView.GetComponent<PathIdSelectionView>().selectLast();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            selectedPathId = pathIdSelectionView.GetComponent<PathIdSelectionView>().getSelectedPathId();
            
            currentActiveView.SetActive(false);
            bookInfoView.SetActive(true);
            currentActiveView = bookInfoView;
            // setup the next view
            if (selectedPathId != pr.getPathId())
            {
                pr.setPathId(selectedPathId);
                selectedBookNum = 0;
            }
            bookInfoView.GetComponent<BookInfoView>().highlightBookInfo(pr.getBookWithLocation(selectedBookNum));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            currentActiveView.SetActive(false);
            phaseSelectionView.SetActive(true);
            currentActiveView = phaseSelectionView;

        }

    }
    private void bookInfoControl() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedBookNum + 1 < pr.getNumberOfBooksInPath())
            {
                selectedBookNum++;
                bookInfoView.GetComponent<BookInfoView>().highlightBookInfo(pr.getBookWithLocation(selectedBookNum));
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedBookNum > 0)
            {
                selectedBookNum--;
                bookInfoView.GetComponent<BookInfoView>().highlightBookInfo(pr.getBookWithLocation(selectedBookNum));
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            // switch to shelf view
            currentActiveView.SetActive(false);
            shelfView.SetActive(true);
            currentActiveView = shelfView;
            shelfView.GetComponent<ShelfView>().highlightBlock(pr.getBookWithLocation(selectedBookNum));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // get the book, send server data
            postdata();
            // go to next, or notify completion.
            currentActiveView.SetActive(false);
            completionView.SetActive(true);
            currentActiveView = completionView;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentActiveView.SetActive(false);
            pathIdSelectionView.SetActive(true);
            currentActiveView = pathIdSelectionView;
        }
    }
    private void completionControl() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedUsername = "";
            selectedPhase = 0; // 0 indicates training, 1 indicates testing
            selectedPathId = 1;
            selectedBookNum = 0;
            currentActiveView.SetActive(false);
            userSelectionView.SetActive(true);
            currentActiveView = userSelectionView;
        }
    }
    private void shelfControl() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedBookNum + 1 < pr.getNumberOfBooksInPath())
            {
                selectedBookNum++;
                shelfView.GetComponent<ShelfView>().highlightBlock(pr.getBookWithLocation(selectedBookNum));
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedBookNum > 0)
            {
                selectedBookNum--;
                shelfView.GetComponent<ShelfView>().highlightBlock(pr.getBookWithLocation(selectedBookNum));
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // switch to book info view
            currentActiveView.SetActive(false);
            bookInfoView.SetActive(true);
            currentActiveView = bookInfoView;
            bookInfoView.GetComponent<BookInfoView>().highlightBookInfo(pr.getBookWithLocation(selectedBookNum));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // get the book, send server data
            // go to next, or notify completion.
            currentActiveView.SetActive(false);
            completionView.SetActive(true);
            currentActiveView = completionView;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentActiveView.SetActive(false);
            pathIdSelectionView.SetActive(true);
            currentActiveView = pathIdSelectionView;
        }
    }
    // Update is called once per frame
    void Update () {
        if (currentActiveView == userSelectionView)
        {
            userSelectionControl();
        }
        else if (currentActiveView == phaseSelectionView)
        {
            phaseSelectionControl();
        }
        else if (currentActiveView == pathIdSelectionView)
        {
            pathIdSelectionControl();
        }
        else if (currentActiveView == bookInfoView)
        {
            bookInfoControl();
        }
        else if (currentActiveView == shelfView)
        {
            shelfControl();
        }
        else if (currentActiveView == completionView)
        {
            completionControl();
        }
    }
}
