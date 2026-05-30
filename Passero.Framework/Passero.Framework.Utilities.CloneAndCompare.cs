using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;

namespace Passero.Framework
{
    public static partial class Utilities
    {

        public static T Clone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            // Usa FastCloner per creare una copia profonda
            //return (T)FastCloner.FastCloner.DeepClone(source);
            return (T)FastCloner.FastCloner.DeepClone(source);
        }


        public static T WisejClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            T newObject = Conversions.ToGenericParameter<T>(Activator.CreateInstance(source.GetType()));
            newObject = Wisej.Core.WisejSerializer.Parse(Wisej.Core.WisejSerializer.Serialize(source));
            return newObject;
        }



        public static T FastClonerClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            // Usa FastCloner per creare una copia profonda
            return (T)FastCloner.FastCloner.DeepClone(source);
        }

        public static T FastDeepClonerClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            // Usa FastDeepCloner per creare una copia profonda
            return (T)FastDeepCloner.DeepCloner.Clone(source);
        }


        public static T JsonClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;
            T newObject = Conversions.ToGenericParameter<T>(Activator.CreateInstance(source.GetType()));
            var deserializeSettings = new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
                //, ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //, PreserveReferencesHandling = PreserveReferencesHandling.All 
            };


            newObject = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
            return newObject;
        }


        public static bool ObjectsEquals(object obj, object another)
        {
            if (ReferenceEquals(obj, another))
                return true;
            if (obj is null || another is null)
                return false;
            if (obj.GetType() != another.GetType())
                return false;
            string objJson = JsonConvert.SerializeObject(obj);
            string anotherJson = JsonConvert.SerializeObject(another);
            return (objJson ?? "") == (anotherJson ?? "");
        }

    }
}