using System;
using System.Collections.Generic;
using DirecLayer;
using DomainLayer.Models;

namespace InfrastructureLayer.Repository
{
    public class SettingsRepository
    {
        public bool UpdateSettings(List<ConfigModel> lAppSettings)
        {
            var output = false;
            try
            {
                AppConfig config = new AppConfig();
                foreach (var appSetting in lAppSettings)
                {
                    config.UpdateConfig("appSettings", appSetting.Code, appSetting.Value);
                }
                output = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return output;
        }
    }
}
