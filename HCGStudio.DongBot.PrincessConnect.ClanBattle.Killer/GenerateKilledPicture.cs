using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace HCGStudio.DongBot.PrincessConnect.ClanBattle.Killer
{
    public static class GenerateKilledPicture
    {
        public static async Task GenerateKilledPictureAsync(string year, string month, byte[] source, byte[] avatar, string battleName, string killedName,
            string clanName, string killReason, Stream outPutStream)
        {
            using var font = new FontFamily("方正兰亭黑简体");
            using var factory = new ImageFactory();
            factory.Load(source)
                .Watermark(new TextLayer
                {
                    Text = year,
                    FontFamily = font,
                    FontSize = 24,
                    FontColor = Color.Black,
                    Position = new Point(217, 273)
                })
                .Watermark(new TextLayer
                {
                    Text = month,
                    FontFamily = font,
                    FontSize = 24,
                    FontColor = Color.Black,
                    Position = new Point(340, 273)
                })
                .Watermark(new TextLayer
                {
                    Text = battleName,
                    FontFamily = font,
                    FontSize = 24,
                    FontColor = Color.Black,
                    Position = new Point(427, 273)
                })
                .Watermark(new TextLayer
                {
                    Text = clanName,
                    FontFamily = font,
                    FontSize = 20,
                    FontColor = Color.Black,
                    Position = new Point(180, 323)
                })
                .Watermark(new TextLayer
                {
                    Text = killedName,
                    FontFamily = font,
                    FontSize = 24,
                    FontColor = Color.White,
                    DropShadow = true,
                    Position = new Point(112, 229)
                })
                .Watermark(new TextLayer
                {
                    Text = killReason,
                    FontFamily = font,
                    FontSize = 24,
                    FontColor = Color.Black,
                    Position = new Point(107, 400)
                });
            if (avatar != null)
            {
                await using var ms = new MemoryStream();
                using var avatarImg = new ImageFactory();
                avatarImg.Load(avatar)
                    .Resize(new Size(120,120))
                    .Format(new BitmapFormat())
                    .Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var srcImage = new Bitmap(ms);
                var dstImage = new Bitmap(srcImage.Width, srcImage.Height,PixelFormat.Format64bppArgb);
                var g = Graphics.FromImage(dstImage);
                using var br = new SolidBrush(Color.Transparent);
                g.FillRectangle(br, 0, 0, dstImage.Width, dstImage.Height);

                var path = new GraphicsPath();
                path.AddEllipse(0, 0, dstImage.Width, dstImage.Height);
                g.SetClip(path);
                g.DrawImage(srcImage, 0, 0);

                factory.Overlay(new ImageLayer {Image = dstImage, Position = new Point(60, 90)});
            }

            factory.Format(new JpegFormat())
                .Save(outPutStream);
        }
    }
}
