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
    /// A Quaternion representing an orientation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct TSQuaternion
    {

        /// <summary>The X component of the quaternion.</summary>
        public FP x;
        /// <summary>The Y component of the quaternion.</summary>
        public FP y;
        /// <summary>The Z component of the quaternion.</summary>
        public FP z;
        /// <summary>The W component of the quaternion.</summary>
        public FP w;

        private static readonly ConcurrentBag<TSQuaternion> _pool = new ConcurrentBag<TSQuaternion>();
        private static readonly object _lock = new object();

        public static readonly TSQuaternion identity = new TSQuaternion(0, 0, 0, 1);

        public TSQuaternion(FP x, FP y, FP z, FP w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static TSQuaternion Acquire()
        {
            if (_pool.TryTake(out var quaternion))
            {
                return quaternion;
            }
            return new TSQuaternion();
        }

        public static void Release(TSQuaternion quaternion)
        {
            quaternion.x = 0;
            quaternion.y = 0;
            quaternion.z = 0;
            quaternion.w = 1;
            _pool.Add(quaternion);
        }

        public static TSQuaternion operator *(TSQuaternion a, TSQuaternion b)
        {
            return new TSQuaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
        }

        public static TSQuaternion operator *(TSQuaternion q, FP scalar)
        {
            return new TSQuaternion(q.x * scalar, q.y * scalar, q.z * scalar, q.w * scalar);
        }

        public static TSQuaternion operator +(TSQuaternion a, TSQuaternion b)
        {
            return new TSQuaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static TSQuaternion operator -(TSQuaternion a, TSQuaternion b)
        {
            return new TSQuaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static bool operator ==(TSQuaternion a, TSQuaternion b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(TSQuaternion a, TSQuaternion b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public static TSQuaternion Inverse(TSQuaternion q)
        {
            FP norm = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
            if (norm == 0)
                return identity;
            return new TSQuaternion(-q.x / norm, -q.y / norm, -q.z / norm, q.w / norm);
        }

        public static TSQuaternion Conjugate(TSQuaternion q)
        {
            return new TSQuaternion(-q.x, -q.y, -q.z, q.w);
        }

        public static FP Dot(TSQuaternion a, TSQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public void Normalize()
        {
            FP norm = FP.Sqrt(x * x + y * y + z * z + w * w);
            if (norm != 0)
            {
                x /= norm;
                y /= norm;
                z /= norm;
                w /= norm;
            }
        }

        public static TSQuaternion Slerp(TSQuaternion from, TSQuaternion to, FP t)
        {
            FP dot = Dot(from, to);
            if (dot < 0)
            {
                to = -to;
                dot = -dot;
            }

            if (dot > FP.One - FP.FromRaw(1))
            {
                return from + (to - from) * t;
            }

            FP theta = FP.Acos(dot);
            FP sinTheta = FP.Sin(theta);
            FP invSinTheta = FP.One / sinTheta;
            FP coeff1 = FP.Sin((FP.One - t) * theta) * invSinTheta;
            FP coeff2 = FP.Sin(t * theta) * invSinTheta;

            return from * coeff1 + to * coeff2;
        }

        public override bool Equals(object obj)
        {
            if (obj is TSQuaternion)
            {
                TSQuaternion other = (TSQuaternion)obj;
                return x == other.x && y == other.y && z == other.z && w == other.w;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }

        public void Set(FP new_x, FP new_y, FP new_z, FP new_w)
        {
            this.x = new_x;
            this.y = new_y;
            this.z = new_z;
            this.w = new_w;
        }

        public void SetFromToRotation(TSVector fromDirection, TSVector toDirection)
        {
            TSQuaternion targetRotation = TSQuaternion.FromToRotation(fromDirection, toDirection);
            this.Set(targetRotation.x, targetRotation.y, targetRotation.z, targetRotation.w);
        }

        public TSVector eulerAngles
        {
            get
            {
                TSVector result = new TSVector();

                FP ysqr = y * y;
                FP t0 = -2.0f * (ysqr + z * z) + 1.0f;
                FP t1 = +2.0f * (x * y - w * z);
                FP t2 = -2.0f * (x * z + w * y);
                FP t3 = +2.0f * (y * z - w * x);
                FP t4 = -2.0f * (x * x + ysqr) + 1.0f;

                t2 = t2 > 1.0f ? 1.0f : t2;
                t2 = t2 < -1.0f ? -1.0f : t2;

                result.x = FP.Atan2(t3, t4) * FP.Rad2Deg;
                result.y = FP.Asin(t2) * FP.Rad2Deg;
                result.z = FP.Atan2(t1, t0) * FP.Rad2Deg;

                return result * -1;
            }
        }

        public static FP Angle(TSQuaternion a, TSQuaternion b)
        {
            TSQuaternion aInv = TSQuaternion.Inverse(a);
            TSQuaternion f = b * aInv;

            FP angle = FP.Acos(f.w) * 2 * FP.Rad2Deg;

            if (angle > 180)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The sum of both quaternions.</returns>
        #region public static JQuaternion Add(JQuaternion quaternion1, JQuaternion quaternion2)
        public static TSQuaternion Add(TSQuaternion quaternion1, TSQuaternion quaternion2)
        {
            TSQuaternion result;
            TSQuaternion.Add(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        public static TSQuaternion LookRotation(TSVector forward)
        {
            return CreateFromMatrix(TSMatrix.LookAt(forward, TSVector.up));
        }

        public static TSQuaternion LookRotation(TSVector forward, TSVector upwards)
        {
            return CreateFromMatrix(TSMatrix.LookAt(forward, upwards));
        }

        public static TSQuaternion RotateTowards(TSQuaternion from, TSQuaternion to, FP maxDegreesDelta)
        {
            FP dot = Dot(from, to);

            if (dot < 0.0f)
            {
                to = Multiply(to, -1);
                dot = -dot;
            }

            FP halfTheta = FP.Acos(dot);
            FP theta = halfTheta * 2;

            maxDegreesDelta *= FP.Deg2Rad;

            if (maxDegreesDelta >= theta)
            {
                return to;
            }

            maxDegreesDelta /= theta;

            return Multiply(Multiply(from, FP.Sin((1 - maxDegreesDelta) * halfTheta)) + Multiply(to, FP.Sin(maxDegreesDelta * halfTheta)), 1 / FP.Sin(halfTheta));
        }

        public static TSQuaternion Euler(FP x, FP y, FP z)
        {
            x *= FP.Deg2Rad;
            y *= FP.Deg2Rad;
            z *= FP.Deg2Rad;

            TSQuaternion rotation;
            TSQuaternion.CreateFromYawPitchRoll(y, x, z, out rotation);

            return rotation;
        }

        public static TSQuaternion Euler(TSVector eulerAngles)
        {
            return Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        public static TSQuaternion AngleAxis(FP angle, TSVector axis)
        {
            axis = axis * FP.Deg2Rad;
            axis.Normalize();

            FP halfAngle = angle * FP.Deg2Rad * FP.Half;

            TSQuaternion rotation;
            FP sin = FP.Sin(halfAngle);

            rotation.x = axis.x * sin;
            rotation.y = axis.y * sin;
            rotation.z = axis.z * sin;
            rotation.w = FP.Cos(halfAngle);

            return rotation;
        }

        public static void CreateFromYawPitchRoll(FP yaw, FP pitch, FP roll, out TSQuaternion result)
        {
            FP num9 = roll * FP.Half;
            FP num6 = FP.Sin(num9);
            FP num5 = FP.Cos(num9);
            FP num8 = pitch * FP.Half;
            FP num4 = FP.Sin(num8);
            FP num3 = FP.Cos(num8);
            FP num7 = yaw * FP.Half;
            FP num2 = FP.Sin(num7);
            FP num = FP.Cos(num7);
            result.x = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.w = ((num * num3) * num5) + ((num2 * num4) * num6);
        }

        /// <summary>
        /// Quaternions are added.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The sum of both quaternions.</param>
        public static void Add(ref TSQuaternion quaternion1, ref TSQuaternion quaternion2, out TSQuaternion result)
        {
            result.x = quaternion1.x + quaternion2.x;
            result.y = quaternion1.y + quaternion2.y;
            result.z = quaternion1.z + quaternion2.z;
            result.w = quaternion1.w + quaternion2.w;
        }
        #endregion

        public static TSQuaternion Lerp(TSQuaternion a, TSQuaternion b, FP t)
        {
            t = TSMath.Clamp(t, FP.Zero, FP.One);

            return LerpUnclamped(a, b, t);
        }

        public static TSQuaternion LerpUnclamped(TSQuaternion a, TSQuaternion b, FP t)
        {
            TSQuaternion result = TSQuaternion.Multiply(a, (1 - t)) + TSQuaternion.Multiply(b, t);
            result.Normalize();

            return result;
        }

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The product of both quaternions.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, JQuaternion quaternion2)
        public static TSQuaternion Multiply(TSQuaternion quaternion1, TSQuaternion quaternion2)
        {
            TSQuaternion result;
            TSQuaternion.Multiply(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        /// <summary>
        /// Multiply two quaternions.
        /// </summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="result">The product of both quaternions.</param>
        public static void Multiply(ref TSQuaternion quaternion1, ref TSQuaternion quaternion2, out TSQuaternion result)
        {
            FP x = quaternion1.x;
            FP y = quaternion1.y;
            FP z = quaternion1.z;
            FP w = quaternion1.w;
            FP num4 = quaternion2.x;
            FP num3 = quaternion2.y;
            FP num2 = quaternion2.z;
            FP num = quaternion2.w;
            FP num12 = (y * num2) - (z * num3);
            FP num11 = (z * num4) - (x * num2);
            FP num10 = (x * num3) - (y * num4);
            FP num9 = ((x * num4) + (y * num3)) + (z * num2);
            result.x = ((x * num) + (num4 * w)) + num12;
            result.y = ((y * num) + (num3 * w)) + num11;
            result.z = ((z * num) + (num2 * w)) + num10;
            result.w = (w * num) - num9;
        }
        #endregion

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <returns>The scaled quaternion.</returns>
        #region public static JQuaternion Multiply(JQuaternion quaternion1, FP scaleFactor)
        public static TSQuaternion Multiply(TSQuaternion quaternion1, FP scaleFactor)
        {
            TSQuaternion result;
            TSQuaternion.Multiply(ref quaternion1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Scale a quaternion
        /// </summary>
        /// <param name="quaternion1">The quaternion to scale.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="result">The scaled quaternion.</param>
        public static void Multiply(ref TSQuaternion quaternion1, FP scaleFactor, out TSQuaternion result)
        {
            result.x = quaternion1.x * scaleFactor;
            result.y = quaternion1.y * scaleFactor;
            result.z = quaternion1.z * scaleFactor;
            result.w = quaternion1.w * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <returns>JQuaternion representing an orientation.</returns>
        #region public static JQuaternion CreateFromMatrix(JMatrix matrix)
        public static TSQuaternion CreateFromMatrix(TSMatrix matrix)
        {
            TSQuaternion result;
            TSQuaternion.CreateFromMatrix(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion from a matrix.
        /// </summary>
        /// <param name="matrix">A matrix representing an orientation.</param>
        /// <param name="result">JQuaternion representing an orientation.</param>
        public static void CreateFromMatrix(ref TSMatrix matrix, out TSQuaternion result)
        {
            FP num8 = (matrix.M11 + matrix.M22) + matrix.M33;
            if (num8 > FP.Zero)
            {
                FP num = FP.Sqrt((num8 + FP.One));
                result.w = num * FP.Half;
                num = FP.Half / num;
                result.x = (matrix.M23 - matrix.M32) * num;
                result.y = (matrix.M31 - matrix.M13) * num;
                result.z = (matrix.M12 - matrix.M21) * num;
            }
            else if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                FP num7 = FP.Sqrt((((FP.One + matrix.M11) - matrix.M22) - matrix.M33));
                FP num4 = FP.Half / num7;
                result.x = FP.Half * num7;
                result.y = (matrix.M12 + matrix.M21) * num4;
                result.z = (matrix.M13 + matrix.M31) * num4;
                result.w = (matrix.M23 - matrix.M32) * num4;
            }
            else if (matrix.M22 > matrix.M33)
            {
                FP num6 = FP.Sqrt((((FP.One + matrix.M22) - matrix.M11) - matrix.M33));
                FP num3 = FP.Half / num6;
                result.x = (matrix.M21 + matrix.M12) * num3;
                result.y = FP.Half * num6;
                result.z = (matrix.M32 + matrix.M23) * num3;
                result.w = (matrix.M31 - matrix.M13) * num3;
            }
            else
            {
                FP num5 = FP.Sqrt((((FP.One + matrix.M33) - matrix.M11) - matrix.M22));
                FP num2 = FP.Half / num5;
                result.x = (matrix.M31 + matrix.M13) * num2;
                result.y = (matrix.M32 + matrix.M23) * num2;
                result.z = FP.Half * num5;
                result.w = (matrix.M12 - matrix.M21) * num2;
            }
        }
        #endregion

        /**
         *  @brief Rotates a {@link TSVector} by the {@link TSQuanternion}.
         **/
        public static TSVector operator *(TSQuaternion quat, TSVector vec)
        {
            FP num = quat.x * 2f;
            FP num2 = quat.y * 2f;
            FP num3 = quat.z * 2f;
            FP num4 = quat.x * num;
            FP num5 = quat.y * num2;
            FP num6 = quat.z * num3;
            FP num7 = quat.x * num2;
            FP num8 = quat.x * num3;
            FP num9 = quat.y * num3;
            FP num10 = quat.w * num;
            FP num11 = quat.w * num2;
            FP num12 = quat.w * num3;

            TSVector result = new TSVector();
            result.x = (1f - (num5 + num6)) * vec.x + (num7 - num12) * vec.y + (num8 + num11) * vec.z;
            result.y = (num7 + num12) * vec.x + (1f - (num4 + num6)) * vec.y + (num9 - num10) * vec.z;
            result.z = (num8 - num11) * vec.x + (num9 + num10) * vec.y + (1f - (num4 + num5)) * vec.z;

            return result;
        }

        public static TSQuaternion operator -(TSQuaternion quaternion)
        {
            return new TSQuaternion(-quaternion.x, -quaternion.y, -quaternion.z, -quaternion.w);
        }

        public static TSQuaternion FromToRotation(TSVector fromDirection, TSVector toDirection)
        {
            TSVector axis = TSVector.Cross(fromDirection, toDirection);
            FP angle = TSVector.Angle(fromDirection, toDirection);
            return AngleAxis(angle, axis.normalized);
        }

        public TSQuaternion Normalized
        {
            get
            {
                TSQuaternion result = this;
                FP mag = TSMath.Sqrt(x * x + y * y + z * z + w * w);
                if (mag > FP.Zero)
                {
                    result.x /= mag;
                    result.y /= mag;
                    result.z /= mag;
                    result.w /= mag;
                }
                return result;
            }
        }

    }
}
