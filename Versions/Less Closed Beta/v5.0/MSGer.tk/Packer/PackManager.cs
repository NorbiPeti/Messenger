using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                foreach (string file in files)
                {
                    IPackable packable = Activator.CreateInstance(type, true) as IPackable;
                    PackableThings.Add(packable);
                    //packable.LoadFromPack(file);
                    if (!packable.LoadFromPack(file))
                        PackableThings.Remove(packable); //2015.05.24. - Ezzel elvileg előbb-utóbb törli
                }
            }
        }
        public static void UnloadAll()
        { //2015.04.11.
            foreach(var packable in PackableThings)
            {
                packable.UnloadFromPack();
            }
            PackableThings.Clear();
        }
        public static string GetName(Type type)
        { //2015.05.16.
            return type.Name.ToLower();
        }
    }
}
