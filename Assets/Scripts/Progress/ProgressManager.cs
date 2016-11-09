using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour {
    private GameManager gameManager;

    public GameObject hudCanvas;
    public GameObject winCanvas;
    public Text winText;
    public GameObject loseCanvas;
    public Text loseText;

    public Day[] days;
    public BComponent[] components;
    public GameObject dayInfo;
    public Text dayInfoText;

    // view UI
    public Slider slider;
    public GameObject dayNumberText;
    public GameObject changeDayButton;

    // plan UI
    public GameObject skilledLaborInput;
    public GameObject generalLaborInput;
    public GameObject startDayButton;
    public GameObject endDayButton;
    public GameObject nextDayButton;
    public GameObject endDayInfo;
    public Text endDayInfoText;

    public int displayedDay;
    public int actualDay;
    public int progressDay;

    public float interval;
    public ModifierManager modifierManager;

    public ArrayList componentsIn;
    public ArrayList componentsOut;

    private float sumExpectedIndicator = 0f;

	void Start () {
        InitializeDays();
        displayedDay = 1;
        UpdateTextWithDisplay();
        UpdateComponentWithDisplay();
        actualDay = 1;
        progressDay = 1;
        gameManager = GetterUtility.GetGameManager();
        modifierManager = new ModifierManager();

        skilledLaborInput.SetActive(false);
        generalLaborInput.SetActive(false);
        startDayButton.SetActive(false);
        endDayButton.SetActive(false);
        nextDayButton.SetActive(false);
        endDayInfo.SetActive(false);

        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);



        for (int i = 0; i <= 16; i++) {
            days[i].Calculation();
            sumExpectedIndicator += days[i].GetExpectedProgressIndicator();

        }

        // for calculating expected progress
//        float testSub = 0;
//        for (int i = 0; i <= 16; i++) {
//            int a = i + 1;
//            testSub += days[i].GetExpectedProgressIndicator();
//            float percent = testSub / sumExpectedIndicator;
//            percent *= 100f;
//            Debug.Log("Day " + a + ": " + percent);
//        }
    }

    public void SwitchToView() {
        UpdateDayWithSlider();
        dayNumberText.SetActive(true);

        skilledLaborInput.SetActive(false);
        generalLaborInput.SetActive(false);
        startDayButton.SetActive(false);
    }

    public void SwitchToPlan() {
        UpdateDayWithProgressDay();
        slider.gameObject.SetActive(false);
        dayNumberText.SetActive(false);
        changeDayButton.SetActive(false);
        endDayButton.SetActive(false);
        nextDayButton.SetActive(false);

        skilledLaborInput.SetActive(true);
        generalLaborInput.SetActive(true);
        startDayButton.SetActive(true);
    }

    public void ChangeProgressDay(int newProgressDay) {
        progressDay = newProgressDay;
    }

    public void StartDay() {
        // disable view UIs
        gameManager.DisableSwitchButton();

        // receive inputs
        days[actualDay - 1].actualSkilledLabor =
            System.Convert.ToInt32(skilledLaborInput.GetComponent<InputField>().text);

        days[actualDay - 1].actualGeneralLabor =
            System.Convert.ToInt32(generalLaborInput.GetComponent<InputField>().text);

        // do the calculation
        days[actualDay - 1].Calculation();

        float sumActualIndicator = 0f;
        for (int i = 0; i <= actualDay - 1; i++) {
            sumActualIndicator += days[i].GetActualProgressIndicator();
        }

        float actualProgress = 100f * sumActualIndicator / sumExpectedIndicator;
        days[actualDay - 1].actualProgress = actualProgress;

        // get progress day
        for (int i = 16; i >= 0; i--) {
            if (actualProgress >= days[i].expectedProgress) {
                progressDay = i + 1;
                break;
            }
        }
//        Debug.Log("sumExpectedIndicator: " + sumExpectedIndicator);
//        Debug.Log("sumActualIndicator: " + sumActualIndicator);
//        Debug.Log("actualProgress: " + actualProgress);
//        Debug.Log("progressDay: " + progressDay);

        // do the animation
        UpdateComponentWithProgressDay();
    }

    public void EndDay() {
        dayInfo.SetActive(false);

        // show the board, include suggestions
        endDayInfo.SetActive(true);
        endDayInfoText.text = days[actualDay - 1].GetEndOfDayString();

        // show NexDay Button
        endDayButton.SetActive(false);
        nextDayButton.SetActive(true);

    }

    public void NextDay() {
        if (progressDay >= 17) {
            Win();
        } else if (actualDay >= 30) {
            Lose();
        }
        // go to next day
        actualDay++;
        UpdateTextWithActualDay();

        endDayInfo.SetActive(false);
        nextDayButton.SetActive(false);

        dayInfo.SetActive(true);

        skilledLaborInput.SetActive(true);
        generalLaborInput.SetActive(true);
        startDayButton.SetActive(true);

        gameManager.EnableSwitchButton();

    }


    #region progress
    public void UpdateDayWithProgressDay() {
        UpdateTextWithActualDay();
        UpdateComponentWithProgressDay();
    }

    void UpdateTextWithActualDay() {
        dayInfoText.text = days[actualDay - 1].ToString();
    }

    void UpdateComponentWithProgressDay() {
        // refresh array lists
        componentsIn = new ArrayList();
        componentsOut = new ArrayList();

        foreach (BComponent component in components) {
            // add those who are not active but (displayedDay >= dayIn)
            if (
            progressDay >= component.componentInfo.scheduledDayIn
            && !component.gameObject.activeSelf
            && progressDay < component.componentInfo.scheduledDayOut
            ) {

                componentsIn.Add(component);
            }

            // remove those who are active but (displayedDay >= dayOut
            if (progressDay >= component.componentInfo.scheduledDayOut && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }

            // remove those who should not exist, used when rewind
            if (progressDay < component.componentInfo.scheduledDayIn && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }
        }

        StartCoroutine(ComponentsAnimationCoroutineForProgress());
    }

    IEnumerator ComponentsAnimationCoroutineForProgress() {
        skilledLaborInput.SetActive(false);
        generalLaborInput.SetActive(false);
        startDayButton.SetActive(false);

        foreach (BComponent inComp in componentsIn) {
            inComp.ComeIn();
            yield return new WaitForSeconds(interval);
        }

        foreach (BComponent outComp in componentsOut) {
            outComp.FlyOut();
            yield return new WaitForSeconds(interval);
        }

        endDayButton.SetActive(true);
        StopCoroutine("ComponentsAnimationCoroutineForProgress");
    }
