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

    // comments
    public string GetWeatherComment() {
        return specialWeather.GetSpecialWeatherComment();
    }

    // calculation things
    public float GetWeatherModifier() {
        float modifier = 1f;
        modifier *= GetTemperatureModifier();
        modifier *= GetHumidityModifier();
        modifier *= GetSpecialModifier();
        return modifier;
    }

    private float GetTemperatureModifier() {
        float modifier = 1f;
        modifier -= (temperature - 26f) * 0.01f;
        return modifier;
    }

    private float GetHumidityModifier() {
        float modifier = 1f;
        modifier -= (humidity - 75f) * 0.01f;
        return modifier;
    }

    private float GetSpecialModifier() {
        return specialWeather.GetSpecialWeatherModifier();
    }
}
