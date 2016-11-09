public class SpecialWeather {

    public SpecialWeatherCondition[] conditions;

    public SpecialWeather(SpecialWeatherCondition[] conditions) {
        this.conditions = conditions;
    }


    public override string ToString() {
        string str = "<b>Special Weather:</b> ";
        if (conditions != null) {
            for (int i = 0; i < conditions.Length; i++) {
                if (conditions[i] == SpecialWeatherCondition.Thunderstorm) {
                    if (i != conditions.Length - 1) {
                        str += "Thunderstorm, ";
                    }
                    else if (i == conditions.Length - 1) {
                        str += "Thunderstorm\n";
                    }
                }
                if (conditions[i] == SpecialWeatherCondition.VeryHot) {
                    if (i != conditions.Length - 1) {
                        str += "Very hot, ";
                    }
                    else if (i == conditions.Length - 1) {
                        str += "Very hot\n";
                    }
                }
                if (conditions[i] == SpecialWeatherCondition.Rain) {
                    if (i != conditions.Length - 1) {
                        str += "Rain, ";
                    }
                    else if (i == conditions.Length - 1) {
                        str += "Rain\n";
                    }
                }
            }
        } else {
            str += "none\n";
        }
        return str;
    }

    public float GetSpecialWeatherModifier() {
        return 1f;
    }
}
