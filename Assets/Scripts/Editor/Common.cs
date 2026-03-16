using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GamePlay.Editor
{
    public static class EditorCommon
    {
        public static void GoScene(string path)
        {
            // 检查当前场景是否被修改
            if (SceneManager.GetActiveScene().isDirty)
            {
                // 显示对话框询问用户是否保存
                int option = EditorUtility.DisplayDialogComplex(
                    "场景已被修改",
                    "当前场景已修改，是否保存更改？",
                    "保存并切换",
                    "取消",
                    "不保存切换"
                );

                switch (option)
                {
                    case 0: // 保存并切换
                        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                        EditorSceneManager.OpenScene(path);
                        break;
                    case 1:
                        return;
                    case 2:
                        EditorSceneManager.OpenScene(path);
                        break;
                }
            }
            else
            {
                EditorSceneManager.OpenScene(path);
            }
        }

        public static void GoSceneNoSave(string path)
        {
            EditorSceneManager.OpenScene(path);
        }
    }
}