using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using Threeyes.Core.Editor;
#endif

namespace Threeyes.Core
{

    /// <summary>
    /// ScriptableObject资源单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SOInstanceBase<T> : ScriptableObject
        where T : SOInstanceBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_inst">用于缓存的字段</param>
        /// <param name="pathInResources">//该Manager在Resources下的路径</param>
        /// <param name="name">文件名</param>
        /// <param name="actionOnCreate">创建后的Action</param>
        /// <returns></returns>
        public static T GetOrCreateInstance(ref T _inst, string name, string pathInResources = "", UnityAction<T> actionOnCreate = null)
        {
            //#缓存字段已经有引用，返回（可能是先前调用过）
            if (_inst)
            {
                //Debug.Log("Load From Cache property");
                return _inst;
            }

            //#尝试从Resources文件夹中获取
            if (!_inst)
            {
                string filePathinResources = GetPathInResources(name, pathInResources);
                _inst = ResourceLoad<T>(filePathinResources);
                if (_inst)
                {
                    //Debug.Log("Load From Directory: " + localDirectoryPath + "/" + name);
                    return _inst;
                }
            }

            //#在本地文件夹中创建并获取引用
            if (!_inst)
            {
#if UNITY_EDITOR
                string localDirectoryPath = "Assets/Resources";//Assets/Resources
                if (pathInResources != "")
                    localDirectoryPath += "/" + pathInResources;// Assets/Resources/xxx

                string absDirPath = EditorPathTool.UnityRelateToAbsPath(localDirectoryPath);
                if (!Directory.Exists(absDirPath))
                {
                    //#Editor模式下，通过IO创建文件夹，而不用Unity内置方法AssetDatabase.CreateFolder，因为其一次只能创建一层文件夹）（可能会因为Unity没有更新而报错）（ref：https://forum.unity.com/threads/assetdatabase-create-folders-recursively.787046/）
                    Directory.CreateDirectory(absDirPath);
                    AssetDatabase.Refresh();
                }

                //PS:此时文件夹已经创建，但是Unity未完全刷新，不影响后续操作
                //if (!AssetDatabase.IsValidFolder(localDirectoryPath))
                //{
                //    Debug.LogError("Directory not found! Try to manual refresh the editor !");
                //    return null;
                //}

                _inst = CreateInstance<T>();
                _inst.OnCreate();
                string localFilePath = localDirectoryPath + "/" + name + ".asset";
                actionOnCreate.Execute(_inst);
                AssetDatabase.CreateAsset(_inst, localFilePath);
                AssetDatabase.SaveAssets();
                //Debug.Log("Create " + name + ".asset");
                if (!_inst)
                {
                    Debug.LogError("Can't create instance: \r\n" + localFilePath);
                }
#else
                        Debug.LogError("Can't Create instance at Runtime!");
                        // 如果仍然没有，就从默认状态中创建一个新的
                        // CreateDefaultT函数可以是从JSON文件中读取，并且在实例化完后指明_instance.hideFlags = HideFlags.HideAndDontSave
                        //_instance = CreateDefaultT();
#endif
            }

            return _inst;
        }

        /// <summary>
        /// First Create
        /// </summary>
        protected virtual void OnCreate()
        {

        }

        #region Utility
        protected static string GetPathInResources(string name, string pathInResources)
        {
            string filePathinResources = name;
            if (pathInResources != "")
                filePathinResources = pathInResources + "/" + name;
            return filePathinResources;
        }

        public static T1 ResourceLoad<T1>(string pathInResources)
        where T1 : ScriptableObject
        {
            //便于在运行时获取
            return Resources.Load<T1>(pathInResources);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// Warning:
    /// -不要在标记了[InitializeOnLoad]的静态类中调用该静态类，因为此时Unity尚未加载完资源，容易导致报错（https://docs.unity3d.com/ScriptReference/InitializeOnLoadAttribute.html）。可改为点击菜单才创建该实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInfo"></typeparam>
    public class SOInstanceBase<T, TInfo> : SOInstanceBase<T>
        where T : SOInstanceBase<T, TInfo>
        where TInfo : SOInstacneInfo, new()
    {
        #region Instance
        public static T Instance
        {
            get
            {
                return GetOrCreateInstance(ref instance, info.defaultName, info.pathInResources);
            }
        }
        private static T instance;
        public static readonly TInfo info = new TInfo();

        /// <summary>
        /// Check if Instance Exist in Assets
        /// </summary>
        public static bool InstanceExists
        {
            get
            {
                string filePathinResources = GetPathInResources(info.defaultName, info.pathInResources);
                T inst = ResourceLoad<T>(filePathinResources);
                return inst != null;
            }
        }
        #endregion
    }

    /// <summary>
    /// 自定义默认路径等信息
    /// </summary>
    public abstract class SOInstacneInfo
    {
        public abstract string defaultName { get; }
        public abstract string pathInResources { get; }
    }
}