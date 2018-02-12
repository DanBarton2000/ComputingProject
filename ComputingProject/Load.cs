using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject {

    public class Load {
        /// <summary>
        /// Reads XML from a path and returns the objects 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<CelestialObject> ReadXMLFromPath(string path) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CelestialObject>));

            List<CelestialObject> objects;
            try {
                using (FileStream stream = File.OpenRead(path)) {
                    objects = (List<CelestialObject>)serializer.Deserialize(stream);

                    // Print out each object
                    objects.ForEach(x => Console.WriteLine(x.ToString()));
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return null;
            }

            return objects;
        }

        /// <summary>
        /// Reads the XML directly and returns the objects
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static List<CelestialObject> ReadXML(string xml) {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CelestialObject>));
            List<CelestialObject> objects;
            try {
                using (var reader = new StringReader(xml)) {
                    objects = (List<CelestialObject>)serializer.Deserialize(reader);
                }

                // Print out each object
                objects.ForEach(x => Console.WriteLine(x.ToString()));
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return null;
            }

            return objects;
        }
    }
}
