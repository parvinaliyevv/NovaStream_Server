namespace NovaStream.Application.Services;

public static class RandomExtension
{
    public static List<T> Shuffle<T>(this Random random, List<T> values)
    {
        var newShuffledList = new List<T>();

        var listcCount = values.Count;

        for (int i = 0; i < listcCount; i++)
        {
            var randomElementInList = random.Next(0, values.Count);
            newShuffledList.Add(values[randomElementInList]);
            values.Remove(values[randomElementInList]);
        }

        return newShuffledList;
    }
}
