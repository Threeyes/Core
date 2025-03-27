using UnityEngine;
using System.Collections.Generic;
namespace Threeyes.Core
{
    public static class LazyExtension_Android
    {
        #region  Utility
        public static string GetTypeName(this AndroidJavaObject androidJavaObject)
        {
            if (androidJavaObject != null)
            {
                // 调用 getClass 方法获取 Class 对象
                AndroidJavaObject classObj = androidJavaObject.Call<AndroidJavaObject>("getClass");

                if (classObj != null)
                {
                    // 调用 getName 方法获取类的名称
                    string className = classObj.Call<string>("getName");
                    return className;
                }
            }
            return "";
        }


        /// <summary>
        /// 调用返回List<>的方法
        /// </summary>
        /// <param name="androidJavaObject"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static List<AndroidJavaObject> GetList(this AndroidJavaObject androidJavaObject, string methodName, params object[] args)
        {
            List<AndroidJavaObject> listElement = new List<AndroidJavaObject>();

            AndroidJavaObject installedApps = androidJavaObject.Call<AndroidJavaObject>(methodName, args);
            int eleCount = installedApps.Call<int>("size");

            for (int i = 0; i < eleCount; i++)
            {
                AndroidJavaObject element = installedApps.Call<AndroidJavaObject>("get", i);//
                listElement.Add(element);
            }
            return listElement;
        }
        #endregion
    }
}