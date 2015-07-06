using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public static class PackManager
    {
        /*public static void Load<T>(T instance) where T : IPackable
        {
            File.ReadAllBytes(instance.FileName);
            instance.LoadFromPack();
        }*/
        private static List<IPackable> PackableThings = new List<IPackable>(); //2015.04.11.
        public static void LoadAll()
        { //2015.04.11.
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                //if (!type.IsAssignableFrom(typeof(IPackable)))
                if (!typeof(IPackable).IsAssignableFrom(type) || type.IsInterface)
                    continue;
                //PackableTypes.Add(Activator.CreateInstance(type) as IPackable); //Automatán initializálja az összes packelhető típust
                if (!Directory.Exists(GetName(type)))
                    //continue;
                    Directory.CreateDirectory(GetName(type)); //2015.05.16.
                string[] files = Directory.GetFiles(GetName(type));
                if (files.Length == 0)
                    DownloadDefaults(type); //2015.06.14.
                foreach (string file in files)
                {
                    IPackable packable = Activator.CreateInstance(type, true) as IPackable;
                    PackableThings.Add(packable);
                    if (typeof(IPackWithSave).IsAssignableFrom(type) && !type.IsInterface)
                        (packable as IPackWithSave).FileName = file; //2015.06.06.
                    //packable.LoadFromPack(file);
                    if (!packable.LoadFromPack(file))
                        PackableThings.Remove(packable); //2015.05.24. - Ezzel elvileg előbb-utóbb törli
                }
            }
        }

        private static void DownloadDefaults(Type type)
        { //2015.06.14.
#if not_for_now
            if (typeof(IPackWithDefaults).IsAssignableFrom(type) && !type.IsInterface)
            {
                IPackable packable = Activator.CreateInstance(type, true) as IPackable;
                WebClient client = new WebClient();
                client.DownloadFile(Networking.WebAddress + "/" + GetName(type) + ".npack", GetName(type) + "\\default.npack");
                string line;
                using (var sr = File.OpenText(GetName(type) + "\\default.npack"))
                {
                    line = sr.ReadLine();
                }
                if (line.StartsWith("<html>"))
                {
                    File.Delete(GetName(type) + "\\default.npack");
                    throw new WebException("Network error while downloading default pack for " + GetName(type) + ".\n" + line);
                }
                PackableThings.Add(packable);
            }
            */
#endif
            //TODO: Az összes IPackable-t *.npack-ként mentse, az elején egy azonosítóval (GetName)
        }
        public static void UnloadAll()
        { //2015.04.11.
            foreach (var packable in PackableThings)
            {
                packable.UnloadFromPack();
            }
            PackableThings.Clear();
        }
        private static string GetName(Type type)
        { //2015.05.16.
            return type.Name.ToLower();
        }
        public static T Add<T>(string name) where T : class, IPackable, IPackWithSave
        { //2015.06.06.
            var type = typeof(T);
            T packable = Activator.CreateInstance(type, true) as T;
            PackableThings.Add(packable);
            packable.AddPack(GetName(type) + Path.DirectorySeparatorChar + name);
            return packable;
        }
        public static void Save<T>(T packable) where T : class, IPackable, IPackWithSave
        {
            //packable.SavePack(GetName(typeof(T)) + Path.DirectorySeparatorChar + packable.FileName);
            packable.SavePack(packable.FileName);
        }

        internal static void Remove<T>(T packable) where T : class, IPackable, IPackWithSave
        { //2015.06.14.
            File.Delete(packable.FileName); //2015.06.14.
        }
    }
}
