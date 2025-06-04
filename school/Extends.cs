namespace Extends;

public static class Extends {
    
    public static float ArithmeticMean(this float[] origin) {
        float sum = origin.Sum();

        return (float)Math.Round(sum / origin.Length, 2);
    }

    public static float[] ToFloatArray(this string[] origin, Func<float, float> validation) {
        float[] result = {0, 0, 0}; 
        for(int i = 0; i < origin.Length; i++) {
             result[i] = float.Parse(origin[i]);
             validation(result[i]);
        }
        return result;
    }

}
