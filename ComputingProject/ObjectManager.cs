using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject
{
    public class ObjectManager
    {
        #region Variables
        public static List<CelestialObject> allObjects = new List<CelestialObject>();
        #endregion

        #region Methods
        public static CelestialObject FindObjectWithName(string name) {
            CelestialObject co = allObjects.First(s => s.Name == name);
            return co;
        }
        #endregion
    }
}
