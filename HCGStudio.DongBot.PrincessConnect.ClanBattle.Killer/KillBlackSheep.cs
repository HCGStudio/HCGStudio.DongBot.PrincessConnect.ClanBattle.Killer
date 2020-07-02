using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HCGStudio.DongBot.Core.Attributes;
using HCGStudio.DongBot.Core.Messages;
using HCGStudio.DongBot.Core.Service;
using Microsoft.Extensions.Configuration;

namespace HCGStudio.DongBot.PrincessConnect.ClanBattle.Killer
{
    [Service("内鬼清除")]
    public class KillBlackSheep
    {
        public IConfiguration Configuration { get; set; }
        public IMessageProvider MessageProvider { get; set; }
        public IMessageSender MessageSender { get; set; }
        
        [OnKeyword("鲨内鬼",InvokePolicies = InvokePolicies.Group, KeywordPolicy = KeywordPolicy.Begin)]
        public async Task KillBlackSheepAsync(long groupId, long userId, Message message)
        {
            var plainPicturePath = Configuration["Pcr:ClanBattle:KillBlackSheep:PlainPath"];
            var battleYear = Configuration["Pcr:ClanBattle:KillBlackSheep:BattleYear"] ?? string.Empty;
            var battleMonth = Configuration["Pcr:ClanBattle:KillBlackSheep:BattleMonth"] ?? string.Empty;
            var battleName = Configuration["Pcr:ClanBattle:KillBlackSheep:BattleName"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(plainPicturePath) || !File.Exists(plainPicturePath))
            {
                await MessageSender.SendGroupAsync(groupId, 
                    new SimpleMessage("配置Pcr:ClanBattle:KillBlackSheep:PlainPath为空或者文件不存在！"));
                return;
            }

            if (message is IUnionMessage unionMessage)
            {
                var atUser = 0L;
                foreach (var mes in unionMessage.Messages)
                {
                    if (mes is IAtMessage atMessage && !atMessage.AtAll)
                    {
                        atUser = atMessage.Content;
                    }
                }

                if (atUser == 0)
                {
                    await MessageSender.SendGroupAsync(groupId,
                        new SimpleMessage("要鲨哪个内鬼？"));
                    return;
                }
                var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36 Edg/83.0.478.58");
                var response = await http.GetAsync($"http://q.qlogo.cn/headimg_dl?dst_uin={atUser}&spec=640");
                var avatar = response.Content.ReadAsByteArrayAsync();
                var plain = File.ReadAllBytesAsync(plainPicturePath);
                var name = MessageProvider.GetGroupUserNameAsync(groupId, atUser);
                var clanName = MessageProvider.GetGroupNameAsync(groupId);
                var fileName = Path.Combine(Environment.CurrentDirectory, "temp", $"{Guid.NewGuid()}.jpg");
                await using var stream = new FileStream(fileName, FileMode.Create);
                await GenerateKilledPicture.GenerateKilledPictureAsync(battleYear, battleMonth, await plain,
                    await avatar, battleName, await name,
                    await clanName, "成为头号内鬼", stream);
                await stream.FlushAsync();
                await MessageSender.SendGroupAsync(groupId,
                    new LocalPictureMessage(fileName));
            }
            else
            {
                await MessageSender.SendGroupAsync(groupId,
                    new SimpleMessage("要鲨哪个内鬼？"));
            }

        }
    }
}