#endregion

    IEnumerator ComponentsAnimationCoroutine() {
        slider.gameObject.SetActive(false);
        changeDayButton.gameObject.SetActive(false);

        foreach (BComponent inComp in componentsIn) {
            inComp.ComeIn();
            yield return new WaitForSeconds(interval);
        }

        foreach (BComponent outComp in componentsOut) {
            outComp.FlyOut();
            yield return new WaitForSeconds(interval);
        }

        slider.gameObject.SetActive(true);
        changeDayButton.gameObject.SetActive(true);
        StopCoroutine("ComponentsAnimationCoroutine");
    }

    #region display
    // called by "Change Day" button
    public void UpdateDayWithSlider() {
        displayedDay = (int) slider.value;
        UpdateTextWithDisplay();
        UpdateComponentWithDisplay();
    }

    void UpdateComponentWithDisplay() {
        // refresh array lists
        componentsIn = new ArrayList();
        componentsOut = new ArrayList();

        foreach (BComponent component in components) {
            // add those who are not active but (displayedDay >= dayIn)
            if (
            displayedDay >= component.componentInfo.scheduledDayIn
            && !component.gameObject.activeSelf
            && displayedDay < component.componentInfo.scheduledDayOut
            ) {

                componentsIn.Add(component);
            }

            // remove those who are active but (displayedDay >= dayOut
            if (displayedDay >= component.componentInfo.scheduledDayOut && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }

            // remove those who should not exist, used when rewind
            if (displayedDay < component.componentInfo.scheduledDayIn && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }
        }

        StartCoroutine(ComponentsAnimationCoroutine());
    }

    void UpdateTextWithDisplay() {
        dayInfoText.text = days[displayedDay - 1].ToString();
    }
    #endregion

    private void InitializeDays() {
        days = new Day[30];

        // declare variables, which will be reused
        float expectedProgress;
        string activityOnThatDay;
        int expectedSkilledLabor;
        int expectedGeneralLabor;
        SpecialWeatherCondition[] conditions;
        SpecialWeather specialWeather;
        Weather weather;

        #region day 1
        expectedProgress = 3.1f;
        activityOnThatDay = "Setting out for concrete walls, Prepare materials for formwork and rebars";
        expectedSkilledLabor = 1;
        expectedGeneralLabor = 2;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30f, 83.5f, specialWeather);

        days[0] = new Day(1, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 2
        expectedProgress = 7.7f;
        activityOnThatDay = "Prepare materials for formwork and rebars, Formwork";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 2;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.5f, 80.5f, specialWeather);

        days[1] = new Day(2, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 3
        expectedProgress = 11.6f;
        activityOnThatDay = "Formwork";
        expectedSkilledLabor = 1;
        expectedGeneralLabor = 2;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.7f, 78.5f, specialWeather);

        days[2] = new Day(3, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 4
        expectedProgress = 17.1f;
        activityOnThatDay = "Formwork, Rebar fixing";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 2;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.8f, 80.5f, specialWeather);

        days[3] = new Day(4, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 5
        expectedProgress = 23.5f;
        activityOnThatDay = "Rebar fixing, Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 3;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.4f, 83f, specialWeather);

        days[4] = new Day(5, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 6
        expectedProgress = 29.8f;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 3;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.5f, 74.5f, specialWeather);

        days[5] = new Day(6, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 7
        expectedProgress = 35.1f;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 2;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.2f, 73f, specialWeather);

        days[6] = new Day(7, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 8
        expectedProgress = 40.3f;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 2;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.1f, 73.5f, specialWeather);

        days[7] = new Day(8, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 9
        expectedProgress = 43.0f;
        activityOnThatDay = "Setting out for brick walls";
        expectedSkilledLabor = 1;
        expectedGeneralLabor = 1;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.2f, 70.5f, specialWeather);

        days[8] = new Day(9, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 10
        expectedProgress = 52.5f;
        activityOnThatDay = "Setting out for brick walls, Concreting for concrete walls, Concrete curing";
        expectedSkilledLabor = 3;
        expectedGeneralLabor = 4;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(32f, 67.5f, specialWeather);

        days[9] = new Day(10, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 11
        expectedProgress = 56.4f;
        activityOnThatDay = "Concrete curing, Remove formwork, Lifting and installation of concrete wall";
        expectedSkilledLabor = 1;
        expectedGeneralLabor = 3;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.9f, 76.5f, specialWeather);

        days[10] = new Day(11, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 12
        expectedProgress = 62.0f;
        activityOnThatDay = "Lifting and installation of concrete wall, Remove formwork";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 3;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.3f, 81.5f, specialWeather);

        days[11] = new Day(12, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 13
        expectedProgress = 67.7f;
        activityOnThatDay = "Lifting and installation of concrete wall, Remove formwork";
        expectedSkilledLabor = 2;
        expectedGeneralLabor = 3;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.4f, 78.5f, specialWeather);

        days[12] = new Day(13, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 14
        expectedProgress = 75.3f;
        activityOnThatDay = "Lifting and installation of concrete wall, Window & door installation";
        expectedSkilledLabor = 3;
        expectedGeneralLabor = 4;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.8f, 73.5f, specialWeather);

        days[13] = new Day(14, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 15
        expectedProgress = 85.7f;
        activityOnThatDay = "Lifting and installation of concrete wall, Window & door installation, Brick wall fixing";
        expectedSkilledLabor = 3;
        expectedGeneralLabor = 5;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.2f, 81.5f, specialWeather);

        days[14] = new Day(15, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 16
        expectedProgress = 96.3f;
        activityOnThatDay = "Brick wall fixing";
        expectedSkilledLabor = 3;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.1f, 77.5f, specialWeather);

        days[15] = new Day(16, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 17
        expectedProgress = 100;
        activityOnThatDay = "Brick wall fixing";
        expectedSkilledLabor = 1;
        expectedGeneralLabor = 2;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.6f, 74.5f, specialWeather);

        days[16] = new Day(17, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 18
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.8f, 79f, specialWeather);

        days[17] = new Day(18, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 19
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.2f, 76.2f, specialWeather);

        days[18] = new Day(19, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 20
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.6f, 75.5f, specialWeather);

        days[19] = new Day(20, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 21
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(27.1f, 77.3f, specialWeather);

        days[20] = new Day(21, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 22
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(27.9f, 80.6f, specialWeather);

        days[21] = new Day(22, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 23
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.4f, 79.2f, specialWeather);

        days[22] = new Day(23, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 24
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(28.7f, 79.1f, specialWeather);

        days[23] = new Day(24, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 25
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.2f, 77.6f, specialWeather);

        days[24] = new Day(25, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 26
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.7f, 76.6f, specialWeather);

        days[25] = new Day(26, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 27
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.9f, 76.1f, specialWeather);

        days[26] = new Day(27, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 28
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.6f, 74.9f, specialWeather);

        days[27] = new Day(28, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 29
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.7f, 75.7f, specialWeather);

        days[28] = new Day(29, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

#region day 30
        expectedProgress = 100;
        activityOnThatDay = "No, all work should have been done";
        expectedSkilledLabor = 0;
        expectedGeneralLabor = 0;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.8f, 76.8f, specialWeather);

        days[29] = new Day(30, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion
    }


    public ModifierManager GetModifierManager() {
        return  modifierManager;
    }

    public void Win() {
        Time.timeScale = 0f;
        hudCanvas.SetActive(false);
        winCanvas.SetActive(true);
        winText.text = GetWinReport();
    }

    public void Lose() {
        Time.timeScale = 0f;
        hudCanvas.SetActive(false);
        loseCanvas.SetActive(true);
        loseText.text = GetLoseReport();
    }

    public string GetWinReport() {
        string str = "";
        str += "The project was completed in <b>" + actualDay + " days</b>, which was ";
        if (actualDay > 17) {
            int dayDiff = actualDay - 17;
            str += "<b>" + dayDiff + " days</b> later than expected.\n\n";
        } else if (actualDay == 17) {
            str += "<b>on time</b>.\n\n";
        } else if (actualDay < 17) {
            int dayDiff = 17 - actualDay;
            str += "<b>" + dayDiff + " days</b> earlier than expected.\n\n";
        }

        str += "The <b>total labor cost</b> was: ";
        float totalLaborCost = 0f;
        float expectedTotalLaborCost = 0f;
        for (int i = 0; i <= actualDay - 1; i++) {
            totalLaborCost += days[i].actualCost;
        }

        for (int i = 0; i <= 16; i++) {
            expectedTotalLaborCost += days[i].expectedLaborCost;
        }

        str += totalLaborCost + "HKD, ";

        if (totalLaborCost > expectedTotalLaborCost) {
            float costDiff = totalLaborCost - expectedTotalLaborCost;
            str += "which was " + costDiff + "HKD <b>more</b> than expected (" + expectedTotalLaborCost + "HKD).\n\n";
        } else if (totalLaborCost == expectedTotalLaborCost) {
            str += "which was same as expected.\n\n";
        } else if (totalLaborCost < expectedTotalLaborCost) {
            float costDiff = expectedTotalLaborCost - totalLaborCost;
            str += "which was " + costDiff + "HKD <b>less</b> than expected (" + expectedTotalLaborCost + "HKD).\n\n";
        }

        float incentive = 0f;
        float incentivePerDay = 2000f;
        if (actualDay < 17) {
            incentive = (17 - actualDay) * incentivePerDay;
            str += "You also got " + incentive + "HKD as incentive for early completion.\n\n";
        } else if (actualDay > 17) {
            incentive = (actualDay - 17) * 10000f;
            str += "The liquidated damage for late completion is: " + incentive + "HKD.\n\n";
        }

        return str;
    }

    public string GetLoseReport() {
        string str = "";
        str += "The project was not completed in 30 days.\n\n";

        str += "The <b>total labor cost</b> was already: ";
        float totalLaborCost = 0f;
        for (int i = 0; i <= actualDay - 1; i++) {
            totalLaborCost += days[i].actualCost;
        }
        str += totalLaborCost + "HKD, ";

        str += "but the progress is only " + days[actualDay - 1].actualProgress + "%.\n\n";

        float incentive = 0f;
        if (actualDay > 17) {
            incentive = (actualDay - 17) * 10000f;
            str += "And you've already got a liquidated damange of " + incentive + "HKD.\n\n";
        }

        str += "\nYou were finally fired by the company.\n\n";
        return str;
    }
}
