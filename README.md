# RabiConfigLib 使用指南

轻量级 Unity Mod 配置读取框架，演示如何通过 RabiConfigLib 工具链把 Excel 表生成运行时代码与数据（.cs + .txt），并在 Mod 启动时通过强类型 API 访问配置。

**核心流程：** Excel（多表） → run.exe（RabiConfigLib）→ 生成 .cs 强类型代码 与 .txt 数据 → Mod 引用运行库并两行初始化访问。

## 目录
1. [下载并安装工程](#1-下载并安装工程)
2. [配置项目引用](#2-配置项目引用)
3. [拷贝 RabiConfig 工具目录](#3-拷贝-rabiconfig-工具目录)
4. [配置生成器](#4-配置生成器)
5. [准备 Excel 数据](#5-准备-excel-数据)
6. [配置 manifest.txt](#6-配置-manifesttxt)
7. [设置 Excel Sheet](#7-设置-excel-sheet)
8. [运行生成器](#8-运行生成器)
9. [在 Mod 中使用](#9-在-mod-中使用)
10. [常见问题](#常见问题)

---

## 1. 下载并安装工程

### 1.1 下载 RabiConfigLib
从创意工坊或代码仓库下载 RabiConfigLib 工程。

---

## 2. 配置项目引用

### 2.1 找到 ExampleProject.csproj
如果你有项目，需要创建自己的 Mod 项目。

### 2.2 修改游戏目录
在 `ExampleProject.csproj`（或你的项目文件）中，找到游戏目录配置：
```xml
<PropertyGroup>
    <DuckovPath>D:\SteamLibrary\steamapps\common\Escape from Duckov</DuckovPath>
</PropertyGroup>
```
将 `DuckovPath` 修改为你实际的游戏安装路径。

### 2.3 配置 RabiConfigLib 引用
确保项目文件中引用了 RabiConfigLib：
```xml
<ItemGroup>
    <Reference Include="RabiConfigLib">
        <HintPath>..\..\RabiConfigLib\RabiConfigLib.dll</HintPath>
        <Private>False</Private>
    </Reference>
</ItemGroup>
```
或者使用相对路径指向 RabiConfigLib 的 DLL 文件位置。

---

## 3. 拷贝 RabiConfig 工具目录

### 3.1 找到 RabiConfig 工具目录
RabiConfig 工具目录通常包含：
- `run.exe` - 代码生成器主程序
- `config.json` - 生成器配置文件
- `manifest.txt` - 表格清单文件
- 其他工具依赖文件

### 3.2 拷贝到你的项目
将整个 `RabiConfig` 目录拷贝到你的 Mod 项目目录中，例如：
```
你的Mod项目\
├── RabiConfig\
│   ├── run.exe
│   ├── config.json
│   ├── manifest.txt
│   └── ...
├── Assets\
└── ...
```

---

## 4. 配置生成器

### 4.1 打开 config.json
编辑 `RabiConfig\config.json` 文件。

### 4.2 配置命名空间
找到 `NAME_SPACE_NAME` 字段，设置为你的 Mod 命名空间：
```json
{
    "NAME_SPACE_NAME": "YourModName",
}
```

**注意事项：**
- 命名空间必须是合法的 C# 标识符
- 使用点分隔，不能包含空格或特殊字符

### 4.3 配置代码输出目录
找到 `CODE_EXPORT_FOLDER` 字段，设置为生成代码的目标目录：
```json
{
    "CODE_EXPORT_FOLDER": "../YourModName",
}
```

**建议：**
- 指向你的 Mod 源码目录
- 确保该目录会被编译进 Mod 程序集
- 可以使用相对路径（相对于 run.exe）或绝对路径

### 4.4 配置数据输出目录
找到 `DATA_EXPORT_FOLDER` 字段，设置为生成数据文件的目标目录：
```json
{
    "DATA_EXPORT_FOLDER": "../Assets/Game",
}
```

**建议：**
- 建议使用默认路径，不修改

---

## 5. 准备 Excel 数据

### 5.1 创建 Excel 文件
按照你的数据需求创建 Excel 文件（.xlsx 格式）。

### 5.2 表格格式要求
- **第一行**：表头（列名），例如：`key | Annotate | Defname | value | ...`
- **第一列**：通常作为主键（key），用于标识每一行数据
- **分隔符**：使用 `|`（竖线）分隔列（生成器会自动处理）
- **数据行**：从第二行开始填写实际数据

### 5.3 Excel 示例格式
```
key          | name      | groupId | value | description
item_001     | 物品A     | 1       | 100   | 这是一个物品
item_002     | 物品B     | 2       | 200   | 这是另一个物品
```

### 5.4 支持的数据类型
- `int` - 整数
- `float` - 浮点数
- `string` - 字符串
- `bool` - 布尔值
- `Vector3` - 格式：`x-y-z`，例如：`1.5-2.0-3.5`
- `Dictionary<TK, TV>` - 格式：`key1:value1,key2:value2`
- `List<T>` - 格式：`value1,value2,value3`

---

## 6. 配置 manifest.txt

### 6.1 打开 manifest.txt
编辑 `RabiConfig\manifest.txt` 文件。

### 6.2 添加表格配置
在 `manifest.txt` 中列出需要生成代码和数据的 Excel 文件：

```
# 格式：Excel文件名
# 例如：
ItemConfig.xlsx
WeaponConfig.xlsx
SkillConfig.xlsx
```

---

## 7. 设置 Excel Sheet

### 7.1 理解 Sheet 机制
一个 Excel 文件可以包含多个 Sheet（工作表），但只有指定的 Sheet 会被处理。

在excel第一个sheet里，每行填写一个需要解析的sheet名，允许这个sheet被解析。

---

## 8. 运行生成器

双击run.exe

### 8.3 检查生成结果
生成完成后，检查以下位置：

**代码文件（CODE_EXPORT_FOLDER）：**
```
YourModProject\Configs\
├── CfgItemConfig.cs        # 主配置类
├── RowCfgItemConfig.cs     # 行数据类
├── CfgWeaponConfig.cs
└── RowCfgWeaponConfig.cs
```

**数据文件（DATA_EXPORT_FOLDER）：**
```
YourModProject\Assets\Configs\
├── CfgItemConfig.txt
├── CfgWeaponConfig.txt
└── ...
```

### 8.4 生成文件说明
- `Cfg[TableName].cs` - 主配置类，包含 `GenerateConfigs()` 方法
- `RowCfg[TableName].cs` - 行数据类，表示表格中的一行数据
- `Cfg[TableName].txt` - 数据文件，运行时读取的文本数据

---

## 9. 在 Mod 中使用

### 9.1 引用生成的代码
确保生成的 `.cs` 文件被包含在你的 Mod 项目中，并能够正确编译。

### 9.2 在 Mod 入口初始化
在你的 Mod 入口类（继承自 `ModBehaviour`）中调用初始化代码：

```csharp
using RabiConfigLib;
using YourModName; // 你的命名空间

public class YourMod : ModBehaviour
{
    public override void OnLoad()
    {
        // 1. 初始化配置管理器
        // 传入 Mod 的根路径（包含 Assets 目录的路径）
        var modRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        ConfigManager.Init(modRootPath);
        
        // 2. 生成所有配置表的运行时数据
        CfgItemConfig.GenerateConfigs();
        CfgWeaponConfig.GenerateConfigs();
        // ... 其他配置表
        
        UnityEngine.Debug.Log("[YourMod] Configs initialized!");
    }
}
```

### 9.3 获取配置数据
通过 `ConfigManager` 获取配置数据：

```csharp
// 通过 key 获取单行数据
var cfg = ConfigManager.Instance.cfgExample; // 示例配置
PrintRow(DefExample.DUnitOnChain, cfg[DefExample.DUnitOnChain]);
```

---

## 常见问题

### Q1: 生成器找不到 Excel 文件？
**A:** 确保 Excel 文件放在 `RabiConfig` 目录下，或修改 `config.json` 中的 Excel 文件路径配置。

### Q2: 生成的代码编译错误？
**A:** 检查命名空间是否正确，确保引用了 `RabiConfigLib.dll`。

### Q3: 运行时找不到数据文件？
**A:** 确保数据文件在 `Assets` 目录下，且 `ConfigManager.Init()` 传入的路径正确。

### Q4: 如何热重载配置？
**A:** 调用 `ConfigManager.Clear()` 然后重新调用 `GenerateConfigs()`。

### Q5: 支持哪些数据类型？
**A:** 支持 int、float、string、bool、Vector3、Dictionary<K,V>、List<T> 等。

---

## 总结

使用 RabiConfigLib 的完整流程：
1. ✅ 下载并放置到 Mods 目录
2. ✅ 配置项目引用和路径
3. ✅ 拷贝 RabiConfig 工具目录
4. ✅ 修改 config.json 配置命名空间和输出目录
5. ✅ 准备 Excel 数据表
6. ✅ 配置 manifest.txt 指定要处理的表格
7. ✅ 在 Excel 中设置 Sheet 标记
8. ✅ 运行 run.exe 生成代码和数据
9. ✅ 在 Mod 入口调用两行初始化代码
10. ✅ 通过 ConfigManager 获取配置数据

现在你可以开始使用 RabiConfigLib 管理你的 Mod 配置数据了！
