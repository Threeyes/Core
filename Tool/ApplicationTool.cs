using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Threeyes.Core
{
    public class ApplicationTool : MonoBehaviour
    {
        #region Url

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">email名称，如AA@qq.com</param>
        /// <param name="subject">标题</param>
        /// <param name="body">主题</param>
        public static void OpenEmailUrl(string email, string subject = null, string body = null)
        {
            string content = "mailto:" + email;
            if (subject.NotNullOrEmpty())
            {
                content += "?subject=" + EscapeURL(subject);
            }
            if (body.NotNullOrEmpty())
            {
                content += "&amp;body=" + EscapeURL(body);
            }
            Application.OpenURL(content);
        }
        static string EscapeURL(string URL)
        {
            return UnityEngine.Networking.UnityWebRequest.EscapeURL(URL).Replace("+", "%20");
        }
        #endregion


        #region Quit 
        ///Ref: https://discussions.unity.com/t/testing-application-wantstoquit-from-the-editor-in-play-mode/255176/3
        ///     -解决：Application.wantsToQuit在编辑器不生效的问题


        /// <summary>
        /// Invoked by calling <see cref="Quit"/>. This is a replacement version of
        /// <see cref="Application.wantsToQuit"/> that supports play-mode in the
        /// editor.
        /// </summary>
        public static event Func<bool> wantsToQuit
        {
            add
            {
                if (Application.isEditor)
                {
                    m_WantsToQuitEditorPlaymode += value;
                }
                else
                {
                    Application.wantsToQuit += value;
                }
            }

            remove
            {
                if (Application.isEditor)
                {
                    m_WantsToQuitEditorPlaymode -= value;
                }
                else
                {
                    Application.wantsToQuit -= value;
                }
            }
        }

        /// <summary>
        /// A wrapper for <see cref="Application.Quit"/> that emits the custom
        /// <see cref="wantsToQuit"/> event which can be caught and canceled
        /// during the editor's play-mode. If the quit request is not canceled,
        /// the application will quit / play-mode will end.
        /// </summary>
        public static void Quit()
        {
            if (m_WantsToQuitEditorPlaymode != null)
            {
                foreach (Func<bool> continueQuit in m_WantsToQuitEditorPlaymode.GetInvocationList())
                {
                    try
                    {
                        if (!continueQuit())
                        {
                            return;
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private static event Func<bool> m_WantsToQuitEditorPlaymode;

        #endregion
    }
}