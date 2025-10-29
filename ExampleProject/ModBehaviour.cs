// ******************************************************************
//       /\ /|       @file       ModBehaviour.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2025-10-29 13:33:40
//    *(__\_\        @Copyright  Copyright (c) 2025, Shadowrabbit
// ******************************************************************

using ExampleProject.Defs;
using UnityEngine;
using System.Linq;

namespace ExampleProject;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    protected override void OnAfterSetup()
    {
        //api示例代码
        base.OnAfterSetup();
        //在你自己的项目的mod的Setup后初始化ConfigManager
        ConfigManager.Instance.Init(info.path);
        //生成运行时数据
        ConfigManager.Instance.GenerateConfigs();
        //在任何地方调用api来获取配置表中的数据
        var cfg = ConfigManager.Instance.cfgExample; // 示例配置
        PrintRow(DefExample.DUnitOnChain, cfg[DefExample.DUnitOnChain]);
        PrintRow(DefExample.DUnitOnDie, cfg[DefExample.DUnitOnDie]);
        PrintRow(DefExample.DHeroOnHit, cfg[DefExample.DHeroOnHit]);
    }

    private static void PrintRow(string label, Configs.RowCfgExample row)
    {
        var listBool = string.Join(",", row.testListBool);
        var dict = string.Join(",", row.testDictionary.Select(kv => $"{kv.Key}:{kv.Value}"));
        Debug.Log(
            $"[CfgExample:{label}] key={row.key}, annotate={row.annotate}, defName={row.defName}," +
            $" testStr={row.testStr}, testFloat={row.testFloat}, testInt={row.testInt}, testBool={row.testBool}," +
            $" testListBool=[{listBool}], testDictionary=[{dict}], groupId={row.groupId}");
    }
}