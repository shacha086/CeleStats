using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.CeleStats {
    public class CeleStatsModule : EverestModule {
        public static CeleStatsModule Instance { get; private set; }

        public override Type SettingsType => typeof(CeleStatsModuleSettings);
        public static CeleStatsModuleSettings Settings => (CeleStatsModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(CeleStatsModuleSession);
        public static CeleStatsModuleSession Session => (CeleStatsModuleSession) Instance._Session;

        public CeleStatsModule() {
            Instance = this;
        }

        public override void Unload()
        {
            Settings.ApiUrlChangedEvent -= ReloadHooks;
            UnloadAllHooks();
        }


        public override void Load() {
            Settings.ApiUrlChangedEvent += ReloadHooks;
            ReloadHooks();
        }

        private static void ReloadHooks(string _)
        {
            ReloadHooks();
        }
        
        private static void ReloadHooks()
        {
            if (Settings.ApiKey == string.Empty || Settings.ApiKey.Length < 60)
            {
                UnloadAllHooks();
                return;
            }
            LoadAllHooks();
        }

        private static void UnloadAllHooks()
        {
            On.Celeste.Player.Update -= HookPlayerUpdate;
            Everest.Events.Level.OnExit -= OnLevelExit;
            Everest.Events.Level.OnComplete -= LevelOnOnComplete;
            Everest.Events.Player.OnDie -= OnPlayerDie;
        }
        
        private static void LoadAllHooks()
        {
            On.Celeste.Player.Update += HookPlayerUpdate;
            Everest.Events.Level.OnExit += OnLevelExit; 
            Everest.Events.Level.OnComplete += LevelOnOnComplete;
            Everest.Events.Player.OnDie += OnPlayerDie;
        }

        private static void LevelOnOnComplete(Level level)
        {
            ExpManager.Instance.AddExp(500);
            ExpManager.Instance.CommitAll();
        }

        private static void OnPlayerDie(Player player)
        {
            ExpManager.Instance.AddExp(50);
        }

        private static void OnLevelExit(Level level, LevelExit exit, LevelExit.Mode mode, Session session, HiresSnow snow)
        {
            ExpManager.Instance.CommitAll();
        }

        private static void HookPlayerUpdate(On.Celeste.Player.orig_Update orig, Player self) {
            if (self.Scene.OnInterval(1f))
            {
                ExpManager.Instance.AddExp();
            }

            if (self.Scene.OnInterval(30f))
            {
                ExpManager.Instance.CommitAll();
            }
            
            if (Input.Dash.Pressed)
            {
                ExpManager.Instance.AddExp(5);
            }

            if (Input.Jump.Pressed)
            {
                ExpManager.Instance.AddExp(5);
            }

            if (Input.Grab.Repeating)
            {
                ExpManager.Instance.AddExp();
            }
            
            orig(self);
        }
    }
}