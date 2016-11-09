using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour {

    public Day[] days;
    public BComponent[] components;
    public Text dayInfoText;

    public Slider slider;
    public GameObject dayNumberText;
    public GameObject changeDayButton;

    public int displayedDay;
    public int actualDay;
    public int dayForTrackProgress;

    public float interval;

    public ArrayList componentsIn;
    public ArrayList componentsOut;

	void Start () {
        InitializeDays();
        displayedDay = 1;
        UpdateTextWithDisplay();
        UpdateComponentWithDisplay();
        actualDay = 1;
        dayForTrackProgress = 1;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            dayForTrackProgress++;
            UpdateDayWithDayForTrackProgress();
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            dayForTrackProgress--;
            UpdateDayWithDayForTrackProgress();
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            SwitchToView();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            SwitchToPlan();
        }
    }

    public void SwitchToView() {
        UpdateDayWithSlider();
        dayNumberText.SetActive(true);
    }

    public void SwitchToPlan() {
        UpdateDayWithDayForTrackProgress();
        slider.gameObject.SetActive(false);
        dayNumberText.SetActive(false);
        changeDayButton.SetActive(false);
    }

    public void UpdateDayForTrackProgress(int newDayForTrackProgress) {
        dayForTrackProgress = newDayForTrackProgress;
    }

    public void UpdateDayWithDayForTrackProgress() {
        UpdateTextWithDayForTrackProgress();
        UpdateComponentWithDayForTrackProgress();
    }

    void UpdateTextWithDayForTrackProgress() {
        dayInfoText.text = days[dayForTrackProgress - 1].ToString();
    }

    void UpdateComponentWithDayForTrackProgress() {
        // refresh array lists
        componentsIn = new ArrayList();
        componentsOut = new ArrayList();

        foreach (BComponent component in components) {
            // add those who are not active but (displayedDay >= dayIn)
            if (
            dayForTrackProgress >= component.componentInfo.scheduledDayIn
            && !component.gameObject.activeSelf
            && dayForTrackProgress < component.componentInfo.scheduledDayOut
            ) {

                componentsIn.Add(component);
            }

            // remove those who are active but (displayedDay >= dayOut
            if (dayForTrackProgress >= component.componentInfo.scheduledDayOut && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }

            // remove those who should not exist, used when rewind
            if (dayForTrackProgress < component.componentInfo.scheduledDayIn && component.gameObject.activeSelf) {
                componentsOut.Add(component);
            }
        }

        StartCoroutine(ComponentsAnimationCoroutine());
    }

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
        expectedProgress = 10;
        activityOnThatDay = "Setting out for concrete walls, Prepare materials for formwork and rebars";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 4;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30f, 83.5f, specialWeather);

        days[0] = new Day(1, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 2
        expectedProgress = 15;
        activityOnThatDay = "Prepare materials for formwork and rebars, Formwork";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 4;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.5f, 80.5f, specialWeather);

        days[1] = new Day(2, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 3
        expectedProgress = 20;
        activityOnThatDay = "Formwork";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.7f, 78.5f, specialWeather);

        days[2] = new Day(3, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 4
        expectedProgress = 25;
        activityOnThatDay = "Formwork, Rebar fixing";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.8f, 80.5f, specialWeather);

        days[3] = new Day(4, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 5
        expectedProgress = 30;
        activityOnThatDay = "Rebar fixing, Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.4f, 83f, specialWeather);

        days[4] = new Day(5, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 6
        expectedProgress = 35;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.5f, 74.5f, specialWeather);

        days[5] = new Day(6, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 7
        expectedProgress = 40;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.2f, 73f, specialWeather);

        days[6] = new Day(7, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 8
        expectedProgress = 45;
        activityOnThatDay = "Preparation of Gypsum Board & Painting";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Thunderstorm;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.1f, 73.5f, specialWeather);

        days[7] = new Day(8, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 9
        expectedProgress = 50;
        activityOnThatDay = "Setting out for brick walls";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.2f, 70.5f, specialWeather);

        days[8] = new Day(9, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 10
        expectedProgress = 55;
        activityOnThatDay = "Setting out for brick walls, Concreting for concrete walls, Concrete curing";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(32f, 67.5f, specialWeather);

        days[9] = new Day(10, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 11
        expectedProgress = 60;
        activityOnThatDay = "Concrete curing, Remove formwork, Lifting and installation of concrete wall";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(31.9f, 76.5f, specialWeather);

        days[10] = new Day(11, "Monday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 12
        expectedProgress = 65;
        activityOnThatDay = "Lifting and installation of concrete wall, Remove formwork";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.3f, 81.5f, specialWeather);

        days[11] = new Day(12, "Tuesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 13
        expectedProgress = 70;
        activityOnThatDay = "Lifting and installation of concrete wall, Remove formwork";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[1];
        conditions[0] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.4f, 78.5f, specialWeather);

        days[12] = new Day(13, "Wednesday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 14
        expectedProgress = 75;
        activityOnThatDay = "Lifting and installation of concrete wall, Window & door installation";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = new SpecialWeatherCondition[2];
        conditions[0] = SpecialWeatherCondition.VeryHot;
        conditions[1] = SpecialWeatherCondition.Rain;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(29.8f, 73.5f, specialWeather);

        days[13] = new Day(14, "Thursday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 15
        expectedProgress = 80;
        activityOnThatDay = "Lifting and installation of concrete wall, Window & door installation, Brick wall fixing";
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

        conditions = null;
        specialWeather = new SpecialWeather(conditions);
        weather = new Weather(30.2f, 81.5f, specialWeather);

        days[14] = new Day(15, "Friday", expectedProgress, activityOnThatDay, expectedSkilledLabor, expectedGeneralLabor, weather);
        #endregion

        #region day 16
        expectedProgress = 85;
        activityOnThatDay = "Brick wall fixing";
        expectedSkilledLabor = 5;
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
        expectedSkilledLabor = 5;
        expectedGeneralLabor = 5;

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
}
