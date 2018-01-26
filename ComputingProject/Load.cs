using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject {

    public class Load {
        public static List<CelestialObject> ReadXML(string filename) {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + filename + ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(List<CelestialObject>));

            List<CelestialObject> objects;

            using (FileStream stream = File.OpenRead(path)) {
                objects = (List<CelestialObject>)serializer.Deserialize(stream);

                // Print out each object
                objects.ForEach(x => Console.WriteLine(x.ToString()));
            }

            return objects;
        }
    }
}
