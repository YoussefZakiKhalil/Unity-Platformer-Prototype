using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CUAS.MMT;

public class DesignerController : MonoBehaviour
{

    [SerializeField] BallDesign[] designs;
    public BallDesign current = null;

    int index = -1; //active index for a design (fixxme: use an iterator instead!)
    bool checkBallorRingButton = true;
    [SerializeField]
    GameObject scrollView;

    //[SerializeField]
    public TMP_Text designName;

    //[SerializeField]
    public GameObject bdIObject; //object with ball design interface (e.g.: player)

    [SerializeField]
    public Material[] materials;

    GameObject previewObject = null;

    Camera previewCamera = null; //camera for material preview

    [SerializeField]
    RenderTexture previewTexture; //temp. storage for rendering

    public GameObject startCanvas;
    public GameObject designCanvas;

    public Button ballButton;
    public Button ringButton;

   

    public void OnEnable()
    {

        Debug.Log("On Enable");
        startCanvas.SetActive(false);
        designCanvas.SetActive(true);
    }


    public void OnDisable()
    {
        Debug.Log("On Disable");

        startCanvas?.SetActive(true);
        designCanvas?.SetActive(false);
        bdIObject.GetComponent<CUAS.MMT.IBallDesign>().ApplyBallDesign(current);
    }


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("DesignerController: start");

        GameObject tmp = new GameObject();
        tmp.name = "PreviewCamera";

        previewCamera = tmp.AddComponent<Camera>();

        previewCamera.orthographic = true;
        previewCamera.transform.localPosition = new Vector3(0, 1.5f, -1);
        previewCamera.orthographicSize = 0.5f;
        previewCamera.targetTexture = previewTexture;
        previewCamera.cullingMask = (1 << LayerMask.NameToLayer("Preview"));

        //https://docs.unity3d.com/ScriptReference/Camera-orthographicSize.html

        current = StatusController.Instance.GetBallDesign();

        designName.text = "none";

        previewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        previewObject.layer = LayerMask.NameToLayer("Preview");
        previewObject.name = "PreviewObject";

        previewObject.transform.localPosition = new Vector3(0, 1.5f, 0);
        previewObject.transform.localRotation = Quaternion.Euler(0, 180f, 0);


        if (designs.Length > 0)
        {
            if (StatusController.Instance.GetBallDesign().ball == null)
            {

                index = 0;

                current.ball = designs[index].ball;
                current.rings = designs[index].rings;

                designName.text = designs[index].designName;


            }
            else
            {

                index = 0; //fixxxme: search to find the correct index

                current = StatusController.Instance.GetBallDesign();
                designName.text = "custom";

            }

            bdIObject.GetComponent<CUAS.MMT.IBallDesign>().ApplyBallDesign(current);

            if (materials.Length > 0)
            {
                float posY = -50;
                //populate the scrollview with materials
                for (int i = 0; i < materials.Length; i++)
                {

                    GameObject obj = new GameObject();

                    obj.AddComponent<RectTransform>();
                    obj.AddComponent<Image>();
                    obj.AddComponent<Button>();

                    obj.transform.SetParent(scrollView.transform.GetChild(0).transform.GetChild(0).transform, false);

                    float ext = Mathf.Min(scrollView.GetComponent<RectTransform>().sizeDelta.x, scrollView.GetComponent<RectTransform>().sizeDelta.y) / 2f;

                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(ext, ext);
                    obj.GetComponent<RectTransform>().localPosition = new Vector2(41.5f, posY);

                    //set callbacks for buttons
                    //https://forum.unity.com/threads/dynamically-create-button-listeners-with-parameter-passing.974346/
                    int i2 = i;
                    obj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        this.OnButtonClicked(i2);
                    });


                    //generate a preview image
                    //https://docs.unity3d.com/ScriptReference/Texture2D.ReadPixels.html

                    previewObject.GetComponent<MeshRenderer>().material = materials[i];

                    Texture2D t = new Texture2D((int)previewTexture.width, (int)previewTexture.height, TextureFormat.RGBA32, false);

                    RenderTexture.active = previewTexture;

                    previewCamera.Render(); //render it

                    t.ReadPixels(new Rect(0, 0, previewTexture.width, previewTexture.height), 0, 0);
                    t.Apply();
                    RenderTexture.active = null;

                    obj.GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, previewTexture.width, previewTexture.height), new Vector2(0.5f, 0.5f));
                    posY = posY - 80;
                }
            }

            StatusController.Instance.SetBallDesign(current);

            Destroy(previewObject);
            Destroy(previewCamera.transform.gameObject);

        } //available designs

    }


    void OnButtonClicked(int i)
    {

        Debug.Log("OnButtonClicked: " + i);


        if (checkBallorRingButton == true)
        {
            selectMaterialCommand command = new selectMaterialCommand(this, materials[i], checkBallorRingButton);
            CommandManager.Instance.AddCommand(command);
            bdIObject.GetComponent<MeshRenderer>().material = materials[i];
            current.ball = bdIObject.GetComponent<MeshRenderer>().material;

        }
        else
        {
            selectMaterialCommand command = new selectMaterialCommand(this, materials[i], checkBallorRingButton);
            CommandManager.Instance.AddCommand(command);
            for (int idx = 0; idx < bdIObject.transform.childCount; idx++)
            {
                bdIObject.transform.GetChild(idx).gameObject.GetComponent<MeshRenderer>().material = materials[i];
                current.rings = bdIObject.GetComponent<MeshRenderer>().material;
            }
        }

    }


    public void OnButtonLeft()
    {
        index--;
        Debug.Log("OnButtonLeft");
        if (index < 0)
            index = designs.Length - 1;


        current.ball = designs[index].ball;
        current.rings = designs[index].rings;
        designName.text = designs[index].designName;
        bdIObject.GetComponent<CUAS.MMT.IBallDesign>().ApplyBallDesign(current);
        CommandManager.Instance.Reset();
    }


    public void OnButtonReset()
    {
        Debug.Log("OnButtonReset");
        current.ball = designs[0].ball;
        current.rings = designs[0].rings;
        designName.text = designs[0].designName;
        bdIObject.GetComponent<CUAS.MMT.IBallDesign>().ApplyBallDesign(current);
        CommandManager.Instance.Reset();
    }


    public void OnButtonUndo()
    {
        CommandManager.Instance.Undo();
        Debug.Log("OnButtonUndo");
    }


    public void OnButtonRight()
    {
        index++;
        if (index >= designs.Length)
            index = 0;


        current.ball = designs[index].ball;
        current.rings = designs[index].rings;
        designName.text = designs[index].designName;
        bdIObject.GetComponent<CUAS.MMT.IBallDesign>().ApplyBallDesign(current);
        CommandManager.Instance.Reset();
    }


    public void OnButtonBall()
    {
        Debug.Log("OnButtonBall");
        ballButton.GetComponent<Image>().color = Color.grey;
        ringButton.GetComponent<Image>().color = Color.white;
        checkBallorRingButton = true;
    }


    public void OnButtonRing()
    {
        Debug.Log("OnButtonRing");
        checkBallorRingButton = false;
        ringButton.GetComponent<Image>().color = Color.grey;
        ballButton.GetComponent<Image>().color = Color.white;
    }

}
