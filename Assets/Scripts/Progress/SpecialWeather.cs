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

    public string GetSpecialWeatherComment() {
        string comments = "    - ";
        if (conditions != null) {
            for (int i = 0; i < conditions.Length; i++) {
                if (conditions[i] == SpecialWeatherCondition.Thunderstorm) {
                    if (i != conditions.Length - 1) {
                        comments += "Thunderstorm, ";
                    }
                    else if (i == conditions.Length - 1) {
                        comments += "Thunderstorm decreased productivity.\n";
                    }
                }
                if (conditions[i] == SpecialWeatherCondition.VeryHot) {
                    if (i != conditions.Length - 1) {
                        comments += "Very hot weather, ";
                    }
                    else if (i == conditions.Length - 1) {
                        comments += "Very hot weather decreased productivity.\n";
                    }
                }
                if (conditions[i] == SpecialWeatherCondition.Rain) {
                    if (i != conditions.Length - 1) {
                        comments += "Rain, ";
                    }
                    else if (i == conditions.Length - 1) {
                        comments += "Rain decreased productivity.\n";
                    }
                }
            }
        } else {
            comments = "";
        }

        return comments;
    }
    public float GetSpecialWeatherModifier() {
        float modifier = 1f;
        if (conditions != null) {
            for (int i = 0; i < conditions.Length; i++) {
                if (conditions[i] == SpecialWeatherCondition.Thunderstorm) {
                    modifier -= 0.02f;
                }
                if (conditions[i] == SpecialWeatherCondition.VeryHot) {
                    modifier -= 0.1f;
                }
                if (conditions[i] == SpecialWeatherCondition.Rain) {
                    modifier -= 0.15f;
                }
            }
        }
        return modifier;
    }
}
