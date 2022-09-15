using System;
using System.Configuration;
using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.CeleStats {
    public class CeleStatsModuleSettings : EverestModuleSettings {
        #region CodeStats API Key
        public delegate void ApiKeyChangedHandler(string apiKey);
        public event ApiKeyChangedHandler ApiKeyChangedEvent;
        private string _apiKey = "";
        private string _apiUrl = "https://codestats.net/api/my/pulses";
        [SettingMaxLength(int.MaxValue)]
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                ApiKeyChangedEvent?.Invoke(value);
                _apiKey = value;
            }
        }

        #endregion

        #region CodeStats API URL
        public delegate void ApiUrlChangedHandler(string apiKey);
        public event ApiUrlChangedHandler ApiUrlChangedEvent;
        [SettingMaxLength(int.MaxValue)]
        public string ApiUrl
        {
            get => _apiUrl;
            set
            {
                ApiUrlChangedEvent?.Invoke(value);
                _apiUrl = value;
            } 
        }
        #endregion
        
        #region KeyBindings

        [DefaultButtonBinding(Buttons.TouchPadEXT, Keys.U)]
        public ButtonBinding ButtonTest { get; set; }

        #endregion
    }
}
