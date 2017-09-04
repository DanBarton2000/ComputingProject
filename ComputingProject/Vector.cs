using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputingProject
{
    public class Vector
    {
        #region Variables
        public double x { get; set; }
        public double y { get; set; }

        public Vector Normalised
        {
            get
            {
                Vector vector = Normalise();
                this.x = vector.x;
                this.y = vector.y;
                return vector;
            }
        }
        public double Magnitude { get { return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); } }

        #endregion

        #region Non-static Methods
        public Vector() {
            x = 0;
            y = 0;
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Set the X and Y values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Returns the normalised vector
        /// </summary>
        /// <returns></returns>
        public Vector Normalise()
        {
            return new Vector(x / Magnitude, y / Magnitude);
        }

        public override string ToString()
        {
            return "X: " + x + " Y: " + y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Work out the angle between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Angle(Vector v1, Vector v2)
        {
            return Math.Acos(Dot(v1, v2)/ (v1.Magnitude * v2.Magnitude));
        }

        /// <summary>
        /// Work out the Dot product between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Dot(Vector v1, Vector v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y);
        }

        /// <summary>
        /// Work out the distance between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Distance(Vector v1, Vector v2)
        {
            return Math.Sqrt(Math.Pow(DifferenceX(v1, v2), 2) + Math.Pow(DifferenceY(v1, v2), 2));
        }

        public static double DistanceSqrt(Vector v1, Vector v2)
        {
            return Math.Pow(DifferenceX(v1, v2), 2) + Math.Pow(DifferenceY(v1, v2), 2);
        }

        public static double DifferenceX(Vector v1, Vector v2) {
            return v2.x - v1.x;
        }

        public static double DifferenceY(Vector v1, Vector v2) {
            return v2.y - v1.y;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector operator *(Vector v1, double scaler)
        {
            return new Vector(v1.x * scaler, v1.y * scaler);
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        #endregion

    }
}
