using RedCorners.Components;
using RedCorners.Forms;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedCorners.Forms.Ad.Demo.Systems
{
    public class SettingsSystem
    {
        public static SettingsSystem Instance { get; private set; } = new SettingsSystem();
        SettingsSystem() { }

        ObjectStorage<Settings> settings;

        bool isInitialized = false;
        public async Task InitializeAsync()
        {
            if (isInitialized) return;
            isInitialized = true;
            await Task.Run(() =>
            {
                settings = new ObjectStorage<Settings>();
                Settings.Instance = settings.Data;
            });

            Signals.SaveSettings.Subscribe(this, async () => await SaveAsync());
        }

        volatile bool isSaving = false;
        public async Task SaveAsync()
        {
            if (isSaving) return;
            isSaving = true;
            await Task.Run(() =>
            {
                Save();
            });
            isSaving = false;
        }

        public void Save()
        {
            if (settings == null)
                settings = new ObjectStorage<Settings>();

            settings.Data = Settings.Instance;
            settings.Save();
        }
    }
}

namespace RedCorners.Forms.Ad.Demo
{
    public partial class Settings
    {
        public static Settings Instance { get; set; }
    }
}