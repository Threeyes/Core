using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Threeyes.Core
{
    public static partial class LazyExtension_Common
    {
        #region Object

        /// <summary>
        /// 克隆实例，适用于ScriptableObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name=""></param>
        /// <returns></returns>
        public static T CloneInstantiate<T>(this T obj) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(obj);
        }

        /// <summary>
        /// 检测类型为T的组件com是否为空，如果不为空则执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        /// <param name="func"></param>
        public static void TryExecute<T>(this T com, UnityAction<T> func) where T : Component
        {
            if (com != null)
                func(com);
        }
        /// <summary>
        /// 尝试获取类型为T的组件，如果不为空则执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        /// <param name="func"></param>
        public static void TryGetComponentAndExecute<T>(this Component com, UnityAction<T> func) where T : Component
        {
            T targetCom = com.GetComponent<T>();
            if (targetCom != null)
                func(targetCom);
        }

        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool NotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// 深度拷贝信息，仅支持序列化的对象 https://stackoverflow.com/questions/1031023/copy-a-class-c-sharp
        /// 
        /// Warning：容易报错
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion

        #region Common

        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        public static bool NotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        public static T GetOrSet<T>(this T obj, Func<T> getFunc)
        {
            if (obj == null)
                obj = getFunc();
            return obj;

        }

        #endregion

        #region PlayerPrefs

        public static bool GetBool(this string key)
        {
            return PlayerPrefs.GetInt(key) != 0;
        }

        public static void SetBool(this string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool GetBool(this string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) != 0;
        }

        #endregion

        #region String

        /// <summary>
        /// 为了避免StartsWith等遇到null会报错，需要调用该方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NotNullString(this string str)
        {
            if (str == null)
                return "";
            return str;
        }

        /// <summary>
        /// 通过回车分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strToRemove"></param>
        /// <returns></returns>
        public static string[] SplitByNewLine(this string str)
        {
            return str.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Remove(this string str, string strToRemove)
        {
            return str.Replace(strToRemove, "");
        }

        /// <summary>
        /// Parse string to Int, Etc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myString"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryParse<T>(this string myString, T defaultValue = default)
        {
            T value = defaultValue;
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            try
            {
                value = (T)converter.ConvertFromString(myString);
                return value;
            }
            catch
            {
                //Debug.LogError("转换失败！");
            }
            return value;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool NotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Returns the extension (including the period ".") of the specified path string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExtension(this string path)
        {
            return Path.GetExtension(path);
        }
        public static string GetFileNameWithoutExtension(this FileInfo fileInfo)
        {
            return Path.GetFileNameWithoutExtension(fileInfo.Name);
        }

        public static string GetFileNameWithoutExtension(this string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetParentPath(this string path)
        {
            //return path.Substring(0, path.LastIndexOf("/")) //这个方法也可用
            return Directory.GetParent(path).FullName;
        }


        //——Format for common struct type——

        /// <summary>
        /// 将浮点值裁剪到指定小数点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format">
        /// 2F:     小数点后两位
        /// 00:00 时间  
        /// </param>
        /// <returns></returns>
        public static string Format(this float value, string format = "F2")
        {
            return value.ToString(format);
        }

        //——RichText——

        public static string AppendRichText(this string strContent, string color)
        {
            return string.Format("<color={0}>", color) + strContent + "</color>";
        }

        static public string ToRichText_Color(this string text, Color32 color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
        }

        #endregion

        #region  StringBuilder

        //ToAdd:在指定静态类中通过枚举传入对应的RichTextColor

        //Append RichText in the end
        public static void AppendWarningRichText(this StringBuilder sB, params object[] arrObj)
        {
            sB.AppendRichText("orange", arrObj);
        }
        public static void AppendRichText(this StringBuilder sB, string color, params object[] arrObj)
        {
            sB.Append(string.Format("<color={0}>", color));
            foreach (object str in arrObj)
            {
                sB.Append(str);
            }
            sB.Append("</color>");
        }

        //Wrap with RichText
        public static void WrapWarningRichText(this StringBuilder sB)
        {
            sB.WrapRichText("orange");
        }
        public static void WrapRichText(this StringBuilder sB, string color)
        {
            sB.Insert(0, string.Format("<color={0}>", color));
            sB.Append("</color>");
        }
        #endregion

        #region Vector Common

        /// <summary>
        /// 两个矢量是否垂直
        /// <paramref name="threadShould"/>误差值[0,1]
        /// </summary>
        public static bool IsPerpendicularTo(this Vector3 vt3A, Vector3 vt3B, float threadShould = 0)
        {
            //Dot结果为0表示相互垂直
            float dotValue = Mathf.Abs(Vector3.Dot(vt3A, vt3B));
            return dotValue <= threadShould;
        }

        public static bool IsParallelTo(this Vector3 vt3A, Vector3 vt3B, float threadShould = 0)
        {
            //Dot结果为1表示同向，-1表示反向
            float dotValue = Mathf.Abs(Vector3.Dot(vt3A, vt3B));
            return 1 - dotValue <= threadShould;
        }

        #endregion

        #region Vector2

        public static void Log(this Vector2 v)
        {
            Debug.Log(v.x + " " + v.y);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        { return Quaternion.Euler(0, 0, degrees) * v; }

        /// <summary>
        /// 以X轴为0，逆时针为正，顺时针为负，范围为[-180,180]
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float ToAngle(this Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }
        #endregion

        #region Vector3

        [Obsolete("Use Vector3.ToString instead")]
        public static string Format(this Vector3 v, string format = "F8")
        {
            return v.ToString(format);
            //return string.Format("({0},{1},{2})", v.x.ToString(format), v.y.ToString(format), v.z.ToString(format));
        }

        public static Vector3 ChangeX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 ChangeY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 ChangeZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static float MaxAxis(this Vector3 vt3)
        {
            return Mathf.Max(vt3.x, vt3.y, vt3.z);
        }

        /// <summary>
        /// 判断是否所有分量都有效
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static bool Avaliable(this Vector3 vec)
        {
            return !vec.x.IsNaN() && !vec.y.IsNaN() && !vec.z.IsNaN();//任意一个值都不为NaN
        }

        public static Vector3 RandomRange(this Vector3 range)
        {
            return new Vector3(RandomRange(range.x), RandomRange(range.y), RandomRange(range.z));
        }

        static float RandomRange(float range)
        {
            return UnityEngine.Random.Range(-range, range);
        }

        public static Vector3 Multi(this Vector3 vt3, Vector3 scaler)
        {
            vt3.Scale(scaler);
            return vt3;
        }

        public static Vector3 Divide(this Vector3 vt3, Vector3 scaler)
        {
            return new Vector3(vt3.x / scaler.x, vt3.y / scaler.y, vt3.z / scaler.z);
        }

        public static bool IsNaN(this float value)
        {
            //https://csharp.2000things.com/2010/09/04/79-equality-checks-for-nan/
            return float.IsNaN(value);
        }

        public static float Size(this Vector3 vt3)
        {
            return vt3.x * vt3.y * vt3.z;
        }

        /// <summary>
        /// 计算vt3Source到vt3Target的插值，适用于更改单一轴向
        /// </summary>
        /// <param name="vt3Source"></param>
        /// <param name="vt3Target"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Lerp(this Vector3 vt3Source, Vector3 vt3Target, Vector3 t)
        {
            return new Vector3(
               Mathf.Lerp(vt3Source.x, vt3Target.x, t.x),
               Mathf.Lerp(vt3Source.y, vt3Target.y, t.y),
               Mathf.Lerp(vt3Source.z, vt3Target.z, t.z)
                                              );
        }

        #endregion

        #region Angle

        /// <summary>
        /// 将角度变为矢量
        /// 
        /// Ref: https://forum.unity.com/threads/how-to-convert-eulerangles-to-forward-direction.1178980/#post-7549813
        /// <paramref name="axis"/>需要转换的轴向<paramref name="axis"/>
        /// </summary>
        public static Vector3 AngleToVector(this Vector3 angel, Vector3? axis = null)
        {
            if (axis == null)
            {
                axis = Vector3.forward;//默认为Forward
            }
            return Quaternion.Euler(angel).ToVector(axis.Value);
        }

        /// <summary>
        /// 将大于180度角进行以负数形式输出，保证角度的数据在统一范围[-180,180]内
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 GetStandardAngle(this Vector3 value)
        {
            return new Vector3(value.x.GetStandardAngle(), value.y.GetStandardAngle(), value.z.GetStandardAngle());
        }


        /// <summary>
        /// 计算两个向量的角度，取值范围为[0，360]
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <param name="axis">A vector around which the other vectors are rotated. 角度对应的轴</param>
        /// <returns></returns>
        public static float DeltaAngle360(this Vector3 from, Vector3 to, Vector3 axis)
        {
            float signedAngle = Vector3.SignedAngle(from, to, axis);//The angle returned will always be between -180 and 180 degrees, because the method returns the smallest angle between the vectors. That is, it will never return a reflex angle.

            //确保值在[0，360]区间
            if (signedAngle < 0)
                signedAngle += 360;
            return signedAngle;
        }

        /// <summary>
        /// 计算两个角度的增量，避免越界
        /// </summary>
        /// <param name="nextAngle"></param>
        /// <param name="lastAngle"></param>
        /// <returns></returns>
        public static Vector3 DeltaAngle(this Vector3 nextAngle, Vector3 lastAngle)
        {
            nextAngle.x = nextAngle.x.DeltaAngle(lastAngle.x);
            nextAngle.y = nextAngle.y.DeltaAngle(lastAngle.y);
            nextAngle.z = nextAngle.z.DeltaAngle(lastAngle.z);
            return nextAngle;
        }

        /// <summary>
        /// 将角度限制在指定范围内
        /// </summary>
        /// <param name="rawNewValue">新值</param>
        /// <param name="originValue">原点值，可以设置为初始值</param>
        /// <param name="rangeValue">范围值（可用其限制轴向）</param>
        /// <param name="validAxid"></param>
        /// <returns></returns>
        public static Vector3 ClampAngle(this Vector3 rawNewValue, Vector3 originValue, Vector3 rangeValue)
        {
            //转为统一单位
            rawNewValue = rawNewValue.GetStandardAngle();
            Vector3 maxRot = (originValue + rangeValue).GetStandardAngle();
            Vector3 minRot = (originValue - rangeValue).GetStandardAngle();
            return rawNewValue.ClampRange(minRot, maxRot);
        }

        /// <summary>
        /// 将Vector3限制在指定范围内
        /// </summary>
        /// <param name="rawNewValue">新值</param>
        /// <param name="originValue">原点值，可以设置为初始值</param>
        /// <param name="rangeValue">范围值（可用其限制轴向）</param>
        /// <param name="validAxid"></param>
        /// <returns></returns>
        public static Vector3 ClampRange(this Vector3 rawNewValue, Vector3 minValue, Vector3 maxValue)
        {
            //转为统一单位
            rawNewValue.x = Mathf.Clamp(rawNewValue.x, minValue.x, maxValue.x);
            rawNewValue.y = Mathf.Clamp(rawNewValue.y, minValue.y, maxValue.y);
            rawNewValue.z = Mathf.Clamp(rawNewValue.z, minValue.z, maxValue.z);
            return rawNewValue;
        }

        /// <summary>
        /// 将大于180度角进行以负数形式输出，保证角度的数据在统一范围[-180,180]内
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetStandardAngle(this float value)
        {
            float angle = value - 180;

            if (angle > 0)
            {
                return angle - 180;
            }

            if (value == 0)
            {
                return 0;
            }

            return angle + 180;
        }
        /// <summary>
        /// 计算两个角度的增量，避免越界
        /// </summary>
        /// <param name="nextAngle"></param>
        /// <param name="lastAngle"></param>
        /// <returns></returns>
        public static float DeltaAngle(this float nextAngle, float lastAngle)
        {
            float sign = Mathf.Sign(nextAngle - lastAngle);

            float deltaAngle = Mathf.Abs(nextAngle - lastAngle);

            //越过0度分界点 
            if (deltaAngle > 180)
            {
                deltaAngle = 360 - deltaAngle;
                sign *= -1;
            }

            return sign * deltaAngle;
        }

        #endregion

        #region Quaternion

        /// <summary>
        /// 将角度变为矢量
        /// </summary>
        /// <param name="quat"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static Vector3 ToVector(this Quaternion quat, Vector3? axis = null)
        {
            if (axis == null)
            {
                axis = Vector3.forward;//默认为Forward
            }
            //原理：将指定轴如（Forward）旋转对应角度angels，即可得到目标的轴向
            return quat * axis.Value;
        }
        /// <summary>
        /// 获取相对于自身坐标系的初始旋转
        /// 
        /// 常见用途：
        /// -自身物体有一定偏转角度，但是需要计算子物体在自身坐标系的初始旋转角度等
        /// 
        /// Warning：
        /// -不能使用tf.rotation代替，因为会因为自身旋转而导致出错！
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion GetSelfCoordinateSystemInitRotation(this Transform tf)
        {
            if (!tf)
                return Quaternion.identity;

            return Quaternion.LookRotation(tf.TransformDirection(Vector3.forward), tf.TransformDirection(Vector3.up));//基于自身坐标系的forward及up，计算出初始旋转值
        }

        //public static Quaternion Rotate(this Quaternion start, Vector3 angle)
        //{
        //    return start * Quaternion.Euler(angle);
        //}
        /// <summary>
        /// 围绕该start旋转值的指定轴，旋转某个角度（局部坐标）
        /// </summary>
        /// <param name="start"></param>
        /// <param name="axis">【Space.Self模式】旋转的轴向，会自动映射到自身坐标系，如Vector3.right</param>
        /// <param name="angle">旋转值</param>
        /// <returns></returns>
        public static Quaternion RotateAround(this Quaternion start, Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            //世界坐标与局部坐标旋转值的区别：https://forum.unity.com/threads/understanding-rotations-in-local-and-world-space-quaternions.153330/#post-1051063

            //Rotate around a local axis: rotation = rotation * Quaternion.AngleAxis(10, Vector3.Up);
            //Rotate around a world axis: rotation = Quaternion.AngleAxis(10, Vector3.Up) * rotation;
            //So, as you can see above, putting the desired rotation last rotates around a local axis, putting it first rotates around a world axis. There is just one really simple rule you need to memorize: Order matters.

            if (relativeTo == Space.Self)
                return start * Quaternion.AngleAxis(angle, axis);
            else
                return Quaternion.AngleAxis(angle, axis) * start;

            //if (angle >= 180)
            //    Debug.LogError("旋转角度大于180！");
            //Vector3 rotateValue = Vector3.one * angle;
            //rotateValue.Scale(axis);//仅对应指定轴有效
            //return start * Quaternion.Euler(rotateValue);
        }

        /// <summary>
        /// 将局部旋转转变为全局旋转
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tfParent"></param>
        /// <returns></returns>
        public static Quaternion ToWorld(this Quaternion source, Transform target)
        {
            if (target.parent)
                return target.parent.rotation * source;//在父物体的旋转基础上，乘以该物体的旋转值，就能得到世界旋转值
            return source;
        }
        /// <summary>
        /// 将全局旋转变为局部旋转
        /// 
        /// Ref: https://forum.unity.com/threads/convert-world-space-rotation-to-local-space-rotation.332025/#post-2150730
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Quaternion ToLocal(this Quaternion source, Transform target)
        {
            if (target.parent == null)
                return source;
            return Quaternion.Inverse(target.parent.rotation) * source;//如果有父物体，则基于父物体进行计算
        }
        #endregion

        #region UnityEvent

        public static void InvokeIfHasPersistent(this UnityEvent unityEvent)
        {
            if (unityEvent.GetPersistentEventCount() > 0)
                unityEvent.Invoke();
        }

        public static void InvokeIfAnyPersistent(this FloatEvent unityEvent, float value)
        {
            if (unityEvent.GetPersistentEventCount() > 0)
                unityEvent.Invoke(value);
        }

        public static void Execute(this UnityEvent unityEvent)
        {
            if (unityEvent != null)
                unityEvent.Invoke();
        }

        public static void Execute<T1>(this UnityEvent<T1> unityEvent, T1 value1)
        {
            if (unityEvent != null)
                unityEvent.Invoke(value1);
        }
        public static void TryExecute(this UnityEvent unityEvent)
        {
            try
            {
                if (unityEvent != null)
                    unityEvent.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void TryExecute<T1>(this UnityEvent<T1> unityEvent, T1 value1)
        {
            try
            {
                if (unityEvent != null)
                    unityEvent.Invoke(value1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        #region UnityAction
        public static void TryExecute(this UnityAction action)
        {
            try
            {
                action.Execute();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1>(this UnityAction<T1> action, T1 value1)
        {
            try
            {
                action.Execute(value1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2>(this UnityAction<T1, T2> action, T1 value1, T2 value2)
        {
            try
            {
                action.Execute(value1, value2);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2, T3>(this UnityAction<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
        {
            try
            {
                action.Execute(value1, value2, value3);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2, T3, T4>(this UnityAction<T1, T2, T3, T4> action, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            try
            {
                action.Execute(value1, value2, value3, value4);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void Execute(this UnityAction action)
        {
            if (action != null)
                action();
        }
        public static void Execute<T1>(this UnityAction<T1> action, T1 value1)
        {
            if (action != null)
                action(value1);
        }

        public static void Execute<T1, T2>(this UnityAction<T1, T2> action, T1 value1, T2 value2)
        {
            if (action != null)
                action(value1, value2);
        }

        public static void Execute<T1, T2, T3>(this UnityAction<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
        {
            if (action != null)
                action(value1, value2, value3);
        }
        public static void Execute<T1, T2, T3, T4>(this UnityAction<T1, T2, T3, T4> action, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (action != null)
                action(value1, value2, value3, value4);
        }
        #endregion

        #region Func

        public static TResult Execute<TResult>(this Func<TResult> action)
        {
            if (action != null)
                return action();
            return default;
        }

        #endregion

        #region Action

        public static void Execute(this Action action)
        {
            if (action != null)
                action();
        }
        public static void Execute<T1>(this Action<T1> action, T1 value1)
        {
            if (action != null)
                action(value1);
        }

        public static void Execute<T1, T2>(this Action<T1, T2> action, T1 value1, T2 value2)
        {
            if (action != null)
                action(value1, value2);
        }
        public static void Execute<T1, T2, T3>(this Action<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
        {
            if (action != null)
                action(value1, value2, value3);
        }
        public static void Execute<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (action != null)
                action(value1, value2, value3, value4);
        }
        public static void Execute<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            if (action != null)
                action(value1, value2, value3, value4, value5);
        }
        public static void TryExecute(this Action action)
        {
            try
            {
                action.Execute();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1>(this Action<T1> action, T1 value1)
        {
            try
            {
                action.Execute(value1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2>(this Action<T1, T2> action, T1 value1, T2 value2)
        {
            try
            {
                action.Execute(value1, value2);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2, T3>(this Action<T1, T2, T3> action, T1 value1, T2 value2, T3 value3)
        {
            try
            {
                action.Execute(value1, value2, value3);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        public static void TryExecute<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            try
            {
                action.Execute(value1, value2, value3, value4);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion

        #region Component

        /// <summary>
        /// 对单层子物体
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="action"></param>
        /// <param name="includeSelf"></param>
        public static void ForEachChild(this Component comp, UnityAction<Component> action, bool includeSelf = true)
        {
            if (includeSelf && comp != null)
                action(comp);

            foreach (Transform tfChild in comp.transform)
            {
                action(tfChild);
            }
        }

        public static void ForEachChildTransform(this Transform tf, UnityAction<Transform> action, bool includeSelf = true, bool isRecursive = false)
        {
            if (includeSelf)
                action(tf);

            foreach (Transform tfChild in tf)
            {
                //Transform是每个物体都有的，所以可以用Recursive
                if (isRecursive)
                {
                    tfChild.ForEachChildTransform(action, true, true);
                }
                else
                {
                    action(tfChild);
                }
            }
        }

        public static void ForEachParent(this Component comp, UnityAction<Component> action, bool includeSelf = true)
        {
            Transform target = comp.transform;
            if (includeSelf)
                action(comp);

            while (target.parent)
            {
                action(target.parent);
                target = target.parent;
            }
        }

        /// <summary>
        /// 递归遍历(Transform)
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="action"></param>
        /// <param name="includeSelf"></param>
        public static void Recursive(this Component comp, UnityAction<Component> action, bool includeSelf = true)
        {
            AlgorithmTool.Recursive(comp, (ele) => ele.transform, action, includeSelf);
        }


        /// <summary>
        /// 递归遍历
        /// </summary>
        public static void Recursive<T>(this T em, Func<T, IEnumerable> getEnum, UnityAction<T> action, bool includeSelf = true, int? maxDepth = null)
        {
            AlgorithmTool.Recursive(em, getEnum, action, includeSelf, maxDepth);
        }
        /// <summary>
        /// 递归遍历
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="action"></param>
        /// <param name="maxDepth">最高的层数</param>
        /// <param name="includeSelf"></param>
        public static void Recursive<T>(this T em, UnityAction<T> action, Func<T, IEnumerable> getEnum, bool includeSelf, int maxDepth)
        {
            AlgorithmTool.Recursive(em, getEnum, action, includeSelf, maxDepth);
        }

        #endregion

        #region GameObject


        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// 
        /// Ref: https://discussions.unity.com/t/how-to-detect-if-a-gameobject-has-been-destroyed/2385/4
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }

        public static T InstantiateUIPrefab<T>(this GameObject obj, Transform tfParent = null, Vector3 position = default, Quaternion rotation = default)
        {
            return obj.InstantiatePrefab(tfParent, position, rotation, Vector3.one).GetComponent<T>();//默认使用缩放1
        }
        public static T InstantiatePrefab<T>(this GameObject obj, Transform tfParent = null, Vector3 position = default, Quaternion rotation = default, Vector3? scale = null)
        {
            return obj.InstantiatePrefab(tfParent, position, rotation, scale).GetComponent<T>();
        }

        /// <summary>
        /// 生成一个保存预制物引用的新物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static GameObject InstantiatePrefab(this GameObject obj, Transform tfParent = null, Vector3 position = default, Quaternion rotation = default, Vector3? scale = null)
        {

            GameObject goInst = UnityEngine.Object.Instantiate(obj);
            goInst.SetupInstantiate(tfParent, position, rotation, scale);
            return goInst;
        }

        public static void SetupInstantiate(this GameObject goInst, Transform tfParent = null, Vector3 position = default, Quaternion rotation = default, Vector3? scale = null)
        {
            Transform tfInst = goInst.transform;
            if (tfParent)
            {
                tfInst.SetParent(tfParent, false);//scale prefab to UI scale 

                tfInst.SetProperty(position, rotation);
                //tfInst.localPosition = position;
                //tfInst.localRotation = rotation;
            }
            else
            {
                tfInst.SetProperty(position, rotation, isLocalSpace: false);
                //tfInst.position = position;
                //tfInst.rotation = rotation;
            }

            //PS:默认不修改缩放，除非明确指定
            if (scale.HasValue)
                tfInst.localScale = scale.Value;
        }

        /// <summary>
        /// Mainly for UI, which will keep scale as one
        /// </summary>
        /// <param name="goInst"></param>
        /// <param name="tfParent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public static void SetupUIInstantiate(this GameObject goInst, Transform tfParent = null, Vector3 position = default, Quaternion rotation = default, Vector3? scale = null)
        {
            Transform tfInst = goInst.transform;
            if (tfParent)
            {
                tfInst.SetParent(tfParent, false);//scale prefab to UI scale 
                tfInst.localPosition = position;
                tfInst.localRotation = rotation;
                tfInst.localScale = scale.HasValue ? scale.Value : Vector3.one;//UI必须设置缩放，否则可能会残留预制物的缩放
            }
            else
            {
                tfInst.position = position;
                tfInst.rotation = rotation;
            }
        }
        /// <summary>
        /// 销毁游戏对象，同时避免Destroy延迟销毁影响后续子物体层级查询
        /// </summary>
        /// <param name="tf"></param>
        public static void DestroyAtOnce(this GameObject go)
        {
            if (go.transform.parent)
                go.transform.SetParent(null);

            go.transform.Destroy();
        }
        #endregion

        #region Transform

        /// <summary>
        /// 获取层级的名称，通常用于Transform.Find
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="rootParent">可选的父物体，常用于相同层级迁移</param>
        /// <returns></returns>
        public static string GetHierarchyName(this Transform tf, Transform rootParent = null)
        {
            string result = tf.name;
            while (tf.parent)
            {
                if (rootParent && tf.parent == rootParent) break;//当遇到父物体时体哦啊出

                result = tf.parent.name + "/" + result;
                tf = tf.parent;
            }
            return result;
        }

        /// <summary>
        /// 根据物体是否为刚体，自动调用对应的移动方法，确保能正确移动到指定位置
        /// 
        /// 用途:
        /// -针对rigidbody.interpolation等计算帧差的物体
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public static void SetProperty(this Transform tf, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null, bool isLocalSpace = true)
        {
            /////【ToTest】以下修改Rigidbody的方法需要详细测试
            ///#1 Cache
            ///PS:
            /// -如果物体有刚体且Interpolate不为None，则需要先设置为None再移动，否则会其导致移动不到正常位置，因为该选项是让物体基于前几帧进行计算（https://forum.unity.com/threads/rigidbody2d-position-doesnt-teleport-the-object-with-the-interpolation-settings-on.1172240/）
            Rigidbody rigidbody = tf.GetComponent<Rigidbody>();
            bool cacheIsKinematic = false;
            RigidbodyInterpolation cacheRigidbodyInterpolation = RigidbodyInterpolation.None;
            bool shouldChangeRigdibody = false;
            if (rigidbody)
            {
                if (rigidbody.interpolation != RigidbodyInterpolation.None)
                {
                    shouldChangeRigdibody = true;
                    cacheIsKinematic = rigidbody.isKinematic;
                    cacheRigidbodyInterpolation = rigidbody.interpolation;
                    rigidbody.interpolation = RigidbodyInterpolation.None;
                    rigidbody.isKinematic = true;
                    rigidbody.gameObject.SetActive(false);//最重要的起作用代码（可能会导致物体的OnEnable/Disable被多次调用）
                }
            }

            //#2 Set
            if (position.HasValue)
            {
                if (isLocalSpace)
                    tf.localPosition = position.Value;
                else
                    tf.position = position.Value;
            }
            if (rotation.HasValue)
            {
                if (isLocalSpace)
                    tf.localRotation = rotation.Value;
                else
                    tf.rotation = rotation.Value;
            }
            if (scale.HasValue)
                tf.localScale = scale.Value;//PS:缩放暂无全局可设置字段

            //#3 Restore
            if (rigidbody && shouldChangeRigdibody)
            {
                if (shouldChangeRigdibody)
                {
                    rigidbody.isKinematic = cacheIsKinematic;
                    rigidbody.interpolation = cacheRigidbodyInterpolation;
                    rigidbody.gameObject.SetActive(true);
                }

                //【ToTest】重置矢量，避免因为缩放等导致其有初始速度
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }


        /// <summary>
        /// PS:名称不能加/！！！否则会出现找不到子物体的问题 file:///C:/Program%20Files_Custom/Unity%20Group/Unity2018.2.10/Editor/Data/Documentation/en/ScriptReference/Transform.Find.html
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="name"></param>
        /// <param name="isInitTranform"></param>
        /// <returns></returns>
        public static Transform AddChildOnce(this Transform tf, string name, bool isInitTranform = true)
        {
            Transform target = null;
            if (tf)
            {
                target = tf.Find(name);
            }

            if (!target)
            {
                GameObject go = new GameObject(name);
                target = go.transform;
                target.SetParent(tf);
                if (isInitTranform)
                {
                    target.localPosition = default;
                    target.localScale = Vector3.one;
                    target.localEulerAngles = default;
                }
            }
            return target;
        }

        public static void DestroyAllChild(this Transform tf)
        {
            // Also note that you should never iterate through arrays and destroy the elements you are iterating over. This will cause serious problems (as a general programming practice, not just in Unity).(https://docs.unity3d.com/ScriptReference/Object.DestroyImmediate.html)
            //Destroy的问题：Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
            //因此要先把子物体移到外面
            while (tf.childCount > 0)
            {
                Transform tfSon = tf.GetChild(0);
                tfSon.SetParent(null);
                tfSon.Destroy();//调用本类中的扩展方法，根据程序是否运行调用对应方法
            }
        }

        /// <summary>
        /// 销毁组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public static void DestroyComponent<T>(T component) where T : Component
        {
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(component);
            else
                UnityEngine.Object.DestroyImmediate(component);
        }

        /// <summary>
        /// 销毁游戏对象
        /// </summary>
        /// <param name="tf"></param>
        public static void Destroy(this Transform tf)
        {
            if (Application.isPlaying)
                UnityEngine.Object.Destroy(tf.gameObject);
            else
                UnityEngine.Object.DestroyImmediate(tf.gameObject);
        }


        [Obsolete]
        public static Color GetColor(this Transform tf, string colorName = "_Color")
        {
            return tf.GetMat().GetColor(colorName);
        }

        [Obsolete]
        public static void SetColor(this Transform tf, Color color, string colorName = "_Color")
        {
            tf.GetMat().SetColor(colorName, color);
        }
        [Obsolete]
        public static Material GetMat(this Transform tf)
        {
            if (tf.GetComponent<MeshRenderer>() != null)
            {
                MeshRenderer render = tf.GetComponent<MeshRenderer>();

                if (Application.isEditor)
                    return render.material;
                else
                    return render.sharedMaterial;
            }
            return null;
        }
        [Obsolete]
        public static bool HasRender(this Transform tf)
        {
            return tf.GetComponent<MeshRenderer>();
        }
        #endregion

        #region RectTransform

        /// <summary>
        /// 获取target相对应的鼠标位置，
        /// </summary>
        /// <param name="target"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector3 GetMousePos(this Transform target, Camera camera = null)
        {
            if (!camera)
                camera = Camera.main;
            Vector3 distanceCamToTarget = target.position - camera.transform.position;//计算与相机的偏移值，适用于透视模式的相机
            Vector3 mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceCamToTarget.z));
            mousePos.z = target.position.z;
            return mousePos;
        }

        #endregion

        #region Attribute

        //Enum

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        //Common

        //public static TAttribute GetCustomAttributes<TField,TContainer,TAttribute>(this TField filed)
        //{
        //    Type typeContainer = typeof(TContainer);
        //    typeContainer.GetField()
        //}

        /// <summary>
        /// 获取实例中某个字段的Attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="objectType">实例类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Type objectType, string fieldName)
            where TAttribute : class
        {
            object attribute = null;
            if (string.IsNullOrEmpty(fieldName))//fieldName为空：直接检查object是否包含该Attribute
            {
                attribute = objectType.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
            }
            else
            {
                var propertyInfo = objectType.GetProperty(fieldName);
                if (propertyInfo != null)
                {
                    attribute = propertyInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
                }

                var fieldInfo = objectType.GetField(fieldName);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault();
                }
            }
            return attribute as TAttribute;
        }

        /// <summary>
        /// Cache Attribute Data
        /// </summary>
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<TType, TAttribute>(this Type sourceType, Func<TAttribute, string> attributeValueAction) where TAttribute : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, null);
        }

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <param name="name">field name或property name</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<TType, TAttribute>(this Type sourceType, Func<TAttribute, string> attributeValueAction,
            string name) where TAttribute : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, name);
        }

        private static string GetAttributeValue<TAttribute>(Type sourceType, Func<TAttribute, string> attributeValueAction,
            string name) where TAttribute : Attribute
        {
            var key = BuildKey(sourceType, name);
            if (!Cache.ContainsKey(key))
            {
                CacheAttributeValue(sourceType, attributeValueAction, name);
            }

            return Cache[key];
        }

        /// <summary>
        /// 缓存Attribute Value
        /// </summary>
        private static void CacheAttributeValue<T>(Type type,
            Func<T, string> attributeValueAction, string name)
        {
            var key = BuildKey(type, name);
            var value = GetValue(type, attributeValueAction, name);
            lock (key + "_attributeValueLockKey")
            {
                if (!Cache.ContainsKey(key))
                {
                    Cache[key] = value;
                }
            }
        }

        private static string GetValue<T>(Type type,
            Func<T, string> attributeValueAction, string name)
        {
            object attribute = null;
            if (string.IsNullOrEmpty(name))
            {
                attribute =
                    type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            }
            else
            {
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    attribute =
                        propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }

                var fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }
            }

            return attribute == null ? null : attributeValueAction((T)attribute);
        }

        /// <summary>
        /// 缓存Collection Name Key
        /// </summary>
        private static string BuildKey(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return type.FullName;
            }

            return type.FullName + "." + name;
        }

        #endregion

        #region FileStream

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name=“fs“>文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }
        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name=“data“></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        #endregion

        #region Component/Interface

        /// <summary>
        /// Wall Through all the component in given Scene (include inactive object), it may spend huge time 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        /// <param name="func"></param>
        /// <param name="isIncludeHide"></param>
        /// <param name="includeSelf"></param>
        public static void ForEachSceneComponent<T>(this Component comp, UnityAction<T> func, bool isIncludeHide = true, bool includeSelf = true, Scene? scene = null)
        {

            Scene sceneTarget = scene.HasValue ? scene.Value : SceneManager.GetActiveScene();
            if (!sceneTarget.IsValid())
            {
                Debug.LogError("The scene" + sceneTarget + " is not valid!");
                return;
            }

            foreach (GameObject go in sceneTarget.GetRootGameObjects())
            {
                go.transform.ForEachChildComponent(func, isIncludeHide, includeSelf, true);
            }
        }

        public static void ForEachSiblingComponent<T>(this Component comp, UnityAction<T> func, bool isIncludeHide = true, bool includeSelf = true)
        {
            if (comp.transform.parent != null)
            {
                comp.transform.parent.ForEachChildComponent(func, isIncludeHide, includeSelf, false);//不需要递归，只需要针对同级物体
            }
        }

        public static void ForEachParentComponent<T>(this Component comp, UnityAction<T> func, bool isIncludeHide = true, bool includeSelf = true)
        {
            UnityAction<Component> targetFunc = (tf) =>
            {
                if (isIncludeHide || !isIncludeHide && tf.gameObject.activeInHierarchy)
                    tf.ForEachSelfComponent(func);
            };

            comp.ForEachParent(targetFunc, includeSelf);
        }
        /// <summary>
        /// Set every component on child
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        /// <param name="func"></param>
        /// <param name="IsIncludeHide"></param>
        /// <param name="IsRecursive"></param>
        public static void ForEachChildComponent<T>(this Component comp, UnityAction<T> func, bool isIncludeHide = true, bool includeSelf = true, bool isRecursive = true)
        {
            UnityAction<Component> sonFunc =
            (c) =>
            {
                if (isIncludeHide || !isIncludeHide && c.gameObject.activeInHierarchy)
                    c.ForEachSelfComponent(func);
            };

            if (isRecursive)
                comp.Recursive(sonFunc, includeSelf);
            else
                comp.ForEachChild(sonFunc, includeSelf);
        }


        /// <summary>
        /// For all of self compoents, In case some GameObject contains multi Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transform"></param>
        /// <param name="func"></param>
        public static void ForEachSelfComponent<T>(this Component comp, UnityAction<T> func)
        {
            foreach (T t in comp.GetComponents<T>())//In case some GameObject contains multi Component
            {
                if (t != null)
                    func.Execute(t);
            }
        }

        public static T AddComponentOnce<T>(this Component com) where T : Component
        {
            if (com && com.gameObject)
            {
                return com.gameObject.AddComponentOnce<T>();
            }
            else
            {
                Debug.LogError("组件为空！");
            }
            return null;
        }

        public static T AddComponentOnce<T>(this GameObject go) where T : Component
        {
            if (go.GetComponent<T>() != null)
                return go.GetComponent<T>();
            else
                return go.gameObject.AddComponent<T>();
        }

        public static Component AddComponentOnce(this GameObject tf, Type componentType)
        {
            if (tf.GetComponent(componentType) != null)
                return tf.GetComponent(componentType);
            else
                return tf.gameObject.AddComponent(componentType);
        }

        public static void RemoveComponentOnce<T>(this GameObject tf) where T : Component
        {
            T component = tf.GetComponent<T>();
            if (component != null)
            {
                DestroyComponent(component);
            }
        }

        public static void RemoveComponentOnce(this GameObject tf, Type componentType)
        {
            Component component = tf.GetComponent(componentType);
            if (component != null)
            {
                DestroyComponent(component);
            }
        }


        public static TReturn FindFirstComponentInParent<TReturn>(this Component comp, bool includeSelf = true)
            where TReturn : class//Component
        {
            TReturn tReturn = null;
            comp.transform.ForEachParent(
               (tf) =>
               {
                   if (tReturn == null)
                       tf.TryGetComponent(out tReturn);//Warning:不要直接使用GetComponent，因为如果TReturn为Component，其内部会通过CastHelper<T>返回对应类型实例的Default值而不是null而导致意外赋值给tReturn，这样会导致后续的(tReturn == null)无法进入！
               },
               includeSelf
               );
            return tReturn;
        }

        /// <summary>
        /// 找第一个同名的物体
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="name"></param>
        /// <param name="includeSelf"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public static Transform FindFirstChild(this Component comp, string name, bool includeSelf = true, bool isRecursive = false)
        {
            Transform tReturn = null;
            comp.transform.ForEachChildTransform(
               (tf) =>
               {
                   if (!tReturn)
                   {
                       if (tf.name == name)
                           tReturn = tf;
                   }
               },
               includeSelf,
               isRecursive
               );
            return tReturn;
        }

        public static List<TReturn> FindComponentsInChild<TReturn>(this Component comp, bool includeSelf = true, bool isRecursive = false, Predicate<TReturn> match = null)
        {
            List<TReturn> listResult = new List<TReturn>();
            comp.transform.ForEachChildTransform(
               (tf) =>
               {
                   TReturn cacheComp = tf.GetComponent<TReturn>();
                   if (tf.TryGetComponent(out cacheComp))
                   {
                       if (match == null || match != null && match(cacheComp))
                           listResult.Add(cacheComp);
                   }
               }, includeSelf, isRecursive);
            return listResult;
        }

        //通过名字及match查找
        public static TReturn FindFirstComponentInChild<TReturn>(this Component comp, string name, bool includeSelf = true, bool isRecursive = false, Predicate<TReturn> match = null)
    where TReturn : Component
        {
            TReturn tReturn = null;
            comp.transform.ForEachChildTransform(
               (tf) =>
               {
                   if (!tReturn)
                   {
                       if (tf.name == name)
                       {
                           TReturn cacheComp = tf.GetComponent<TReturn>();
                           if (cacheComp)
                               if (match == null || match != null && match(cacheComp))
                                   tReturn = cacheComp;
                       }
                   }
               }, includeSelf, isRecursive);
            return tReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="comp"></param>
        /// <param name="includeSelf"></param>
        /// <param name="isRecursive"></param>
        /// <param name="match">对返回组件进行检测</param>
        /// <returns></returns>
        public static TReturn FindFirstComponentInChild<TReturn>(this Component comp, bool includeSelf = true, bool isRecursive = false, Predicate<TReturn> match = null)
        where TReturn : Component
        {
            TReturn tReturn = null;
            comp.transform.ForEachChildTransform(
               (tf) =>
               {
                   if (!tReturn)
                   {
                       TReturn cacheComp = tf.GetComponent<TReturn>();
                       if (cacheComp)
                           if (match == null || match != null && match(cacheComp))
                               tReturn = cacheComp;
                   }
               },
               includeSelf,
               isRecursive
               );
            return tReturn;
        }

        /// <summary>
        /// https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
        /// Clone Component 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static T CopyComponent<T>(this T original, T dest) where T : Component
        {
            Type type = original.GetType();

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dest, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dest, prop.GetValue(original, null), null);
            }
            return dest as T;
        }

        #endregion

        #region UnityEvent

#if UNITY_EDITOR

        /// <summary>
        /// Warning: 调用后需要运行EditorUtility.SetDirty
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <param name="targetObj"></param>
        /// <param name="functionName"></param>
        public static void AddVoidPersistentListenerOnce(this UnityEventBase unityEvent, object targetObj, string functionName)
        {
#if UNITY_EDITOR

            Type[] argumentTypes = new Type[] { };
            MethodInfo targetMethodInfo = UnityEventBase.GetValidMethodInfo(targetObj, functionName, argumentTypes);

            for (int i = 0; i != unityEvent.GetPersistentEventCount(); i++)
            {
                object tempObject = unityEvent.GetPersistentTarget(i);
                if (tempObject == targetObj && unityEvent.GetPersistentMethodName(i) == targetMethodInfo.Name)
                {
                    Debug.LogWarning(functionName + " 同名委托！");
                    return;
                }
            }
            //bug:
            if (targetMethodInfo.NotNull())
            {
                UnityAction methodDelegate = Delegate.CreateDelegate(typeof(UnityAction), targetObj, targetMethodInfo) as UnityAction;
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(unityEvent, methodDelegate);
            }
            else
            {
                Debug.LogError("无指定方法！");
            }

#endif
        }

        /// <summary>
        /// 增加带参的Listener
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityEvent"></param>
        /// <param name="targetObj"></param>
        /// <param name="methodOrPropertyName"></param>
        /// <param name="argumentTypes"></param>
        /// <returns>是否增加事件成功</returns>
        public static bool AddPersistentListenerOnce<T>(this UnityEvent<T> unityEvent, object targetObj, string methodOrPropertyName, bool isProperty = false)
        {
#if UNITY_EDITOR
            try
            {

                Type[] argumentTypes = new Type[] { typeof(T) };
                MethodInfo targetMethodInfo = null;
                if (isProperty)
                    targetMethodInfo = targetObj.GetType().GetProperty(methodOrPropertyName).GetSetMethod();
                else
                    targetMethodInfo = UnityEventBase.GetValidMethodInfo(targetObj, methodOrPropertyName, argumentTypes);

                for (int i = 0; i != unityEvent.GetPersistentEventCount(); i++)
                {
                    object tempObject = unityEvent.GetPersistentTarget(i);
                    if (tempObject == targetObj && unityEvent.GetPersistentMethodName(i) == targetMethodInfo.Name)
                    {
                        Debug.LogWarning(targetMethodInfo.Name + "为同名监听！不再重复添加");
                        return true;
                    }
                }

                if (targetMethodInfo.NotNull())
                {
                    UnityAction<T> methodDelegate = Delegate.CreateDelegate(typeof(UnityAction<T>), targetObj, targetMethodInfo) as UnityAction<T>;
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, methodDelegate);
                }
                else
                {
                    Debug.LogError("无指定方法！");
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
#endif
            return true;
        }
#endif

        #endregion

        #region TimeSpan

        public static double TimeSpanToValue(this TimeSpan span)
        {
            return span.TotalSeconds;
        }

        public static string FormatTime(this TimeSpan timeSpan, string customFormat = @"mm\:ss")
        {
            DateTime dateTime = new DateTime(timeSpan.Ticks);
            return dateTime.ToString(customFormat);
        }


        /// <summary>
        /// 将十进制的秒转换为DateTime格式
        /// </summary>
        public static string DateTimeFormat(this double seconds, string format = "c")
        {
            //Warning: .Net旧版本没有ToString(format)的方法(Support for formatting TimeSpan values was added in the .NET Framework 4. )(https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tostring?redirectedfrom=MSDN&view=net-5.0#System_TimeSpan_ToString_System_String_)
            return TimeSpan.FromSeconds(Math.Round(seconds)).ToString(/*format*/);
        }

        #endregion

        #region DateTime

        private static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);//起始日期

        public static double DateToValue(this DateTime dateTime)
        {
            return (dateTime - Epoch).TotalSeconds;
        }

        public static string DateToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("t");
        }

        public static string DateToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("d");
        }
        public static string DateToDateTimeString(this DateTime dateTime, string customFormat)
        {
            if (customFormat != null)
            {
                try
                {
                    return dateTime.ToString(customFormat);
                }
                catch
                {
                }
            }
            return string.Format("{0}{1}{2}", dateTime.ToString("d"), Environment.NewLine, dateTime.ToString("t"));
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format">
        /// yyyy-MM-dd HH:mm:ss——适用于Editor编辑、DateTime.TryParse/TryDeSerializeDateTime转换
        /// yyyyMMdd_HHmmss    ——适用于文件名后缀
        /// 
        /// </param>
        /// <returns></returns>
        public static string Serialize(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            try
            {
                return dateTime.ToString(format);
            }
            catch (Exception e)
            {
                Debug.LogError(e + "\r\n" + dateTime.ToString() + " " + format);
                return "";
            }
        }

        /// <summary>
        /// 转回DateTime类型
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="resultDateTime"></param>
        /// <returns></returns>
        public static bool TryDeSerializeDateTime(this string strDateTime, out DateTime resultDateTime)
        {
            if (DateTime.TryParse(strDateTime, out resultDateTime))
            {
                return true;
            }
            else
            {
                Debug.LogError("[" + strDateTime + "] 的日期转换失败！");
                return false;
            }
        }

        //——Convert from other struct——

        /// <summary>
        /// 时间戳转为DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(double value)
        {
            return Epoch.AddSeconds(value);
        }
        #endregion
    }
}