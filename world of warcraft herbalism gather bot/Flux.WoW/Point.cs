using System;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Flux.WoW
{
    [StructLayout(LayoutKind.Explicit, Size = 4 * 3 /* I know it's 12. I did it for a reason. */)]
    public struct Point : IEquatable<Point>
    {
        public static readonly Point Empty = new Point(0, 0, 0);

        public Point(float x, float y, float z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(float[] vecs) : this(vecs[0], vecs[1], vecs[2])
        {
        }

        [FieldOffset(0)]
        public float X;

        [FieldOffset(4)]
        public float Y;

        [FieldOffset(8)]
        public float Z;

        public bool IsValid { get { return X != 0f && Y != 0f && Z != 0f; } }

        #region IEquatable<Point> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Point other)
        {
            return Distance(other) <= .2;
        }

        #endregion

        /// <summary>
        /// Gets the distance to the specified location.
        /// </summary>
        /// <param name="toX">To X.</param>
        /// <param name="toY">To Y.</param>
        /// <param name="toZ">To Z.</param>
        /// <returns></returns>
        public double Distance(float toX, float toY, float toZ)
        {
            float dX = X - toX;
            float dY = Y - toY;
            float dZ = Z - toZ;
            return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        public Point RayCast(float heading, float distance)
        {
            return RayCast(this, heading, distance);
        }

        public static Point RayCast(Point from, float heading, float distance)
        {
            return new Point(from.X + ((float) Math.Cos(heading) * distance), from.Y + ((float) Math.Sin(heading) * distance), from.Z);
        }

        public float Length { get { return (float) Math.Sqrt(X * X + Y * Y + Z * Z); } }

        public float DotProduct(Point to)
        {
            return X * to.X + Y * to.Y + Z * to.Z;
        }

        public double CalculateAngle(Point to, bool inRadians)
        {
            // Make sure neither are 0,0,0
            if (!IsValid || !to.IsValid)
            {
                return 0;
            }
            // Avoid the sillyness here.
            float thisMag = Length;
            float toMag = to.Length;

            // Normalize them..
            Point v1 = this * (1 / thisMag);
            Point v2 = to * (1 / toMag);

            double rads = Math.Acos(v1.X * v2.X + v1.Y * v2.Y + v1.Z + v2.Z);

            return inRadians ? rads : rads * 180 / Math.PI;
        }

        public static Point operator *(Point pt, float mag)
        {
            return new Point(pt.X * mag, pt.Y * mag, pt.Z * mag);
        }

        /// <summary>
        /// Gets the distance to the specified Point.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <returns></returns>
        public double Distance(Point pt)
        {
            return Distance(pt.X, pt.Y, pt.Z);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("X: {0:F2}, Y: {1:F2}, Z: {2:F2}", X, Y, Z);
        }

        public float[] GetArray()
        {
            return new[] {X, Y, Z};
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        ///                 </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (obj.GetType() != typeof (Point))
            {
                return false;
            }
            return Equals((Point) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public XElement GetXml()
        {
            return new XElement("Point", new XAttribute("X", X), new XAttribute("Y", Y), new XAttribute("Z", Z));
        }
    }
}