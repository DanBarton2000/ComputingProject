using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject {

    public class Load {
        public static List<IQuadtreeObject> ReadXML(string filename) {
            /*try {
                XmlSerializer reader = new XmlSerializer(typeof(CelestialObject));

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + filename + ".xml";
                StreamReader file = new StreamReader(path);
                Console.WriteLine(path);
                //List<CelestialObject> objects = (List<CelestialObject>)reader.Deserialize(file);
                //return objects;
                return null;

            }
            catch(FileNotFoundException) {
                Console.WriteLine("File not found!");
                return null;
            }*/
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + filename + ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(CelestialObject));

            List<IQuadtreeObject> objects;

            using (FileStream stream = File.OpenRead(path)) {

                objects = (List<IQuadtreeObject>)serializer.Deserialize(stream);

                objects.ForEach(x => Console.WriteLine(x.ToString()));
            }

            return objects;
        }
    }
}
