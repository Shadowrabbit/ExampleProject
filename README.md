项目简介
Escape from Duckov — ExampleProject 是一个面向 Unity Mod 的轻量配置读取示例。演示如何在 MOD 生命周期中加载文本配置、生成运行时数据，并通过强类型 API 便捷访问表格内容。
主要特性
自动加载: 从 Mod 目录下 Assets/*.txt 递归加载并解析
强类型访问: 由生成器产出的 CfgExample/RowCfgExample、DefExample 常量
分组检索: 按 groupId 聚合并获取列表
极简接入: 两行完成初始化与生成运行时数据
快速开始
在你的 Mod OnAfterSetup 中初始化与生成：
读取具体配置行（无需遍历，按定义名直取）：
按分组获取列表：
目录结构
Configs/：强类型配置类（示例：CfgExample.cs、RowCfgExample）
Defs/：配置表 Key 常量（示例：DefExample.cs）
ConfigManager.cs：加载与管理配置数据的入口
ModBehaviour.cs：演示在 MOD 生命周期中的接入与使用
工作流程
Excel/文本经工具导出为 .txt
ConfigManager.Init(path) 扫描 Assets/ 下 txt → 构建 CsvReader
GenerateConfigs() 解析至 CfgExample，支持下标与分组访问
适用场景
需要在 Unity Mod 中以最低侵入成本接入配置表
希望通过常量定义名稳定索引配置，避免魔法字符串
运行环境
Unity（与 Duckov Mod 环境）
C#/.NET，兼容标准 Unity 项目结构
