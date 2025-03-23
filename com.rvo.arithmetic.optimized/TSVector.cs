/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

using System;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

namespace RVO.Arithmetic.Optimized
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TSVector
    {
        private static FP ZeroEpsilonSq = TSMath.Epsilon;
        internal static TSVector InternalZero;
        internal static TSVector Arbitrary;

        /// <summary>The X component of the vector.</summary>
        public FP x;
        /// <summary>The Y component of the vector.</summary>
        public FP y;
        /// <summary>The Z component of the vector.</summary>
        public FP z;

        // 缓存平方长度，避免重复计算
        private FP? _sqrMagnitude;
        // 缓存长度，避免重复计算
        private FP? _magnitude;

        private static readonly ConcurrentBag<TSVector> _pool = new ConcurrentBag<TSVector>();
        private static readonly object _lock = new object();

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly TSVector zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly TSVector left;
        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly TSVector right;
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly TSVector up;
        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly TSVector down;
        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly TSVector back;
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly TSVector forward;
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly TSVector one;
        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly TSVector MinValue;
        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly TSVector MaxValue;
        #endregion

        #region Private static constructor
        static TSVector()
        {
            one = new TSVector(1, 1, 1);
            zero = new TSVector(0, 0, 0);
            left = new TSVector(-1, 0, 0);
            right = new TSVector(1, 0, 0);
            up = new TSVector(0, 1, 0);
            down = new TSVector(0, -1, 0);
            back = new TSVector(0, 0, -1);
            forward = new TSVector(0, 0, 1);
            MinValue = new TSVector(FP.MinValue);
            MaxValue = new TSVector(FP.MaxValue);
            Arbitrary = new TSVector(1, 1, 1);
            InternalZero = zero;
        }
        #endregion

        public static TSVector Abs(TSVector other)
        {
            return new TSVector(FP.Abs(other.x), FP.Abs(other.y), FP.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FP sqrMagnitude
        {
            get
            {
                if (!_sqrMagnitude.HasValue)
                {
                    _sqrMagnitude = (x * x) + (y * y) + (z * z);
                }
                return _sqrMagnitude.Value;
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FP magnitude
        {
            get
            {
                if (!_magnitude.HasValue)
                {
                    _magnitude = FP.Sqrt(sqrMagnitude);
                }
                return _magnitude.Value;
            }
        }

        public static TSVector ClampMagnitude(TSVector vector, FP maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public TSVector normalized
        {
            get
            {
                TSVector result = new TSVector(this.x, this.y, this.z);
                result.Normalize();
                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public TSVector(int x, int y, int z) : this((FP)x, (FP)y, (FP)z)
        {
        }

        public TSVector(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            _sqrMagnitude = null;
            _magnitude = null;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public TSVector(FP xyz) : this(xyz, xyz, xyz)
        {
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            _sqrMagnitude = null;
            _magnitude = null;
        }

        public static TSVector Lerp(TSVector from, TSVector to, FP percent)
        {
            return from + (to - from) * percent;
        }

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        #endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>
        #region public override bool Equals(object obj)
        public override bool Equals(object obj)
        {
            if (!(obj is TSVector)) return false;
            TSVector other = (TSVector)obj;
            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(TSVector other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
            _sqrMagnitude = null;
            _magnitude = null;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        #region public static bool operator ==(JVector value1, JVector value2)
        public static bool operator ==(TSVector value1, TSVector value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
        #endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        #region public static bool operator !=(JVector value1, JVector value2)
        public static bool operator !=(TSVector value1, TSVector value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        #region public static JVector Min(JVector value1, JVector value2)
        public static TSVector Min(TSVector value1, TSVector value2)
        {
            return new TSVector(
                (value1.x < value2.x) ? value1.x : value2.x,
                (value1.y < value2.y) ? value1.y : value2.y,
                (value1.z < value2.z) ? value1.z : value2.z
            );
        }
        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        #region public static JVector Max(JVector value1, JVector value2)
        public static TSVector Max(TSVector value1, TSVector value2)
        {
            return new TSVector(
                (value1.x > value2.x) ? value1.x : value2.x,
                (value1.y > value2.y) ? value1.y : value2.y,
                (value1.z > value2.z) ? value1.z : value2.z
            );
        }
        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
        public void MakeZero()
        {
            x = FP.Zero;
            y = FP.Zero;
            z = FP.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
        public bool IsZero()
        {
            return (this.sqrMagnitude == FP.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        #region public static JVector Transform(JVector position, JMatrix matrix)
        public static TSVector Transform(TSVector position, TSMatrix matrix)
        {
            // 缓存输入值以避免多次访问
            FP x = position.x;
            FP y = position.y;
            FP z = position.z;

            return new TSVector(
                x * matrix.M11 + y * matrix.M21 + z * matrix.M31,
                x * matrix.M12 + y * matrix.M22 + z * matrix.M32,
                x * matrix.M13 + y * matrix.M23 + z * matrix.M33
            );
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        #region public static FP Dot(JVector vector1, JVector vector2)
        public static FP Dot(TSVector vector1, TSVector vector2)
        {
            // 直接计算点积，避免创建临时向量
            return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
        }
        #endregion

        // Projects a vector onto another vector.
        public static TSVector Project(TSVector vector, TSVector onNormal)
        {
            FP sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < TSMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
        public static TSVector ProjectOnPlane(TSVector vector, TSVector planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }

        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static FP Angle(TSVector from, TSVector to)
        {
            return TSMath.Acos(TSMath.Clamp(Dot(from.normalized, to.normalized), -FP.ONE, FP.ONE)) * TSMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static FP SignedAngle(TSVector from, TSVector to, TSVector axis)
        {
            TSVector fromNorm = from.normalized, toNorm = to.normalized;
            FP unsignedAngle = TSMath.Acos(TSMath.Clamp(Dot(fromNorm, toNorm), -FP.ONE, FP.ONE)) * TSMath.Rad2Deg;
            FP sign = TSMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static void Add(JVector value1, JVector value2)
        public static TSVector Add(TSVector value1, TSVector value2)
        {
            return new TSVector(value1.x + value2.x, value1.y + value2.y, value1.z + value2.z);
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static TSVector Divide(TSVector value1, FP scaleFactor)
        {
            FP invScaleFactor = FP.One / scaleFactor;
            return new TSVector(value1.x * invScaleFactor, value1.y * invScaleFactor, value1.z * invScaleFactor);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector Subtract(JVector value1, JVector value2)
        public static TSVector Subtract(TSVector value1, TSVector value2)
        {
            return new TSVector(value1.x - value2.x, value1.y - value2.y, value1.z - value2.z);
        }
        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        #region public static JVector Cross(JVector vector1, JVector vector2)
        public static TSVector Cross(TSVector vector1, TSVector vector2)
        {
            // 缓存输入值以避免多次访问
            FP x1 = vector1.x;
            FP y1 = vector1.y;
            FP z1 = vector1.z;
            FP x2 = vector2.x;
            FP y2 = vector2.y;
            FP z2 = vector2.z;

            return new TSVector(
                y1 * z2 - z1 * y2,
                z1 * x2 - x1 * z2,
                x1 * y2 - y1 * x2
            );
        }
        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>
        #region public override int GetHashCode()
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        #region public static JVector Negate(JVector value)
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static TSVector Negate(TSVector value)
        {
            return new TSVector(-value.x, -value.y, -value.z);
        }
        #endregion

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            // 使用平方长度避免开方运算
            FP sqrLen = sqrMagnitude;

            if (sqrLen < TSMath.Epsilon)
            {
                x = FP.Zero;
                y = FP.Zero;
                z = FP.Zero;
            }
            else
            {
                // 使用快速倒数平方根近似
                FP invLen = FP.One / FP.Sqrt(sqrLen);
                x *= invLen;
                y *= invLen;
                z *= invLen;
            }
            _sqrMagnitude = null;
            _magnitude = null;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        public static TSVector Normalize(TSVector value)
        {
            FP sqrLen = value.sqrMagnitude;
            if (sqrLen < TSMath.Epsilon)
            {
                return zero;
            }
            FP invLen = FP.One / FP.Sqrt(sqrLen);
            return new TSVector(value.x * invLen, value.y * invLen, value.z * invLen);
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        #region public static JVector Multiply(JVector value1, FP scaleFactor)
        public static TSVector Multiply(TSVector value1, FP scaleFactor)
        {
            return new TSVector(value1.x * scaleFactor, value1.y * scaleFactor, value1.z * scaleFactor);
        }
        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        #region public static JVector operator %(JVector value1, JVector value2)
        public static TSVector operator %(TSVector value1, TSVector value2)
        {
            return Cross(value1, value2);
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        #region public static FP operator *(JVector value1, JVector value2)
        public static FP operator *(TSVector value1, TSVector value2)
        {
            return Dot(value1, value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(JVector value1, FP value2)
        public static TSVector operator *(TSVector value1, FP value2)
        {
            return Multiply(value1, value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(FP value1, JVector value2)
        public static TSVector operator *(FP value1, TSVector value2)
        {
            return Multiply(value2, value1);
        }
        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector operator -(JVector value1, JVector value2)
        public static TSVector operator -(TSVector value1, TSVector value2)
        {
            return Subtract(value1, value2);
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static JVector operator +(JVector value1, JVector value2)
        public static TSVector operator +(TSVector value1, TSVector value2)
        {
            return Add(value1, value2);
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static TSVector operator /(TSVector value1, FP value2)
        {
            return Divide(value1, value2);
        }

        public TSVector2 ToTSVector2()
        {
            return new TSVector2(this.x, this.y);
        }

        public TSVector4 ToTSVector4()
        {
            return new TSVector4(this.x, this.y, this.z, FP.One);
        }

        public static TSVector Acquire()
        {
            if (_pool.TryTake(out var vector))
            {
                return vector;
            }
            return new TSVector();
        }

        public static void Release(TSVector vector)
        {
            vector.x = 0;
            vector.y = 0;
            vector.z = 0;
            _pool.Add(vector);
        }
    }
}