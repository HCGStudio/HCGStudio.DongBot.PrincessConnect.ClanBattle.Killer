using System.IO;
using System.Threading.Tasks;
using HCGStudio.DongBot.PrincessConnect.ClanBattle.Killer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClanBattleKillerUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            await using var stream = new FileStream("result.jpg", FileMode.Create);
            await GenerateKilledPicture.GenerateKilledPictureAsync("2020", "7",
                await File.ReadAllBytesAsync("plain.jpg"),
                await File.ReadAllBytesAsync("avatar.jpg"), "双子",
                "兎田佩克拉", "DSY马家沟兔田工会", "公会头号内鬼", stream);
        }
    }
}