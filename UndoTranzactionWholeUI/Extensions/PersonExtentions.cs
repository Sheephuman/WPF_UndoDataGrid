using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot.Extensions
{
    public static class PersonCollectionExtensions // 非ジェネリック静的クラス
    {
        public static void RemoveColections(this IList<Person> colentions, object? NewValue, List<Person> deltaValue)
        {
            foreach (Person token in deltaValue)
                colentions.Remove(token);

        }

        public static void AddColections(this IList<Person> colentions, object? NewValue, List<Person> deltaValue)
        {
            foreach (Person token in deltaValue)
                colentions.Add(token);
        }
    }
}
