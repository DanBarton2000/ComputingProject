﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ComputingProject {

    public class Load {
        public static List<CelestialObject> ReadXML(string filename) {
            try {
                XmlSerializer reader = new XmlSerializer(typeof(CelestialObject));

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + filename;
                StreamReader file = new StreamReader(path);
                List<CelestialObject> objects = (List<CelestialObject>)reader.Deserialize(file);
                return objects;
            }
            catch(FileNotFoundException) {
                Console.WriteLine("File not found!");
                return null;
            }

        }
    }
}
