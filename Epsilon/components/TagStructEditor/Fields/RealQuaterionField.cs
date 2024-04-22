using System;
using System.Windows.Media.Media3D;
using TagTool.Common;

namespace TagStructEditor.Fields
{
    public class RealQuaternionField : ValueField
    {
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }
        public float W { get; set; }

        public bool DegreeView { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        private bool FromDegrees = false;
        private bool FromRealQt = false;

        public RealQuaternionField(ValueFieldInfo info) : base(info)
        {
        }

        public override void Accept(IFieldVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnPopulate(object value)
        {
            var plane = (TagTool.Common.RealQuaternion)value;
            I = plane.I;
            J = plane.J;
            K = plane.K;
            W = plane.W;

            plane *= plane.Length;
            SetEulerAngles(new Quaternion(plane.I, plane.J, plane.K, plane.W));
            DegreeView = true;
        }

        private void UpdateValue()
        {
            if (IsPopulating || FromDegrees)
                return;

            SetActualValue(new TagTool.Common.RealQuaternion(I, J, K, W));
            var q = new RealQuaternion(I, J, K, W);
            q *= q.Length;
            SetEulerAngles(new Quaternion(q.I, q.J, q.K, q.W));
        }

        private void UpdateValueFromDegrees()
        {
            if (IsPopulating || FromRealQt)
                return;

            Quaternion q = ToQuaternion(X, Y, Z);
            var realq = new RealQuaternion((float)q.X, (float)q.Y, (float)q.Z, (float)q.W);
            realq.Normalize();
            SetActualValue(realq);

            FromDegrees = true;
            I = realq.I;
            J = realq.J;
            K = realq.K;
            W = realq.W;
            FromDegrees = false;
        }

        public void OnIChanged() => UpdateValue();
        public void OnJChanged() => UpdateValue();
        public void OnKChanged() => UpdateValue();
        public void OnWChanged() => UpdateValue();
        public void OnXChanged() => UpdateValueFromDegrees();
        public void OnYChanged() => UpdateValueFromDegrees();
        public void OnZChanged() => UpdateValueFromDegrees();

        private void SetEulerAngles(Quaternion q)
        {
            Vector3D angles = new Vector3D();

            // roll / x
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
            {
                //angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
                angles.Y = (float)(Math.PI / 2);

                if (Math.Sign(sinp) < 0)
                    angles.Y *= -1;
            }
            else
            {
                angles.Y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            //radians -> degrees, then round
            double dg = (180 / Math.PI);
            FromRealQt = true;
            X = (float)Math.Round((angles.X * dg), 3);
            Y = (float)Math.Round((angles.Y * dg), 3);
            Z = (float)Math.Round((angles.Z * dg), 3);
            FromRealQt = false;
            //return angles;
        }

        public static Quaternion ToQuaternion(float X, float Y, float Z)
        {
            // degrees -> radians
            double r = (Math.PI / 180);
            Vector3D v = new Vector3D(X * r, Y * r, Z * r);

            float cy = (float)Math.Cos(v.Z * 0.5);
            float sy = (float)Math.Sin(v.Z * 0.5);
            float cp = (float)Math.Cos(v.Y * 0.5);
            float sp = (float)Math.Sin(v.Y * 0.5);
            float cr = (float)Math.Cos(v.X * 0.5);
            float sr = (float)Math.Sin(v.X * 0.5);

            return new Quaternion
            {
                W = (cr * cp * cy + sr * sp * sy),
                X = (sr * cp * cy - cr * sp * sy),
                Y = (cr * sp * cy + sr * cp * sy),
                Z = (cr * cp * sy - sr * sp * cy)
            };
        }
    }
}
