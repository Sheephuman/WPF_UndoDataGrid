using UndoTransaction_SnapShot.MVVM.Model;

namespace UndoTransaction_SnapShot
{
    static class PersonCreater
    {
        internal static Person RandomPerson()
        {
            var random = new Random();
            string[] prefectures =
   {
            "東京", "大阪", "福岡", "北海道", "京都",
            "愛知", "沖縄", "広島", "宮城", "長野"
        };
            // 名前候補（「ひつじ○○」）
            string[] nameSuffix = { "太郎", "花子", "次郎", "美咲", "健一", "真央", "翔", "未来", "一郎", "優子" };



            var people = new List<Person>();

            var person = new Person
            {
                Name = "ひつじ" + nameSuffix[random.Next(nameSuffix.Length)],
                Age = random.Next(20, 41), // 20～40の範囲
                City = prefectures[random.Next(prefectures.Length)]
            };


            return person;

        }

    }
}
