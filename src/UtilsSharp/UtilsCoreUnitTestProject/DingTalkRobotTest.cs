using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsCore;

namespace UtilsCoreUnitTestProject
{
    [TestClass]
    public class DingTalkRobotTest
    {
        private string webhook = "请填写";
        private readonly List<string> _atMobiles;
        public DingTalkRobotTest()
        {
            _atMobiles = new List<string> {"156591xxxxx"};
        }


        [TestMethod]
        public void SendTextMessage()
        {
            var result=DingTalkRobot.SendTextMessage(webhook, "业务报警:钉钉测试", _atMobiles, false);
        }

        [TestMethod]
        public void SendLinkMessage()
        {
            var result = DingTalkRobot.SendLinkMessage(webhook, "业务报警:钉钉测试","钉钉测试", "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg","http://www.baidu.com/");
        }

        [TestMethod]
        public void SendMarkdownMessage()
        {
            var markdownMessage=new List<MarkdownMessage>();
            var msg = new MarkdownMessage
            {
                Index = 0,
                IsLineFeed = true,
                MarkdownType = MarkdownType.文本,
                Text = new Text()
                {
                    Content = "业务报警:钉钉测试",
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/",
                    ContentType = ContentType.加粗,
                    ContentGrade = TitleType.五级
                }
            };
            markdownMessage.Add(msg);
            var msg1 = new MarkdownMessage
            {
                Index = 0,
                IsLineFeed = true,
                MarkdownType = MarkdownType.图片,
                Text = new Text()
                {
                    Content = "业务报警:钉钉测试",
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/",
                    ContentType = ContentType.加粗,
                    ContentGrade = TitleType.一级
                }
            };
            markdownMessage.Add(msg1);
            MarkdownMessage msg2 = new MarkdownMessage
            {
                Index = 0,
                IsLineFeed = true,
                MarkdownType = MarkdownType.链接,
                Text = new Text()
                {
                    Content = "业务报警:钉钉测试",
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/",
                    ContentType = ContentType.加粗,
                    ContentGrade = TitleType.一级
                }
            };
            markdownMessage.Add(msg2);
            var result = DingTalkRobot.SendMarkdownMessage(webhook,"钉钉测试",TitleType.一级, markdownMessage, _atMobiles,true);
        }

        [TestMethod]
        public void SendActionCardMessage()
        {
            var markdownMessage = new List<MarkdownMessage>();
            var msg = new MarkdownMessage
            {
                Index = 0,
                IsLineFeed = true,
                MarkdownType = MarkdownType.图片,
                Text = new Text()
                {
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/"
                }
            };
            markdownMessage.Add(msg);
            var msg1 = new MarkdownMessage
            {
                Index = 1,
                IsLineFeed = true,
                MarkdownType = MarkdownType.文本,
                Text = new Text()
                {
                    Content = "乔布斯 20 年前想打造的苹果咖啡厅 ",
                    ContentType = ContentType.默认,
                    ContentGrade = TitleType.三级
                }
            };
            markdownMessage.Add(msg1);
            var msg2 = new MarkdownMessage
            {
                Index = 2,
                IsLineFeed = true,
                MarkdownType = MarkdownType.文本,
                Text = new Text()
                {
                    Content = "Apple Store 的设计正从原来满满的科技感走向生活化，而其生活化的走向其实可以追溯到 20 年前苹果一个建立咖啡馆的计划",
                    ContentType = ContentType.默认,
                    ContentGrade = TitleType.默认
                }
            };
            markdownMessage.Add(msg2);
            var result = DingTalkRobot.SendActionCardMessage(webhook, "业务报警:乔布斯 20 年前想打造一间苹果咖啡厅，而它正是 Apple Store 的前身", markdownMessage,0,0, "阅读全文", "http://www.baidu.com");
        }

        [TestMethod]
        public void SendSingleActionCardMessage()
        {
            var markdownMessage = new List<MarkdownMessage>();
            var msg = new MarkdownMessage
            {
                Index = 0,
                IsLineFeed = true,
                MarkdownType = MarkdownType.文本,
                Text = new Text()
                {
                    Content = "业务报警:乔布斯",
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/",
                    ContentType = ContentType.斜体,
                    ContentGrade = TitleType.四级
                }
            };
            markdownMessage.Add(msg);
            var msg1 = new MarkdownMessage
            {
                Index = 1,
                IsLineFeed = true,
                MarkdownType = MarkdownType.图片,
                Text = new Text()
                {
                    Content = "业务报警:钉钉测试",
                    ImgUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg",
                    Url = "http://www.baidu.com/",
                    ContentType = ContentType.斜体,
                    ContentGrade = TitleType.一级
                }
            };
            markdownMessage.Add(msg1);

            var btns=new List<Btn>();
            Btn btn = new Btn {ActionUrl = "http://www.baidu.com/", Title = "内容不错"};
            btns.Add(btn);
            Btn btn1 = new Btn {ActionUrl = "http://www.baidu.com/", Title = "不感兴趣"};
            btns.Add(btn1);
            var result = DingTalkRobot.SendSingleActionCardMessage(webhook,"我是标题", markdownMessage, 1, 1, btns);
        }


        [TestMethod]
        public void SendFeedCardMessage()
        {
            var links = new List<Link>
            {
                new Link()
                {
                    Title = "业务报警:时代的火车向前开",
                    MessageUrl = "http://www.baidu.com/",
                    PicUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg"
                },
                new Link()
                {
                    Title = "业务报警:时代的火车向前开2",
                    MessageUrl = "http://www.baidu.com/",
                    PicUrl = "http://zjj.hclygl.com/pc/002/index_files/7303aec2d90144ba8a5c724334518a14.jpg"
                }
            };
            var result = DingTalkRobot.SendFeedCardMessage(webhook, links);
        }
    }
}
