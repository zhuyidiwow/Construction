public class Day{

    public int dayNo;
    public string weekDay;
    public float expectedProgress; // [0%, 100%]
    public string activities;

    public int expectedSkilledLabor;
    public int expectedGeneralLabor;

    public float actualProgress;
    public int actualSkilledLabor;
    public int actualGeneralLabor;

    public Weather weather;

    public float expectedLaborCost;
    public float actualCost; // labor cost

    private float skilledLaborCost = 2028.2f;
    private float generalLaborCost = 945.7f;

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
}
