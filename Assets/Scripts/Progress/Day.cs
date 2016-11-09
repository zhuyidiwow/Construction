public class Day{

    public int dayNo;
    public string weekDay;
    public float expectedProgress; // [0%, 100%]
    public float actualProgress;
    public string activities;

    public int expectedSkilledLabor;
    public int expectedGeneralLabor;

    public int actualSkilledLabor;
    public int actualGeneralLabor;

    public Weather weather;

    public float expectedLaborCost;
    public float actualCost; // labor cost
    public float expectedProgressIndicator;
    public float actualProgressIndicator;

    private float skilledLaborCost = 2028.2f;
    private float generalLaborCost = 945.7f;

    public ProgressManager progressManager;
    public ModifierManager modifierManager;

    public Day(int dayNo, string weekday, float expectedProgress, string activityOnThatDay,
            int expectedSkilledLabor, int expectedGeneralLabor, Weather weather) {
        this.dayNo = dayNo;
        this.weekDay = weekday;
        this.expectedProgress = expectedProgress;
        this.activities = activityOnThatDay;
        this.expectedSkilledLabor = expectedSkilledLabor;
        this.expectedGeneralLabor = expectedGeneralLabor;
        this.weather = weather;
        expectedLaborCost = skilledLaborCost * expectedSkilledLabor + generalLaborCost * expectedGeneralLabor;
    }

    private void DoEffectsOnModifiers() {
        int actualLabor = actualSkilledLabor + actualGeneralLabor;
        int expectedLabor = expectedSkilledLabor + expectedGeneralLabor;

        // change motivation modifier
        if (actualLabor > expectedLabor) {
            if (actualLabor - expectedLabor > 2) {
                modifierManager.motivationModifier -= (actualLabor - expectedLabor) * 0.02f;
                if (modifierManager.motivationModifier <= 0.5f) {
                    modifierManager.motivationModifier = 0.5f;
                }
            }
        }

        if (actualLabor < expectedLabor) {
            if (expectedLabor - actualLabor <= 2) {
                modifierManager.motivationModifier += (expectedLabor - actualLabor) * 0.03f;
            }
        }

        // change skill modifier
        float actualProportion = 0f;
        if (actualGeneralLabor != 0) {
            actualProportion = actualSkilledLabor/actualGeneralLabor;
        }
        float expectedProportion = 0f;
        if (expectedGeneralLabor != 0) {
            expectedProportion = expectedSkilledLabor/expectedGeneralLabor;
        }
        if (actualSkilledLabor > expectedSkilledLabor || actualProportion > expectedProportion) {
            modifierManager.skillfulnessModifier += 0.02f;
        }
    }

    public void Calculation() {
        CalculateExpectedProgressIndicator();
        CalculateActualProgressIndicator();

        actualCost = skilledLaborCost * actualSkilledLabor + generalLaborCost * actualGeneralLabor;
    }

    public float GetExpectedProgressIndicator() {
        return expectedProgressIndicator;
    }

    public float GetActualProgressIndicator() {
        return actualProgressIndicator;
    }

    private void CalculateExpectedProgressIndicator() {
        progressManager = GetterUtility.GetProgressManager();
        modifierManager = progressManager.GetModifierManager();
        expectedProgressIndicator =
            (expectedSkilledLabor * 1.5f + expectedGeneralLabor * 1f)
            * GetWeatherModifier();
    }

    private void CalculateActualProgressIndicator() {
        progressManager = GetterUtility.GetProgressManager();
        modifierManager = progressManager.GetModifierManager();
        actualProgressIndicator =
            (actualSkilledLabor * 1.5f + actualGeneralLabor * 1f)
            * GetMotivationModifier()
            * GetSkillfulnessModifier()
            * GetWorkAreaModifier()
            * GetWeatherModifier();
    }

    private float GetMotivationModifier() {
        return modifierManager.motivationModifier;
    }

    private float GetSkillfulnessModifier() {
        return modifierManager.skillfulnessModifier;
    }

    private float GetWorkAreaModifier() {
        float modifier = 1f;
        int actualLabor = actualSkilledLabor + actualGeneralLabor;
        int expectedLabor = expectedSkilledLabor + expectedGeneralLabor;

        if (actualLabor > expectedLabor) {
            if (actualLabor - expectedLabor > 2) {
                // decrease productivity to base when too crowded
                modifier = (float) expectedLabor / (float) actualLabor + 0.1f;
            }
        }
        return modifier;
    }

    private float GetWeatherModifier() {
        return weather.GetWeatherModifier();
    }

    public override string ToString() {
        string str =
            "<b>Day</b> " + dayNo + " (" + weekDay + ")\n" +
            "<b>Expected progress:</b> " + expectedProgress + "%\n" +
            "<b>Activities:</b> " + activities + "\n" +
            "<b>Expected skilled labor:</b> " + expectedSkilledLabor + "\n" +
            "<b>Expected general labor:</b> " + expectedGeneralLabor + "\n" +
            "<b>Expected labor cost:</b> " + expectedLaborCost + "HKD\n";
        str += weather.ToString();
        return str;
    }

    public string GetEndOfDayString() {
        string str =
        "<b>DAILY REPORT</b>\n" +
        "<b>Day</b> " + dayNo + " (" + weekDay + ")\n" +
        "<b>Expected progress:</b> " + expectedProgress + "%\n" +
        "<b>Expected activities:</b> " + activities + "\n" +
        "<b>Expected skilled labor:</b> " + expectedSkilledLabor + "\n" +
        "<b>Expected general labor:</b> " + expectedGeneralLabor + "\n" +
        "<b>Expected labor cost:</b> " + expectedLaborCost + "HKD\n\n" +
        "<b>Actual skilled labor:</b> " + actualSkilledLabor + "\n" +
        "<b>Actual general labor:</b> " + actualGeneralLabor + "\n" +
        "<b>Actual labor cost:</b> " + actualCost + "HKD\n";
        if (dayNo > 17) {
            str += "<b>Liquidated Damage:</b> 10000 HKD\n";
        }

        if (actualProgress != 0) {
            str += "<b>Actual progress:</b> " + actualProgress.ToString().Substring(0, 4) + "%\n";
        } else {
            str += "<b>Actual progress:</b> 0%\n";
        }

//        string test =
//        "<b>Expected progress indicator:</b> " + expectedProgressIndicator + "\n" +
//        "<b>Actual progress indicator:</b> " + actualProgressIndicator + "\n" +
//        "<b>Weather modifier:</b> " + GetWeatherModifier() + "\n" +
//        "<b>Motivation:</b> " + GetMotivationModifier() + "\n" +
//        "<b>Skillfulness:</b> " + GetSkillfulnessModifier() + "\n" +
//        "<b>Work area:</b> " + GetWorkAreaModifier() + "\n";
//        str += test;

        string comments = "\n<b>Comments:</b>\n";
        comments += GetLaborNumberComment();
        comments += GetSkillComment();
        comments += GetWeatherComment();
        if (comments == "\n<b>Comments:</b>\n") {
            comments += "    The project progressed normally.\n";
        }
        if (actualProgress >= expectedProgress) {
            comments += "    + You're ahead schedule! Good job!\n";
        }
        str += comments;

        // str += weather.ToString();
        DoEffectsOnModifiers();
        return str;
    }

    // comments
    private string GetLaborNumberComment() {
        string comments = "";

        int actualLabor = actualSkilledLabor + actualGeneralLabor;
        int expectedLabor = expectedSkilledLabor + expectedGeneralLabor;

        if (actualLabor > expectedLabor) {
            if (actualLabor - expectedLabor <= 2) {
                comments += "    + Proper overmanning accelerated project.\n";
            } else if (actualLabor - expectedLabor > 2) {
                comments += "    - Excessive overmanning casued congestion on site and wasted labor cost.\n";
                comments += "    - Excessive overmanning decreased motivation.\n";
            }
        }

        if (actualLabor < expectedLabor) {
            comments += "    - Undermanning may delay project.\n";
            if (expectedLabor - actualLabor <= 2) {
                comments += "    + Slight undermanning increased motivation.\n";
            }
        }

        return comments;
    }

    private string GetSkillComment() {
        string comments = "";
        float actualProportion = 0f;
        if (actualGeneralLabor != 0) {
            actualProportion = actualSkilledLabor/actualGeneralLabor;
        }
        float expectedProportion = 0f;
        if (expectedGeneralLabor != 0) {
            expectedProportion = expectedSkilledLabor/expectedGeneralLabor;
        }
        if (actualSkilledLabor > expectedSkilledLabor || actualProportion > expectedProportion) {
            comments = "    + Enough skilled labor increased overall skillfulness.\n";
        }
        return comments;
    }

    private string GetWeatherComment() {
        return weather.GetWeatherComment();
    }


}
