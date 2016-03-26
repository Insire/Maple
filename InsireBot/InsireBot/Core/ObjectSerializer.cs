using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace InsireBot
{
    public static class ObjectSerializer
    {
        public static void SaveCollection<T>(ObservableCollection<T> Items, ISettings settings)
        {
            if (Items.Count > 0)
            {
                var path = GetPath(settings);

                var s = new XmlSerializer(typeof(ObservableCollection<T>));
                var writer = new StreamWriter(path);
                s.Serialize(writer, Items);
                writer.Close();
            }
        }

        public static void Save<T>(T Items, ISettings settings)
        {
            var path = GetPath(settings);

            var s = new XmlSerializer(typeof(T));
            var writer = new StreamWriter(path);
            s.Serialize(writer, Items);
            writer.Close();
        }

        public static ObservableCollection<T> LoadCollection<T>(ISettings settings)
        {
            var result = new ObservableCollection<T>();
            var path = GetPath(settings);

            if (File.Exists(path))
            {
                var deserializer = new XmlSerializer(typeof(ObservableCollection<T>));
                var textReader = new StreamReader(path);
                var item = (ObservableCollection<T>)deserializer.Deserialize(textReader);
                textReader.Close();

                if (item == null)
                    return result = item;
            }
            return result;
        }

        public static T Load<T>(ISettings settings) where T : new()
        {
            var result = new T();
            var path = GetPath(settings);

            if (File.Exists(path))
            {
                var deserializer = new XmlSerializer(typeof(T));
                var textReader = new StreamReader(path);
                var item = (T)deserializer.Deserialize(textReader);
                textReader.Close();

                if (item == null)
                    return result = item;
            }
            return result;
        }

        private static string GetPath(ISettings settings)
        {
            var result = Path.Combine(new[] { settings.Directory, settings.FileName, settings.Extension });

            return result;
        }
    }
}
