//#define STATIC_MOCK
//#define LOCALIZE_INPLACE


using DiscordRPC.Logging;
using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPet_Simulator.Windows.Interface;
using System.Threading;
using LinePutScript.Localization.WPF;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using LinePutScript.Dictionary;

namespace VPet.Plugin.DiscordRPC
{
    public class DiscordRPC : MainPlugin
    {
        public DiscordRpcClient client;
        public override string PluginName => "DiscordRPC";

        public DiscordRPC(IMainWindow mainwin) : base(mainwin)
        {
            InitLocalization();
        }

        public override void LoadPlugin()
        {
            InitRPC();
            // delegate ui event
            MW.Main.TimeHandle += Main_TimeHandle;
        }

        public override void EndGame()
        {
            MW.Main.TimeHandle -= Main_TimeHandle;
            client.Dispose();

#if LOCALIZE_INPLACE
            File.WriteAllText("RPC.lps", LocalizeCore.StoreTranslationListToLPS());
#endif

            base.EndGame();
        }

        private void Main_TimeHandle(VPet_Simulator.Core.Main main)
        {
            var state = QueryState();
            //main.Say(state);
            UpdatePresence();
        }

        /// <summary>
        /// query vpet state and work info, generate raw string
        /// </summary>
        private string QueryState()
        {
            // use enum type name + int repr. because VPet.Core has all "normal" misspelled!!!!!!!!
            var state = string.Format("WorkingState{0}", (int)MW.Main.State).Translate();
            _RichPresence.Assets.LargeImageKey = _ImageKey[(int)MW.Main.State];

            var work = "";
            if (MW.Main.State == VPet_Simulator.Core.Main.WorkingState.Work ||
                MW.Main.State == VPet_Simulator.Core.Main.WorkingState.Travel) {
                work = MW.Main.nowWork.NameTrans;
                _RichPresence.Assets.LargeImageKey = _WorkImageKey[(int)MW.Main.nowWork.Type];

                TimeSpan elapsed = DateTime.Now - MW.Main.WorkTimer.StartTime;
                _RichPresence.Timestamps = new Timestamps {
                    End = DateTime.UtcNow.AddSeconds((TimeSpan.FromMinutes(MW.Main.nowWork.Time) - elapsed).TotalSeconds)
                };
            }
            work = string.IsNullOrEmpty(work) ? "WorkDesc0".Translate() : work;

            var mood = string.Format("ModeType{0}", (int)MW.Main.Core.Save.CalMode()).Translate();

            _RichPresence.State = mood + work;
            _RichPresence.Details = MW.Main.Core.Save.Name + state;

            return string.Format("{0} {1} {2}", state, work, mood);
        }

        private void UpdatePresence()
        {
            client.SetPresence(_RichPresence);
        }

        private bool IsDiscordPresent() => Process.GetProcessesByName("Discord").Length > 0;

        private void InitRPC()
        {
            client = new DiscordRpcClient(_RPCID);
            client.Initialize();

            _RichPresence = new RichPresence() {
                Assets = new Assets() {
                    LargeImageKey = "vpet_icon_default",
                    LargeImageText = MW.Main.Core.Save.Name,
                },
            };
        }

        private void InitLocalization()
        {
#if LOCALIZE_INPLACE
            LocalizeCore.StoreTranslation = true;
#endif
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dir = Path.Combine(dir, "..\\lang\\");

            foreach (var locale in Directory.GetFiles(dir, "*.lps", SearchOption.TopDirectoryOnly)) {
                LocalizeCore.AddCulture(locale.Split('\\').Last().Split('.').First(), new LPS_D(File.ReadAllText(locale)));
            }

            LocalizeCore.LoadDefaultCulture();
        }

        private string _RPCID => "1143326257833070664";
        private RichPresence _RichPresence;
        private string[] _ImageKey = {
            "vpet_large_think",
            "vpet_large_work",
            "vpet_large_nap",
            "vpet_large_travel",
            "vpet_large_idle",
        };
        private string[] _WorkImageKey = {
            "vpet_large_work",
            "vpet_large_study",
            "vpet_large_game",
        };
    }
}


#if STATIC_MOCK
internal class Program
{
    static void Main(string[] args)
    {
        var client = new DiscordRpcClient(
            applicationID: "1143326257833070664", 
            logger: new ConsoleLogger(LogLevel.Warning, true));

        client.OnReady += (sender, e) =>
        {
            Console.WriteLine("Received Ready from user {0}", e.User.Username);
        };

        client.OnPresenceUpdate += (sender, e) =>
        {
            Console.WriteLine("Received Update! {0}", e.Presence);
        };

        client.Initialize();

        while (client != null && !Console.KeyAvailable) {
            client.SetPresence(new RichPresence() {
                Details = "写代码",
                State = "普通地工作",
                Assets = new Assets() {
                    LargeImageKey = "vpet_icon_default",
                    LargeImageText = "VPet"
                },
                Timestamps = new Timestamps() {
                    End = DateTime.Now.AddMinutes(10),
                }
            });

            Thread.Sleep(2000);

            Console.WriteLine("Press any key to terminate");
        }

        client.Dispose();
    }
}
#endif