# 鲨内鬼图生成器

## 配置教程

这个插件可以生成鲨内鬼图，使用这个插件，除了将编译好的放入`Plugins`文件夹之外，还进行以下额外操作：

- 下载[`ImageProcessor`](https://www.nuget.org/packages/ImageProcessor/)，并将其dll文件一同放入插件文件夹（使用这个库按照作者的要求需要在推特上联系他）。

- 修改`Dong! Bot`的`.csproj`，在其中加入

  ```XML
  <PackageReference Include="System.Drawing.Common" Version="4.7.0" />
  ```

  然后重新编译`Dong! Bot`.

- 在`PluginConfig`文件夹中创建任意`JSON`文件，然后填入配置，以下为示例：

  ``` Json
  {
      "Pcr": 
      {
          "ClanBattle":
          {
              "KillBlackSheep":
              {
                  "PlainPath": "./res/plain.jpg",
                  "BattleYear" : 2020,
                  "BattleMonth" : 7,
                  "BattleName" : "双子"
              }
          }
      }
  }
  ```

- 将[原图](https://github.com/HCGStudio/HCGStudio.DongBot.PrincessConnect.ClanBattle.Killer/blob/master/ClanBattleKillerUnitTest/plain.jpg)放入你在`PlainPath`中指定的位置。

- 启动`Dong! Bot`。

## 使用方式：

在群内发送鲨内鬼+at需要鲨的人，即可自动生成图片。

## 图来源

最早能在搜索引擎中发现的帖子来源于NGA[**[无聊水]以后会长踢人可以送一个证书**](https://ngabbs.com/read.php?tid=22406220)，但是其右下角水印显示其来自于百度贴吧，如果有人是原作者或者知道来源请发ISSUE，这样就可以正确署名。

