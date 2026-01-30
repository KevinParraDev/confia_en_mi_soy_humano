public static class ReadingTimeCalculatorHelper
{
    private const int WordsPerMinute = 250;
    private const int AverageCharactersWord = 5;

    public static float CalculateReadingTime(string text)
    {
        int characterCount = text.Length;

        float wordCount = characterCount / AverageCharactersWord;

        float timeInMinutes = wordCount / WordsPerMinute;

        return timeInMinutes * 60f;
    }
}
