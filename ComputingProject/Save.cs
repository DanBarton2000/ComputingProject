using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject
{
    public class Save
    {
        public static void WriteXML(string filename) {
            XmlSerializer writer = new XmlSerializer(typeof(CelestialObject));

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + filename;

            FileStream stream = File.Create(path);

            writer.Serialize(stream, ObjectManager<IQuadtreeObject>.AllObjects);

            stream.Close();
        }
    }
}
