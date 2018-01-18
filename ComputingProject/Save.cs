﻿using System;
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
            XmlSerializer writer = new XmlSerializer(typeof(List<CelestialObject>));

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + filename + ".xml";

            Console.WriteLine("Path: " + path);

            FileStream stream = File.Create(path);

            List<CelestialObject> objects = new List<CelestialObject>();

            ObjectManager.AllObjects.ForEach(x => objects.Add(x as CelestialObject));

            objects.ForEach(x => Console.WriteLine(x.ToString()));

            writer.Serialize(stream, objects);

            stream.Close();
        }
    }
}
