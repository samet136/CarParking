using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Car Setting")]
    public GameObject[] Cars;
    public int NumberOfCar;
    int RemainingCarvalue;
    int ActiveCarIndex = 0;

    [Header("Canvas Setting")]
    public Sprite ParkingImage;
    public TextMeshProUGUI RemainingCar;
    public GameObject[] CarCanvasImage;
    public TextMeshProUGUI[] Texts;
    public GameObject[] Panels;
    public GameObject[] Buttons;



    [Header("Platform Setting")]
    public GameObject Platform_1;
    public GameObject Platform_2;
    bool IsThereReturn;

    public float[] RotationSpeed;

    [Header("Level Setting")]
    public int DiamondCount;
    public ParticleSystem CollisionEffect;
    public AudioSource[] Sounds;
    public bool MoveUp;

    void Start()
    {
        CheckDefaultValues();
        RemainingCarvalue = NumberOfCar;
        IsThereReturn = true;
        //RemainingCar.text = RemainingCarvalue.ToString();
        for (int i = 0; i < NumberOfCar; i++)
        {
            CarCanvasImage[i].SetActive(true);
        }


    }
    public void NewCarBring()
    {

        RemainingCarvalue--;
        if (ActiveCarIndex < NumberOfCar)
        {
            Cars[ActiveCarIndex].SetActive(true);
        }
        else
        {
            Win();
        }
        CarCanvasImage[ActiveCarIndex - 1].GetComponent<Image>().sprite = ParkingImage;

        //RemainingCar.text = RemainingCarvalue.ToString();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Cars[ActiveCarIndex].GetComponent<Car>().forward = true;
            ActiveCarIndex++;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Panels[0].SetActive(false);
            Panels[3].SetActive(true);

        }
        if (IsThereReturn)
        {
            Platform_1.transform.Rotate(new Vector3(0, 0, -RotationSpeed[0]), Space.Self);
            if (Platform_2!=null)
            {
                Platform_2.transform.Rotate(new Vector3(0, 0, RotationSpeed[1]), Space.Self);

            }
        }
     
    }

    // Bellek Yönetimi
    public void Lost()
    {
        Texts[6].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[7].text = SceneManager.GetActiveScene().name;
        Texts[8].text = (NumberOfCar - RemainingCarvalue).ToString();
        Texts[9].text = DiamondCount.ToString();
        Sounds[1].Play();
        Sounds[3].Play();
        Panels[1].SetActive(true);
        Panels[3].SetActive(false);
        Invoke("LostBringButton", 3f);
        IsThereReturn = false;


    }
    void Win()
    {
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + DiamondCount);
        Texts[2].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[3].text = SceneManager.GetActiveScene().name;
        Texts[4].text = (NumberOfCar - RemainingCarvalue).ToString();
        Texts[5].text = DiamondCount.ToString();
        Sounds[2].Play();
        Panels[2].SetActive(true);
        Panels[3].SetActive(false);

        Invoke("WinBringButton", 1f);

    }
    void LostBringButton()
    {
        Buttons[0].SetActive(true);
    }
    void WinBringButton()
    {
        Buttons[1].SetActive(true);
    }

    void CheckDefaultValues()
    {
        

        Texts[0].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[1].text = SceneManager.GetActiveScene().name;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

}
