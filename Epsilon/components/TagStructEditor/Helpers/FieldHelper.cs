using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagStructEditor.Fields;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.Definitions.RenderMethod.RenderMethodPostprocessBlock.TextureConstant;

namespace TagStructEditor.Helpers
{
    public class FieldHelper
    {
        public static string GetFieldPath(IField field)
        {
            var parts = new List<string>();
            IField f = field;
            while (f != null)
            {
                if (f is ValueField vf)
                {
                    if (vf is BlockField bf && vf != field)
                        parts.Add($"{vf.FieldInfo.ActualName}[{bf.CurrentIndex}]");
                    else
                        parts.Add(vf.FieldInfo.ActualName);
                }

                f = f.Parent;
            }

            if (field is TagFunctionField)
                parts.Insert(0, "Data");

            return string.Join(".", parts.AsEnumerable().Reverse().Where(x => !string.IsNullOrEmpty(x)));
        }

        public static string GetFieldValueForSetField(StringTable stringTable, ValueField field)
        {
            return FormatValue(field.FieldInfo.ValueGetter(field.Owner));

            string FormatValue(object value)
            {
                // TODO: this should really be in tagtool globally accessible.
                switch (value)
                {
                    case null:
                        return "null";
                    case string str:
                        return $"\"{str}\"";
                    case Angle angle:
                        return $"{angle.Degrees}";
                    case RealEulerAngles2d angles2d:
                        return $"{angles2d.Yaw.Degrees} {angles2d.Pitch.Degrees}";
                    case RealEulerAngles3d angles3d:
                        return $"{angles3d.Yaw.Degrees} {angles3d.Pitch.Degrees} {angles3d.Roll.Degrees}";
                    case RealVector3d vector3d:
                        return $"{vector3d.I} {vector3d.J} {vector3d.K}";
                    case RealQuaternion quaternion:
                        return $"{quaternion.I} {quaternion.J} {quaternion.K} {quaternion.W}";
                    case RealPoint3d point3d:
                        return $"{point3d.X} {point3d.Y} {point3d.Z}";
                    case RealPoint2d point2d:
                        return $"{point2d.X} {point2d.Y}";
                    case RealPlane3d plane3d:
                        return $"{plane3d.I} {plane3d.J} {plane3d.K} {plane3d.D}";
                    case RealPlane2d plane2d:
                        return $"{plane2d.I} {plane2d.J} {plane2d.D}";
                    case RealArgbColor realArgb:
                        return $"{realArgb.Alpha} {realArgb.Red} {realArgb.Green} {realArgb.Blue}";
                    case ArgbColor argb:
                        return $"{argb.Alpha} {argb.Red} {argb.Green} {argb.Blue}";
                    case RealRgbColor realRgb:
                        return $"{realRgb.Red} {realRgb.Green} {realRgb.Blue}";
                    case RealRgbaColor realRgba:
                        return $"{realRgba.Red} {realRgba.Green} {realRgba.Blue} {realRgba.Alpha}";
                    case RealRectangle3d realRect3d:
                        return $"{realRect3d.X0} {realRect3d.X1} {realRect3d.Y0} {realRect3d.Y1} {realRect3d.Z0} {realRect3d.Z1}";
                    case Rectangle2d rect2d:
                        return $"{rect2d.Top} {rect2d.Left} {rect2d.Bottom} {rect2d.Right}";
                    case RealMatrix4x3 realMatrix4x3:
                        return FormatRealMatrix4x3(realMatrix4x3);
                    case IBounds bounds:
                        return FormatBounds(bounds);
                    case DatumHandle datumHandle:
                        return $"{datumHandle.Salt} {datumHandle.Index}";
                    case StringId stringId:
                        return stringTable.GetString(stringId);
                    case TagTool.Tags.TagFunction function:
                        return FormatTagFunctionData(function);
                    case Enum flags:
                        return $"{string.Join("", value.ToString().Split(' '))}";
                    case PackedSamplerAddressMode packedAddressMode:
                        return $"{packedAddressMode.AddressU} {packedAddressMode.AddressV}";
                    default:
                        return $"{value}";
                }

                string FormatBounds(IBounds bounds)
                {
                    var boundsType = bounds.GetType();
                    var lower = boundsType.GetProperty("Lower").GetValue(bounds);
                    var upper = boundsType.GetProperty("Upper").GetValue(bounds);
                    return $"{FormatValue(lower)} {FormatValue(upper)}";
                }

                string FormatRealMatrix4x3(RealMatrix4x3 matrix)
                {
                    return
                        $"{matrix.m11} {matrix.m12} {matrix.m13}" +
                        $"{matrix.m21} {matrix.m22} {matrix.m23}" +
                        $"{matrix.m31} {matrix.m32} {matrix.m33}" +
                        $"{matrix.m41} {matrix.m42} {matrix.m43}";
                }

                string FormatTagFunctionData(TagTool.Tags.TagFunction function)
                {
                    string valueString = "";
                    foreach (var datum in function.Data)
                        valueString += datum.ToString("X2");

                    return valueString;
                }
            }
        }
    }
}
