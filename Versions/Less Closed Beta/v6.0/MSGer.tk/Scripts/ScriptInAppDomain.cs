using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    [Serializable]
    public class ScriptInAppDomain
    {
        private AppDomain appdomain;
        private string path;
        public string Path
        {
            get
            {
                path = (appdomain.GetData(DataName) as ScriptInAppDomain).path;
                return path;
            }
            set
            {
                path = value;
                appdomain.SetData(DataName, this);
            }
        }
        private bool success;
        public bool Success
        {
            get
            {
                success = (appdomain.GetData(DataName) as ScriptInAppDomain).success;
                return success;
            }
            set
            {
                success = value;
                appdomain.SetData(DataName, this);
            }
        }
        [NonSerialized]
        private IScript script;
        private IScript Script
        {
            get
            {
                script = (appdomain.GetData(DataName) as ScriptInAppDomain).script;
                return script;
            }
            set
            {
                script = value;
                appdomain.SetData(DataName, this);
            }
        }
        private CompilerErrorCollection compilererrors;
        public CompilerErrorCollection CompilerErrors
        {
            get
            {
                compilererrors = (appdomain.GetData(DataName) as ScriptInAppDomain).compilererrors;
                return compilererrors;
            }
            set
            {
                compilererrors = value;
                appdomain.SetData(DataName, this);
            }
        }
        public ScriptInAppDomain(string path, AppDomain domain)
        {
            appdomain = domain; //Ez remélhetőleg megoldja, hogy mindig a jó AppDomain-ből szerezze az adatokat
            Path = path;
        }
        public void LoadInAppDomain()
        {
            /*var context = AppDomain.CurrentDomain.GetData(DataName) as ScriptInAppDomain;
            Console.WriteLine("Contexts equal: " + (context == this));*/
            //appdomain = AppDomain.CurrentDomain; //Ez remélhetőleg megoldja, hogy mindig a jó AppDomain-ből szerezze az adatokat
            string code = File.ReadAllText(Path);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            //parameters.ReferencedAssemblies.Add("");
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        parameters.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException)
                {
                    // this happens for dynamic assemblies, so just ignore it. 
                }
            }
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                //context.Success = false;
                //AppDomain.CurrentDomain.SetData(DataName, context);
                Success = false; //Automatikusan beállítja
                return;
            }
            else //A fordítás sikeres
            {
                Assembly asm = results.CompiledAssembly;
                foreach (var type in asm.GetTypes())
                {
                    if (type.IsClass && typeof(IScript).IsAssignableFrom(type))
                    {
                        var script = Activator.CreateInstance(type) as IScript;
                        if (script != null)
                        {
                            Script = script;
                            Script.Load(); //Lefuttatja a szkript Load() függvényét
                            break;
                        }
                    }
                }
                if (Script == null)
                    Success = false;
                else
                    Success = true;
                //AppDomain.CurrentDomain.SetData(DataName, context);
                return;
            }
        }
        public void UnloadInAppDomain()
        {
            Script.Unload();
        }
        public void JustCompileInAppDomain()
        {
            string code = File.ReadAllText(Path);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        parameters.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException)
                {
                    // this happens for dynamic assemblies, so just ignore it. 
                }
            }
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            CompilerErrors = results.Errors; //.Errors: 2015.04.10.
        }
        public const string DataName = "PluginContextData";
    }
}
