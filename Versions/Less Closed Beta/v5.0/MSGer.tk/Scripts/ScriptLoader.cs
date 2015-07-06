using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSGer.tk
{
    public class ScriptLoader : IPackable
    {
        public string Path;
        public ScriptLoader(string path)
        {
            Path = path;
        }
        //public List<ScriptLoader> Scripts = new List<ScriptLoader>();
        /*public ScriptLoader()
        {
            if (!Directory.Exists("scripts"))
                Directory.CreateDirectory("scripts");
            string[] files = Directory.GetFiles("scripts");
            foreach(string file in files)
            { //TODO: Tárolja el a készítő(k) adatait és egyebeket akár attribútumokként, vagy hasonló
                Scripts.Add(new ScriptLoader(file));
                if (!Scripts.Last().Load())
                {
                    MessageBox.Show(Language.Translate(Language.StringID.ScriptError));
                    try
                    {
                        Scripts.Last().Unload();
                    }
                    catch
                    {
                    }
                }
            }
        }*/
        private AppDomain PluginAppDomain;
        private ScriptInAppDomain script
        {
            get
            {
                return PluginAppDomain.GetData(ScriptInAppDomain.DataName) as ScriptInAppDomain;
            }
            set
            {
                PluginAppDomain.SetData(ScriptInAppDomain.DataName, value);
            }
        }
        public bool Load()
        {
            if (!File.Exists(Path))
                return false;
            var pluginAppDomainSetup = new AppDomainSetup();
            pluginAppDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            PluginAppDomain = AppDomain.CreateDomain(System.IO.Path.GetFileName(Path), null, pluginAppDomainSetup);
            script = new ScriptInAppDomain(Path, PluginAppDomain);
            //PluginAppDomain.SetData(ScriptInAppDomain.DataName, script);
            PluginAppDomain.DoCallBack(script.LoadInAppDomain);
            //return (PluginAppDomain.GetData(ScriptInAppDomain.DataName) as ScriptInAppDomain).Success;
            if (!script.Success)
                AppDomain.Unload(PluginAppDomain); //2015.04.06.
            return script.Success;
        }
        public void Unload()
        {
            PluginAppDomain.DoCallBack(script.UnloadInAppDomain);
            AppDomain.Unload(PluginAppDomain);
            PluginAppDomain = null;
        }
        public CompilerErrorCollection JustCompile()
        {
            if (!File.Exists(Path) || PluginAppDomain != null)
                return null;
            var pluginAppDomainSetup = new AppDomainSetup();
            pluginAppDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            PluginAppDomain = AppDomain.CreateDomain(System.IO.Path.GetFileName(Path), null, pluginAppDomainSetup);
            script = new ScriptInAppDomain(Path, PluginAppDomain);
            PluginAppDomain.DoCallBack(script.JustCompileInAppDomain);
            var errors = script.CompilerErrors; //2015.04.10.
            AppDomain.Unload(PluginAppDomain);
            PluginAppDomain = null;
            return errors;
        }

        public bool LoadFromPack(string filename)
        {
            if (!this.Load())
            {
                MessageBox.Show(Language.Translate(Language.StringID.ScriptError));
                try
                {
                    this.Unload();
                }
                catch
                {
                }
                return false; //2015.05.24.
            }
            return true; //2015.05.24.
        }

        public void UnloadFromPack()
        {
            this.Unload();
        }
    }
}
