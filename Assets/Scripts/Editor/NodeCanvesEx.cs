using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFrame.Runtime;
using ParadoxNotion.Design;
using UnityEditor;

// 在某个编辑器脚本中添加类型
[InitializeOnLoad]
public class CustomTypeRegistration
{
    private static List<Type> _effComponentTypes;

    static CustomTypeRegistration()
    {
        GetAllEffComponentTypes();
        foreach (var VARIABLE in _effComponentTypes)
        {
            TypePrefs.AddType(VARIABLE);
        }

        TypePrefs.AddType(typeof(EffEntity));
    }

    public static List<Type> GetAllEffComponentTypes()
    {
        if (_effComponentTypes != null)
            return _effComponentTypes;

        _effComponentTypes = new List<Type>();

        // 获取当前程序集
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            try
            {
                // 查找所有继承自EffComponent的类型
                IEnumerable<Type> types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(EffComponent)));

                _effComponentTypes.AddRange(types);
            }
            catch (ReflectionTypeLoadException)
            {
                // 忽略加载失败的程序集
                continue;
            }
        }

        return _effComponentTypes;
    }

    /// <summary>
    /// 根据命名空间获取EffComponent类型
    /// </summary>
    /// <param name="namespaceName">命名空间名称</param>
    /// <returns>指定命名空间下的EffComponent子类列表</returns>
    public static List<Type> GetEffComponentTypesByNamespace(string namespaceName)
    {
        return GetAllEffComponentTypes()
            .Where(t => t.Namespace != null && t.Namespace.Contains(namespaceName))
            .ToList();
    }

    /// <summary>
    /// 根据类型名称获取EffComponent类型
    /// </summary>
    /// <param name="typeName">类型名称</param>
    /// <returns>匹配的EffComponent子类</returns>
    public static Type GetEffComponentTypeByName(string typeName)
    {
        return GetAllEffComponentTypes()
            .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
    }
}