using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EnumTypes;
using EventLibrary;
using UnityEditor;

public class UIManager : Singleton<UIManager>
{
    [Header("Camera")]
    public CinemachineVirtualCamera vanCamera;
    public CinemachineVirtualCamera shutterCamera;

    [Header("Van")]
    public GameObject buttonUI;
    public GameObject ingamePlayerUI;
    public GameObject shutter;
    public Animator shutterAnim;

    [Header("FirstIntro")]
    public GameObject loadingUI;
    public GameObject spaceToStart;

    [Header("OptionUI")]
    public GameObject optionSettingUI;
    public GameObject escButton;

    [Header("StopUI")]
    public GameObject stopUI;

    [Header("UnderBarUI")]
    public GameObject underBarCancle;
    public GameObject underBarStop;

    [Header("MaskTransitionUI")]
    public GameObject broccoliMask; // 크기를 변경할 RectTransform
    public GameObject pineappleMask; // 크기를 변경할 RectTransform

    [Header("Battle")]
    public GameObject battleRoomUI;
    public GameObject battleUI;
    public GameObject battleResultUI;
    public CustomReadyButtonScript battleUIReadyBtn;

    [Header("ExitLobbyUI")]
    public GameObject exitLobbyUI;

    [Header("Loading")]
    public GameObject[] loadingFoodArr;
    public GameObject loadingKeyUI;
    public Image loadingKeyBar;
    public GameObject loadingMapUI;
    public Image loadingMapBar;
    public GameObject[] mapImage;
    public GameObject[] mapText;

    [Header("BusMap")]
    public GameObject worldMapEscUI;
    public GameObject busTopUI;

    [Header("StageMap")]
    public GameObject stageMapEscUI;
    public GameObject ingameStageMapEscUI;
    public GameObject[] stageEscMapName;

    [Header("RecipeUI")]
    public GameObject recipeUI;
    public GameObject[] recipeArr;

    [Header("Resolution")]
    public TextMeshProUGUI resolutionText;
    public GameObject fullScreenButton;
    public GameObject fullScreenCheck;
    public bool windowScreen;
    private bool settingWindowScreen;
    public int resolutionArrNum;
    private int settingResolutionArrNum;
    public string[] resolutionTextArr
        = new string[] { "1280 x 720", "1680 x 1050", "1920 x 1080", "2560 x 1440", "3840 x 2160" };


    //private bool escUiOn = false;
    public bool first = true;
    public SceneType sceneType;
    public MapType mapType = MapType.None;

    public void JsonUILoad()
    {
        settingWindowScreen = LoadData.Instance.optionData.saveWindowMode;
        windowScreen = LoadData.Instance.optionData.saveWindowMode;
        resolutionArrNum = LoadData.Instance.optionData.saveResolutionNum;
        settingResolutionArrNum = LoadData.Instance.optionData.saveResolutionNum;

        SetResolution();
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
    }

    private void Start()
    {
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
    }

    private void Update()
    {
        //Debug.Log(sceneType);
        //Debug.Log(mapType);
        //BusMapEscUI();

        EscUI();
    }


    #region Popup

    // 옵션창 On
    public void SettingOn()
    {
        optionSettingUI.SetActive(true);
    }

    // 옵션창 Off
    public void SeetingOff()
    {
        optionSettingUI.SetActive(false);
    }

    // 그만두기창 On
    public void StopUIOn()
    {
        //escUiOn = true;
        stopUI.SetActive(true);
    }

    // 그만두기창 Off
    public void StopUIOff()
    {
        //escUiOn = false;
        stopUI.SetActive(false);

        // BusMapEscUI도 같이 종료
        //if (SceneManager.GetActiveScene().name == "WorldMap")
        //{
        worldMapEscUI.SetActive(false);
        //}

        // stageMapEscUI도 같이 종료
        stageMapEscUI.SetActive(false);
    }

    public void BattleRoomUION()
    {
        battleRoomUI.SetActive(true);
    }

