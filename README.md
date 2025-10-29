```markdown
# Escape from Duckov — ExampleProject

轻量级 Unity Mod 配置读取示例，演示如何通过 RabiConfigLib 工具链把 Excel 表生成运行时代码与数据（.cs + .txt），并在 Mod 启动时通过强类型 API 访问配置。

核心流程：Excel（多表） → run.exe（RabiConfigLib）→ 生成 .cs 强类型代码 与 .txt 数据 → Mod 引用运行库并两行初始化访问。

## 核心说明
- 生成阶段：使用 RabiConfigLib 的生成器（run.exe）读取 Excel，生成强类型 C# 代码与导出文本数据。
- 运行阶段：Mod 引用 RabiConfigLib 运行库，运行时读取生成的 .txt 并通过生成的强类型 API 访问数据（两行初始化调用）。

## 主要特性
- Excel 驱动：按示例格式准备 N 个 Excel 表（支持表头）。
- 自动生成：run.exe 自动生成强类型 .cs 文件与导出的 .txt 数据。
- 运行时库：RabiConfigLib 提供读取/加载运行时的基础能力。
- 极简接入：两行初始化完成运行时数据生成。

## 快速开始（5 步）
1. 引用运行时库  
   - 在你的 Mod 项目中添加 RabiConfigLib 的运行时库（RabiConfigLib.dll、UnityPackage 或相等方式），确保运行时可以加载该库。
   - 从ExampleProject拷贝RabiConfig目录到自己的Mod根目录下(内部包含生成器run.exe、示例excel表、config.json配置文件)

2. 准备 Excel 表（N 个）  
   - 按示例格式准备表格（推荐保留表头以便按列名映射）。  
   - 示例列（可按项目约定）：id, name, groupId, value。  

3. 修改生成器配置 config.json（位于 RabiConfigLib 工具目录）  
   - 将 NAME_SPACE_NAME 设置为你的 Mod 命名空间（例如 `Shadowrabbit.Mod.Configs`）。  
   - 将 CODE_EXPORT_FOLDER 设置为生成代码的目标目录（指向你的 Mod 源码目录或会被编译进 Mod 的文件夹）。  
   - 将 DATA_EXPORT_FOLDER 设置为生成数据（.txt）的目标目录（建议指向 Mod 的 Assets 目录）。  

4. 运行生成器（自动生成代码与文本）  
   - 在生成器目录运行：  
     run.exe --config config.json  
   - 生成结果示例：`Configs/CfgExample.cs`, `Configs/RowCfgExample.cs`, 以及 `Assets/CfgExample.txt`（路径由 config.json 决定）。  

5. 在 Mod 启动入口调用两行 API（运行时初始化）  
   ```csharp
   // 1) 初始化运行时配置管理（传入包含 Assets/ 的 Mod 根路径）
   ConfigManager.Init(modRootPath);

   // 2) 调用生成的表级 API 生成运行时强类型数据（示例表名：CfgExample）
   CfgExample.GenerateConfigs();
   ```

注意事项
- NAME_SPACE_NAME 必须是合法的 C# 命名空间（点分隔的标识符，不能含空格或非法字符）。  
- CODE_EXPORT_FOLDER 与 DATA_EXPORT_FOLDER 可使用相对路径（相对于 run.exe）或绝对路径；确保生成器有写入权限。  
- 推荐把生成代码目录配置为 Mod 的源码目录或会被包含进 Mod 程序集的目录，这样生成后可直接编译。  
- 若保留表头（IncludeHeader），生成器会尝试按列名映射；否则按列序解析。

## 生成后整合到 Mod
1. 将生成的 .cs 文件纳入你的 Mod 源码（或放到会被编译的位置）。  
2. 确保生成的 .txt 数据在运行时能被 ConfigManager.Init 指定的路径找到（通常放在 Assets/ 下）。  
3. 在 Mod 启动时调用 ConfigManager.Init(...) 与 每个表的 GenerateConfigs() 完成运行时数据生成。  
4. 如需热重载，可通过 ConfigManager.Clear() + Init() 或再次调用 GenerateConfigs() 完成热刷。
