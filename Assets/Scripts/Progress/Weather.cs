public class Weather{

    public float temperature;
    public float humidity;
    public SpecialWeather specialWeather;

    public Weather(float temperature, float humidity, SpecialWeather specialWeather) {
        this.temperature = temperature;
        this.humidity = humidity;
        this.specialWeather = specialWeather;
    }

    public override string ToString() {
        string str =
            "<b>Temperature:</b> " + temperature + "ºC\n" +
            "<b>Humidity:</b> " + humidity + "%\n";
        str += specialWeather.ToString();
        return str;
    }

    // calculation things
    public float GetWeatherModifier() {
        return 1f;
    }

    private float GetTemperatureModifier() {
        return 1f;
    }

    private float GetHumidityModifier() {
        return 1f;
    }

    private float GetSpecialModifier() {
        return 1f;
    }
}