    public void BattleRoomUIOFF()
    {
        battleRoomUI.SetActive(false);
    }

    public void ExitLobbyUIOn()
    {
        //escUiOn = true;
        exitLobbyUI.SetActive(true);
    }

    public void ExitLobbyUIOff() //CancleExitLobby
    {
        //escUiOn = false;
        exitLobbyUI.SetActive(false);
    }

    public void StageMapEscUIOff() // StageEscUICancle
    {
        stageMapEscUI.SetActive(false);
    }

    public void EscUIStopOn()
    {
        stopUI.SetActive(true);
    }

    public void EscUIStopOff()
    {
        stopUI.SetActive(false);
    }

    public void worldMapEscUICancle() //BusMapEscUICancle
    {
        worldMapEscUI.SetActive(false);
        //escUiOn = false;
    }

    #endregion

    #region Option UI

    // 볼륨 네모칸 UI
    public void SetSoundSquares(float volumeBGM, GameObject[] BGMSquares)
    {
        int j = 0;
        for (float i = 0; i < 1; i += 0.1f)
        {
            if (i < volumeBGM) //켜있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(false);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(true);
            }
            else //꺼있는거
            {
                BGMSquares[j].transform.GetChild(0).gameObject.SetActive(true);
                BGMSquares[j].transform.GetChild(1).gameObject.SetActive(false);
            }
            j++;
        }
    }

    // 해상도 오른쪽 화살표 버튼클릭
    public void ResolutionRightButton()
    {
        resolutionArrNum = (resolutionArrNum + 1) % 5;
        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    // 해상도 왼쪽 화살표 버튼클릭
    public void ResolutionLeftButton()
    {
        if (resolutionArrNum == 0) resolutionArrNum = 4;
        else
            resolutionArrNum = (resolutionArrNum - 1) % 5;

        resolutionText.text = resolutionTextArr[resolutionArrNum];
    }

    // 창모드 버튼 클릭
    public void OnClickFullScreenButton()
    {
        windowScreen = !windowScreen;
        fullScreenCheck.SetActive(windowScreen);
    }

    // 해상도 저장
    public void SaveResolution()
    {
        settingResolutionArrNum = resolutionArrNum;
        settingWindowScreen = windowScreen;

        //LoadData.Instance.SaveOptionDataToJson();

        SetResolution();
    }

    // 해상도 적용
    public void SetResolution()
    {
        switch (resolutionArrNum)
        {
            case 0:
                Screen.SetResolution(1280, 720, !windowScreen);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, !windowScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, !windowScreen);
                break;
            case 3:
                Screen.SetResolution(2560, 1440, !windowScreen);
                break;
            case 4:
                Screen.SetResolution(3840, 2160, !windowScreen);
                break;
        }
    }

    // 옵션 취소시
    public void CancleResolution()
    {
        // 원래 설정값으로 변경
        resolutionArrNum = settingResolutionArrNum;
        windowScreen = settingWindowScreen;

        // 적용
        resolutionText.text = resolutionTextArr[resolutionArrNum];
        fullScreenCheck.SetActive(windowScreen);
    }

    #endregion


    #region Mask InOut Tool

    // 마스크 축소 UI
    public void MaskInUI(GameObject inMask, string goTo)
    {
        SoundManager.Instance.ScreenInUI();
        inMask.SetActive(true);

        RectTransform inMaskRect = inMask.GetComponent<RectTransform>();
        float MaskInTime = 0;

        // 마스크 RECT 크게 설정
        if (inMask == broccoliMask)
        {
            inMaskRect.sizeDelta = new Vector2(4300, 4300);
            MaskInTime = 0.3f;
        }
        else if (inMask == pineappleMask)
        {
            inMaskRect.sizeDelta = new Vector2(7300, 7300);
            MaskInTime = 0.5f;
        }

        StartCoroutine(MaskInOut(inMaskRect, Vector2.zero, MaskInTime, () =>
        {
            LoadingFood();
            switch (goTo)
            {
                case "BattleUISet":
                    Invoke(goTo, 1F);
                    break;
                //case "BattleUIOff":
                //    Invoke(goTo, 1F);
                //    break;
                case "LoadingKeyUIOn":
                    Invoke(goTo, 1f);
                    break;
                case "LoadingKeyUIONBattle":
                    Invoke(goTo, 1f);
                    break;
                case "EnterBusMapMaskOut":
                    Invoke(goTo, 1f);
                    break;
                case "EnterIntroLoadingMaskOut":
                    busTopUI.SetActive(false);
                    Invoke(goTo, 1f);
                    break;
                case "LoadingMapUIOn":
                    busTopUI.SetActive(false);
                    Invoke(goTo, 1f);
                    break;
                case "EnterIntroMaskOut":
                    Invoke(goTo, 1f);
                    break;
                case "EnterStageMaskOut":
                    Invoke(goTo, 1f);
                    break;
            }
        }));
    }

    // 마스크 확대 UI
    public void MaskOutUI(GameObject inMask, GameObject outMask, string goTo)
    {
        SoundManager.Instance.ScreenOutUI();
        inMask.SetActive(false);
        outMask.SetActive(true);

        RectTransform outMaskRect = outMask.GetComponent<RectTransform>();
        outMaskRect.sizeDelta = new Vector2(0, 0);

        Vector2 targetRect = Vector2.zero;
        float MaskOutTime = 0;

        // 마스크 타겟 RECT 설정
        if (inMask == broccoliMask)
        {
            targetRect = new Vector2(4300, 4300);
            MaskOutTime = 0.3f;
        }
        else if (inMask == pineappleMask)
        {
            targetRect = new Vector2(7300, 7300);
            MaskOutTime = 0.5f;
        }

        // 씬 이동이 없을시 LoadingFoodOff 조금 느리게 꺼지게
        if (goTo == "") Invoke("LoadingFoodOff", 0.5f);
        else LoadingFoodOff();

        StartCoroutine(MaskInOut(outMaskRect, targetRect, MaskOutTime, () =>
        {
            outMask.SetActive(false);

            switch (goTo)
            {
                case "GoToBusMap":
                    EventManager<SceneChangeEvent>.TriggerEvent(SceneChangeEvent.WorldMapOpen);
                    break;
                case "GoToIntroMap":
                    EventManager<SceneChangeEvent>.TriggerEvent(SceneChangeEvent.IntroMapOpen);
                    break;
                case "GoToBattleRoom":
                    EventManager<SceneChangeEvent>.TriggerEvent(SceneChangeEvent.BattleRoomOpen);
                    break;
                case "GoToTestStage":
                    //mapType = MapType.Tuto;
                    EventManager<SceneChangeEvent>.TriggerEvent(SceneChangeEvent.TestStageMapOpen);
                    break;
                case "GoToMine":
                    EventManager<SceneChangeEvent>.TriggerEvent(SceneChangeEvent.stageMineMapOpen);
                    break;
                case "busTopUIOn":
                    busTopUI.SetActive(true);
                    break;

                default:
                    break;
            }
        }));
    }

    IEnumerator MaskInOut(RectTransform rt, Vector2 toSize, float time, System.Action onComplete)
    {
        Vector2 fromSize = rt.sizeDelta;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            rt.sizeDelta = Vector2.Lerp(fromSize, toSize, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rt.sizeDelta = toSize; // 최종 크기 설정
        onComplete?.Invoke(); // 완료 시 콜백 호출
    }

    #endregion

    #region BattleUI Mask

    // 배틀로비 들어가기 전 LodingUI MaskIn
    public void EnterLoadingKeyUIBattle()
    {
        MaskInUI(broccoliMask, "LoadingKeyUIONBattle");
    }

    // 배틀로비 들어가기전 LoadingUI MaskOut
    public void LoadingKeyUIONBattle()
    {
        buttonUI.SetActive(false);
        battleRoomUI.SetActive(false);
        stopUI.SetActive(false);
        stageMapEscUI.SetActive(false);
        escButton.SetActive(false);
        loadingKeyUI.SetActive(true);
        RecipeUIOff();
        MaskOutUI(broccoliMask, pineappleMask, "GoToBattleRoom");
    }


    // 배틀로비 MaskIn
    public void BattleUIOn()
    {
        MaskInUI(broccoliMask, "BattleUISet");
    }

    // 배틀로비 MaskOut
    public void BattleUISet()
    {
        loadingKeyBar.fillAmount = 0;
        loadingKeyUI.SetActive(false);
        battleUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, "");
    }
    #endregion

    #region Intro Mask
    // 인트로 들어가기 전 LoadingUI MaskIn
    public void EnterIntroLoadingMaskIn()
    {
        MaskInUI(broccoliMask, "EnterIntroLoadingMaskOut");
    }

    // 인트로 들어가기 전 LoadingUI MaskOut
    public void EnterIntroLoadingMaskOut()
    {
        loadingKeyUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, "GoToIntroMap");
    }

    // 인트로씬 MaskIn
    public void EnterIntroMaskIn()
    {
        VanSingleton.Instance.van.SetActive(true);
        busTopUI.SetActive(false);
        buttonUI.SetActive(true);
        exitLobbyUI.SetActive(false);
        battleUI.SetActive(false);
        shutterCamera.Priority = 9;
        MaskInUI(pineappleMask, "EnterIntroMaskOut");
    }

    // 인트로씬 MaskOut
    public void EnterIntroMaskOut()
    {
        loadingKeyBar.fillAmount = 0f;
        loadingKeyUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, "");
    }

    #endregion


    #region WorldMap Mask
    public void EnterLoadingKeyUI()
    {
        MaskInUI(broccoliMask, "LoadingKeyUIOn");
    }

    public void LoadingKeyUIOn()
    {
        loadingKeyUI.SetActive(true);
        MaskOutUI(broccoliMask, pineappleMask, "GoToBusMap");
        //MaskOutUI(broccoliMask, pineappleMask, "");
    }

    public void EnterBusMapMaskIn()
    {
        MaskInUI(pineappleMask, "EnterBusMapMaskOut");
    }

    public void EnterBusMapMaskOut()
    {
        loadingKeyBar.fillAmount = 0f;
        loadingKeyUI.SetActive(false);
        MaskOutUI(pineappleMask, broccoliMask, "busTopUIOn");
    }

    #endregion


    #region EnterStage

    public void EnterLoadingMapUI()
    {
        MaskInUI(pineappleMask, "LoadingMapUIOn");
    }

    public void LoadingMapUIOn()
    {
        loadingMapUI.SetActive(true);
        battleUI.SetActive(false);
        switch (mapType)
        {
            case MapType.Tuto:
                SetStageImageText(0);
                MaskOutUI(pineappleMask, broccoliMask, "GoToTestStage");
                break;
            case MapType.stage1_4:
                SetStageImageText(1);
                MaskOutUI(pineappleMask, broccoliMask, "GoToTestStage");
                break;
            case MapType.stage2_5:
                SetStageImageText(2);
                MaskOutUI(pineappleMask, broccoliMask, "GoToTestStage");
                break;
            case MapType.stage3_3:
                SetStageImageText(3);
                MaskOutUI(pineappleMask, broccoliMask, "GoToTestStage");
                break;
            case MapType.stageWizard:
                SetStageImageText(4);
                MaskOutUI(pineappleMask, broccoliMask, "GoToTestStage");
                break;
            case MapType.stageMine:
                SetStageImageText(5);
                MaskOutUI(pineappleMask, broccoliMask, "GoToMine");
                break;
        }
    }

    public void SetStageImageText(int arrNum)
    {
        mapImage[arrNum].SetActive(true);
        mapText[arrNum].SetActive(true);
        stageEscMapName[arrNum].SetActive(true);
    }

    public void EnterStageMaskIn()
    {
        MaskInUI(broccoliMask, "EnterStageMaskOut");
    }

    public void EnterStageMaskOut()
    {
        loadingMapBar.fillAmount = 0f;
        loadingMapUI.SetActive(false);
        MaskOutUI(broccoliMask, pineappleMask, "");
    }
    #endregion

    #region EscUI

    public void EscUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (escUiOn == false)
            //{
            //escUiOn = true;
            SoundManager.Instance.ButtonTick();
            switch (sceneType)
            {
                case SceneType.WorldMap:
                    worldMapEscUI.SetActive(true);                 
                    break;
                case SceneType.BattleMap:
                case SceneType.StageMap:
                    stageMapEscUI.SetActive(true);
                    break;
                case SceneType.Intro:
                    StopUIOn();
                    break;
                case SceneType.BattleLobby:
                    ExitLobbyUIOn();
                    break;
                case SceneType.NetworkBattleMap:
                    // GameScene Canvas문제로 분기처리.
                    //if (GameObject.Find("Ingame Esc Stage UI") != null)
                    //{
                    // 현재 오브젝트의 활성화 상태를 체크
                    ingameStageMapEscUI = FindInactiveObjectByName("Ingame Esc Stage UI");
                    bool isUIActive = ingameStageMapEscUI.activeSelf;
                    ingameStageMapEscUI.SetActive(!isUIActive);
                    //}
                    break;
            }
        }
    }

    GameObject FindInactiveObjectByName(string name)
    {
        Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.hideFlags == HideFlags.None && t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void EscUIButton()
    {
        SoundManager.Instance.ButtonTick();
        if (sceneType == SceneType.WorldMap)
        {
            worldMapEscUI.SetActive(true); 
        }

        if (sceneType == SceneType.BattleMap || sceneType == SceneType.StageMap)
        {
            stageMapEscUI.SetActive(true);
        }
    }

    public void ExitGame()
    {
        mapType = MapType.None;
        for (int i = 0; i < mapImage.Length; i++)
        {
            mapImage[i].SetActive(false);
            mapText[i].SetActive(false);
            stageEscMapName[i].SetActive(false);
        }
        if (sceneType == SceneType.WorldMap)
        {
            EscUIStopOff();
            worldMapEscUICancle();
            EnterIntroLoadingMaskIn();
        }
        else if(sceneType == SceneType.BattleLobby)
        {
            EnterIntroLoadingMaskIn();
        }
        else if (sceneType == SceneType.BattleMap)
        {
            EnterLoadingKeyUIBattle();
        }
        else if (sceneType == SceneType.StageMap)
        {
            EscUIStopOff();
            StageMapEscUIOff();
            EnterLoadingKeyUI();
            RecipeUIOff();
        }
        else if (sceneType == SceneType.Intro)
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                                                                Application.Quit();
            #endif
        }
    }

    #endregion


    #region LoadingFood
    public void LoadingFood()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                loadingFoodArr[0].SetActive(true);
                loadingFoodArr[1].SetActive(false);
                loadingFoodArr[2].SetActive(false);
                break;
            case 1:
                loadingFoodArr[0].SetActive(false);
                loadingFoodArr[1].SetActive(true);
                loadingFoodArr[2].SetActive(false);
                break;
            case 2:
                loadingFoodArr[0].SetActive(false);
                loadingFoodArr[1].SetActive(false);
                loadingFoodArr[2].SetActive(true);
                break;
        }
    }

    public void LoadingFoodOff()
    {
        for (int i = 0; i < loadingFoodArr.Length; i++)
            loadingFoodArr[i].SetActive(false);
    }

    #endregion

    #region RecipeUI
    public void RecipeUIOn(int arr)
    {
        recipeUI.SetActive(true);
        recipeArr[arr].SetActive(true);
        StartCoroutine("IRecipeUIPopIn");
    }

    IEnumerator IRecipeUIPopIn()
    {
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.RecipeUIPopIn();
    }

    public void RecipeUIOff()
    {
        recipeUI.SetActive(false);
        foreach (var recipe in recipeArr)
            recipe.SetActive(false);
    }
    #endregion 


}