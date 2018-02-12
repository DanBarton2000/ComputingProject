using System;

namespace ComputingProject
{
    public class Vector2
    {
        #region Variables
        public double x { get; set; }
        public double y { get; set; }

        public Vector2 Normalised
        {
            get
            {
                Vector2 vector = Normalise();
                x = vector.x;
                y = vector.y;
                return vector;
            }
        }
        public double Magnitude { get { return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); } }

        #endregion

        #region Non-static Methods
        public Vector2() {
            x = 0;
            y = 0;
        }

        public Vector2(double x, double y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Set the X and Y values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(double x, double y) {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Set the x and y values using a Vector2
        /// </summary>
        /// <param name="v"></param>
        public void Set(Vector2 v) {
            x = v.x;
            y = v.y;
        }

        public void Add(double x, double y) {
            this.x += x;
            this.y += y;
        }

        public void Add(Vector2 v) {
            x += v.x;
            y += v.y;
        }

        /// <summary>
        /// Returns the normalised vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Normalise()
        {
            return new Vector2(x / Magnitude, y / Magnitude);
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
        public static double Angle(Vector2 v1, Vector2 v2)
        {
            return Math.Acos(Dot(v1, v2)/ (v1.Magnitude * v2.Magnitude));
        }

        /// <summary>
        /// Work out the Dot product between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y);
        }

        /// <summary>
        /// Work out the distance between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Distance(Vector2 v1, Vector2 v2) {
            if (v1 == null || v2 == null) {
                return 0;
            }
            return Math.Sqrt(Math.Pow(DifferenceX(v1, v2), 2) + Math.Pow(DifferenceY(v1, v2), 2));
        }

        /// <summary>
        /// Works out the distance squared between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DistanceSqr(Vector2 v1, Vector2 v2) {
            if (v1 == null || v2 == null) {
                return 0;
            }
            return Math.Pow(DifferenceX(v1, v2), 2) + Math.Pow(DifferenceY(v1, v2), 2);
        }

        /// <summary>
        /// Works out the X difference between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DifferenceX(Vector2 v1, Vector2 v2) {
            if (v1 == null || v2 == null) {
                return 0;
            }
            return v2.x - v1.x;
        }

        /// <summary>
        /// Works out the Y difference between 2 vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DifferenceY(Vector2 v1, Vector2 v2) {
            if (v1 == null || v2 == null) {
                return 0;
            }
            return v2.y - v1.y;
        }

        /// <summary>
        /// Adds 2 vectors together
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2) {
            if (v1 == null && v2 == null) {
                return new Vector2();
            }
            else if (v1 == null) {
                return v2;
            }
            else if (v2 == null) {
                return v1;
            }

            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Subtracts 2 vectors together
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2) {
            if (v1 == null && v2 == null) {
                return new Vector2();
            }
            else if (v1 == null) {
                return v2;
            }
            else if (v2 == null) {
                return v1;
            }
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Works out the product between a vector and a scaler
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scaler"></param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 v1, double scaler) {
            if (ReferenceEquals(v1, null)) {
                return null;
            }
            return new Vector2(v1.x * scaler, v1.y * scaler);
        }

        /// <summary>
        /// Works out the product between a vector and a scaler
        /// </summary>
        /// <param name="scaler"></param>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Vector2 operator *(double scaler, Vector2 v1) {
            if (ReferenceEquals(v1, null)) {
                return null;
            }
            return new Vector2(v1.x * scaler, v1.y * scaler);
        }

        /// <summary>
        /// Divides a vector by a scaler
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="scaler"></param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 v1, double scaler)
        {
            return new Vector2(v1.x / scaler, v1.y / scaler);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            if (!ReferenceEquals(v1, null) && !ReferenceEquals(v2, null)) {
                return v1.x == v2.x && v1.y == v2.y;
            }
            else if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null)) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            if (!ReferenceEquals(v1, null) && !ReferenceEquals(v2, null)) {
                return v1.x != v2.x || v1.y != v2.y;
            }
            else if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null)) {
                return true;
            }
            return false;
        }

        #endregion

    }
}
