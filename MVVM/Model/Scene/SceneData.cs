using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.Model.Scene
{
    internal class SceneData
    {
        public string SceneName;
        public Dictionary<string, SceneSetting> SceneSettings;
        
    }

    internal class SceneSetting
    {
        public string SettingName;
        public bool IsEnabled;

        public SceneSetting(string settingName, bool isEnabled)
        {
            SettingName = settingName;
            IsEnabled = isEnabled;
        }
    }
}
