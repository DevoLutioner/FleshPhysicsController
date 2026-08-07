# 躯体肉感物理控制器（Flesh Physics Controller）v0.8.6.3

[![Release](https://img.shields.io/github/v/release/DevoLutioner/FleshPhysicsController)](https://github.com/DevoLutioner/FleshPhysicsController/releases)

**English:** Flesh Physics Controller is a BepInEx 5 plugin for Koikatsu / Koikatsu Party that adds flesh jiggle physics to the thighs, arms and belly. It includes a spring system for everyday use and an MMD-accurate chain system for dancing, with per-bone controls, XML presets and per-card save.

**搜索关键词 / Keywords:** Koikatsu, Koikatu, Koikatsu Party, KKS, flesh physics, jiggle, thighs, arm, belly, dynamic bone, spring physics, chain physics, BepInEx, plugin, mod

为角色的大腿、手臂、小肚子增加“肉感物理”：肉随动作自然滞后摆动，
类似胸部/臀部物理的手感。不修改角色数据文件，不改骨骼缩放，
尽量保持角色卡片原本的体型。

## 功能一览

- 三个部位独立调参：**Thigh（大腿）**、**Arm（手臂）**、**Belly（小肚子）**
- 两套物理系统可切换：**弹簧系统**（日常跟手）与**链式系统**（跳舞增强）
- 每根肉感骨骼独立控制：开关、振幅（Amp）、旋转（Rot）、
  旋转计算（RC，默认开启）、XYZ 轴冻结
- **Dance response（0~5）**：增强 VMD 跳舞抖动，弹簧/链式共用同一倍数
- 参数实时可调，随角色卡自动保存（GUID 不变，旧卡数据兼容）
- 内置 Soft / Realistic / Exaggerated 预设，支持保存/加载 XML 预设
- 一键 `Clear shape` 还原卡片原始体型
- 面板带输入屏蔽，不会点击穿透到游戏视角

## 安装

1. 把压缩包里的 `BepInEx\plugins\ThighPhysicsController\` 整个文件夹
   覆盖到游戏根目录的 `BepInEx\plugins\` 下（同名文件夹直接合并）。
2. 启动 `CharaStudio.exe`（或 `Koikatu.exe`）。
3. 加载任意角色卡，按 `Insert` 打开面板。

> 插件文件夹与 DLL 沿用 `ThighPhysicsController` 名称是兼容性考虑；
> 插件显示名与游戏内面板标题统一为英文 `Flesh Physics Controller`。

## 使用方法

### 打开与基本操作

- `Insert`：开关面板。
- 面板顶部选择角色，中部用 `Thigh / Arm / Belly` 切换部位。
- `Thigh physics enabled`（或 Arm/Belly）：该部位物理总开关，默认开启。
- 所有滑条拖动实时生效，改动会随角色卡自动保存。

### 两种物理系统的切换

面板里有一个开关：

`Game DynamicBone chain physics (MMD-accurate)`

- **不勾选 = 弹簧系统**（默认）；
- **勾选 = 链式系统**。

两个系统拥有完全独立的参数集，互不串扰：

- 弹簧系统：使用上方的 `Weight / Gravity` + `Bone Thigh flesh (shared)` 参数区
  （Damping / Elasticity / Stiffness / Inert），每骨参数用 `Bones`；
- 链式系统：使用 `Chain mode parameters` 参数区，每骨参数用 `ChainBones`。

### 参数速查

| 参数 | 范围 | 弹簧默认 | 链式默认 | 作用 |
| --- | --- | --- | --- | --- |
| Weight | 0 ~ 1 | 0.7 | 0.7 | 肉感整体强度 |
| Gravity | -0.2 ~ 0.2 | 0.05 | 0.05 | 下垂感，正数向下垂 |
| Damping | 0 ~ 1 | 0.12 | 0.30 | 运动阻尼，越大越安静 |
| Elasticity | 0 ~ 1 | 0.02 | 0.25 | 弹性回中力度 |
| Stiffness | 0 ~ 1 | 0.08 | 0.9 | 刚性/长度保持 |
| Inert | 0 ~ 1 | 0.30 | 0.40 | 惯性滞后，越大甩得越明显 |
| Dance response | 0 ~ 5 | 1 | 1 | 舞蹈/动作响应倍数（两系统统一） |
| Jitter freq | 0 ~ 2.5 | 1 | 1 | 抖动频率：越高回弹越快、抖动越密；0 为完全自由 |
| Motion smooth | 0.05 ~ 0.5 | 0.25 | — | 弹簧响应平滑：越小越柔、越不抖 |

每根骨（`Per-bone: Amp / Rot / RC / Axis`）：

- `Amp`：该骨摆动幅度；
- `Rot`：非 RC 骨骼的平滑旋转幅度（默认 0.25）；
- `RC`：RotCalc，让骨骼朝下一根肉感骨方向瞄准旋转（默认开启）；
- `X / Y / Z`：对应轴的振幅，设 0 等于冻结该轴，也可以输入 0.5 做半衰减。

### 预设与恢复

- 内置预设：`Soft`（Weight=0，最安静）、`Realistic`、`Exaggerated`；
- `Save preset`：输入名字保存（自动补 `.xml`，不会写出预设目录）；
- `Load preset`：加载列表中选中的预设；
- `Load from file...`：从外部任意位置加载 XML 预设；
- `Clear shape (restore card defaults)`：一键还原卡片原始体型；
- `Reset to defaults`：把当前部位参数恢复为插件默认值。

### 配置文件

`BepInEx\config\codex.koikatumanager.thighphysicscontroller.cfg`

- `Window key`：面板快捷键（默认 Insert）；
- `Auto apply on load`：角色加载时自动应用（默认 true）；
- `Force enable`：即使卡片禁用了物理也强制开启（默认 true）；
- `Remember per-character settings`：按角色记忆本次会话的参数，
  同场景同名同性别角色自动同步（默认 true）；
- `Auto fix spring drift`：弹簧模式后台慢速纠偏，把跳舞中误固化的基准
  拉回卡片原始姿态，防止大腿越跳越变形（默认 true）；
- `Log flesh physics`：每 2 秒输出一次物理偏移日志（调试用）；
- `Dump skeleton bones`：启动时转储骨骼层级（调试用）。

### 多角色场景：区分、记忆、同步

- 角色列表按 **女性角色 / 男性角色** 分组显示（`#序号 名字`），
  同名男女不会再混在一起；
- 选中角色按实例记忆，场景里增删角色不会跳到另一个人身上；
- 参数按“名字 + 性别 + 性格”记忆（本次游戏会话内），换人/换场景再回来
  会恢复上次的手感，而不是恢复默认或重新读 XML；
- 同场景里同名同性别角色自动共享同一套参数：设置一次，其他同名角色同步生效；
- 角色卡保存时参数仍会写回卡片；记忆只在本会话有效，重启游戏后从卡片/默认值开始。

## 弹簧系统 vs 链式系统（重点）

### 弹簧系统（Spring）

弹簧系统把每根肉感骨当成一个“粒子”：父关节一动，粒子按父关节的
加速度、角速度和基准位移获得滞后速度，再由弹簧拉回基准位置。

特点：**跟手、响应快，抖动频率高、细节细碎**，适合日常走路、转身、挥手、
H 场景这类自然动作，是默认主推手感。

主要参数：`Weight`（整体强度）、`Gravity`（下垂）、`Damping`（阻尼）、
`Elasticity`（回中）、`Stiffness`（刚性）、`Inert`（惯性），
加上每骨 `Amp / Rot / RC / Axis`。

### 链式系统（Chain）

链式系统移植自游戏 DynamicBone_Ver02 的粒子链算法：根粒子锁定在锚点骨骼上，
子粒子逐个做弹簧 + 长度约束，锚点的位移和旋转驱动整条链滞后。

特点：**低频、滞后明显、拖尾长，动作幅度越大越有“肉甩起来”的感觉**，
专门用于 VMD 跳舞增强。

链式有独立的参数集（`Chain mode parameters`）和独立的每骨参数（`ChainBones`），
不占用弹簧滑条。实现上带有几项保护：

- 长度约束收紧（Stiffness=1 保持原骨长），不再出现“橡皮泥”式拉伸；
- 锚点位移限幅 0.30m，避免其它插件（如 BPC）放大骨盆运动时把链拉飞；
- 每帧按当前骨骼姿态刷新静止方向/长度，兼容 KKABMX 的骨骼缩放；
- 小肚子链刻意排除了 `cf_s_waist02` 结构骨，跳舞时不会撕皮肤/身体消失。

### 对比表

| 对比项 | 弹簧系统 | 链式系统 |
| --- | --- | --- |
| 物理模型 | 单粒子弹簧回中 | 链式粒子 + 长度约束 |
| 参数集 | 共享参数 + `Bones` | `Chain` 参数 + `ChainBones`（独立） |
| 驱动来源 | 父关节加速度 / 角速度 / 位移 | 锚点位移 / 锚点角速度 |
| 手感 | 高频跟手、细节细碎 | 低频大幅拖尾 |
| 适合场景 | 日常动作、自然写实 | VMD 跳舞、大幅摆动 |
| 舞蹈响应 | 统一倍数（默认 1=1x） | 统一倍数（参考系数 0.000384） |
| 每骨控制 | `Bones`（Amp/Rot/RC/Axis） | `ChainBones`（Amp/Rot/RC/Axis） |
| 兼容性 | 常规 | ABMX 每帧刷新 + 锚点限幅 |

### 怎么选

- 想要“自然一点、随时跟手”：用**弹簧系统**；
- 想要“跳舞时明显甩肉、拖尾”：用**链式系统**，并把 Dance response 开到 1~3；
- 两者可以随时切换，参数各自保留，不会互相覆盖。

## 教程

### 第 1 步：安装并打开面板

按上文“安装”完成部署 → 启动 CharaStudio → 加载角色卡 → 按 `Insert`。
如果面板显示 `No characters loaded`，先在工作室里加载一个角色。

### 第 2 步：从弹簧系统起步（推荐）

1. 部位选 `Thigh`，确认 `Thigh physics enabled` 已勾选；
2. **不勾选** `Game DynamicBone chain physics`（弹簧模式）；
3. 参数先保持默认：Weight 0.7、Gravity 0.05、Damping 0.12、
   Elasticity 0.02、Stiffness 0.08、Inert 0.30；
4. 让角色走路、转身、挥手，观察大腿肉是否自然滞后；
5. 想让肉更软更甩：调大 Inert、调小 Damping；
   想让肉更稳更紧：调大 Damping、Stiffness；
6. 手臂（Arm）和小肚子（Belly）用同样方法，默认幅度已按部位缩小
   （手臂 0.6、小肚子 0.25）。

### 第 3 步：切换到链式，感受跳舞增强

1. 勾选 `Game DynamicBone chain physics (MMD-accurate)`，
   面板自动切换为 `Chain mode parameters`；
2. 参数先保持默认：Weight 0.7、Gravity 0.05、Damping 0.30、
   Elasticity 0.25、Stiffness 0.9、Inert 0.40；
3. 播放 VMD 跳舞动作，观察大腿/手臂/肚子的滞后拖尾；
4. 觉得不够强：把 `Dance response` 调到 2~5；
   觉得太飘：减小 Chain 的 Weight，或调大 Damping。

### 第 4 步：调每根骨

1. `Per-bone: Amp / Rot / RC / Axis (0 = freeze)` 区域列出该部位所有肉感骨；
2. 勾选某根骨后调 `Amp` 控制摆动幅度；X/Y/Z 任一设 0 表示该轴冻结；
3. 想让骨骼朝下一根肉感骨方向旋转：`RC` 默认已勾上，取消即关闭；
4. 不勾 RC 时，`Rot` 滑条提供基于偏移的平滑旋转（默认 0.25）。

### 第 5 步：保存你的手感

1. 调好参数后，在 Presets 区输入名字（例如 `MyDance.xml`），点 `Save preset`；
2. 下次点 `Load preset`，或从外部 `Load from file...` 载入；
3. 想回到卡片原始体型：点 `Clear shape (restore card defaults)`。

### 第 6 步：排查与调试

- 面板没反应：确认角色已加载、部位开关已打开；
  查看 `output_log.txt` 是否有 `Flesh physics initialized`；
- 跳舞不抖：确认 `Dance response` > 0；链式效果比弹簧明显；
- 身体消失/皮肤撕裂：确认小肚子链不含 `cf_s_waist02`（0.8.0 起已排除）；
  如出现，点 `Clear shape` 后重新 `Apply now`；
- 开调试日志：把配置里 `Log flesh physics` 设为 true，
  每 2 秒会输出 `Flesh physics [骨骼名]: applied=... mag=... rot=...`，
  链式模式输出 `chain applied=... anchor=... amp=...`。

## 0.8.6.3 变更

- `Jitter freq` 下限从 0.2 放开到 0（弹簧/链式、卡片与预设读取同步），
  想要更自由、更“甩”的手感可以把频率调到 0~0.2。

## 0.8.6.2 变更

- 小肚子链移除 `cf_s_spine03`（上段脊柱，刚性骨，不应抖动），只保留
  `cf_s_waist01`；链式模式新增单粒子链支持，小肚子在弹簧/链式下都能正常摆动；
- 小肚子每骨参数区现在只有 `waist01` 一行。

## 0.8.6.1 变更

- 修复部件切换失效：Thigh/Arm/Belly 改用显式按钮，避免面板停在错误部位
  （选小肚子却显示手臂骨骼名的症状即由此引起）；
- 每骨参数区标题标注当前模式（Spring/Chain）；
- 小肚子链固定为 `cf_s_waist01 + cf_s_spine03`（spine03 是上段脊柱，
  视觉上接近肩背，但属于小肚子链的一部分，不是肩臂骨）。

## 0.8.6 变更

- 开放新参数并真实接入物理（随卡片/预设保存）：
  - 弹簧：`Jitter freq`（抖动频率，缩放弹簧回弹速度与弹性回中）、
    `Motion smooth`（响应平滑，控制加速度/角速度滤波，越小越柔）；
  - 链式：`Jitter freq`（抖动频率，缩放刚度/弹性修正）；
- 卡片数据版本升至 55（旧卡缺失字段自动用新默认值，GUID 不变）。

## 0.8.5.1 变更

- RC（RotCalc）默认全开：新建参数、内置预设的每骨 RC 与共享 IsRotationCalc
  默认都为 true（弹簧与链式一致），旧卡已存值不受影响；
- 修复“关闭后腿部仍变形/清除无效”：部位开关关闭时物理立即恢复骨骼姿态；
  链式模式下禁用的骨骼位置+旋转完全复位；移除物理前先复位骨骼，
  避免重新启用后把变形姿态当“原始姿态”固化；
- 链式 RC 不再旋转已禁用的骨骼，避免关掉某根骨后它仍被上/下一根骨带动；
- 弹簧模式防积累：重锚检测改为父空间局部判定（父骨旋转不再把偏移固化进基准），
  RC 改为基准旋转 + ±12° 限幅（消除跳舞反馈扭曲），并内置后台慢速纠偏
  （检测到基准偏移持续 2 秒且无外部改写时，以 0.03m/s 量级拉回卡片姿态），
  正常抖动不受影响，可在配置中关闭；
- 完整历史见包内 `CHANGELOG.md`。

## 0.8.5 变更

- Dance response 上限从 3 放开到 5（UI、卡片读取、预设读取三处同步），默认仍 1；
- 弹簧减抖：默认 Weight 0.8→0.7、Damping 0.03→0.12、Inert 0.35→0.30；
  加速度平滑 0.45→0.25、X/Z 增益 1.5→1.25、驱动系数 0.0003→0.00025、
  弹簧速度阻尼下限 0.8，旧卡若仍觉得抖，点 `Reset to defaults` 生效新默认；
- 链式增强：默认 Weight 0.7、Damping 0.30、Inert 0.40；舞蹈驱动系数
  0.000384→0.0006，切向限幅 0.03→0.05、速度限幅 0.16→0.22，
  Dance response 调到 2~5 时拖尾更明显；
- 多角色场景：角色列表按女性/男性分组显示；选中按实例记忆，增删角色不串人，
  点击行直接切换（不再用选择网格）；
  参数按“名字+性别+性格”会话内记忆，同名同性别角色自动同步；
  `OnReload` 尊重 `maintainState`，场景刷新/换装不再强制重置参数；
- 兼容：移除 KKAPI 最低版本限制（`[BepInDependency("marco.kkapi")]`），
  低版本 KKAPI 环境也能加载。

## 0.8.1 变更

- 插件显示名与游戏内面板标题统一为英文 `Flesh Physics Controller`
  （GUID 不变，旧卡兼容）；文件对话框、配置描述、日志前缀同步统一；
- Dance response 在弹簧/链式之间统一倍数：默认参数下 1=1x、2=2x、3=3x，
  调 Weight/Inert 时两种模式按同一公式缩放，默认手感不变；
- README 重写：新增使用方法、弹簧/链式对比与分步教程。

## API

- GUID：`codex.koikatumanager.thighphysicscontroller`（不可变）
- 版本：0.8.6.3
- 显示名：Flesh Physics Controller
- 卡片数据版本：55
- 依赖：KKAPI `marco.kkapi`（不限制最低版本）
- Harmony 补丁仅用于输入屏蔽：
  `Input.GetAxis / GetAxisRaw / GetMouseButton / GetMouseButtonDown / GetMouseButtonUp`

## 说明

- `ThighBoneParams` 里的 `CollisionRadius / LeverLength / ReflectSpeed /
  SwayAmplitude / DriveGain / Spring / PendulumDamping` 仅保留在卡片/预设
  schema 中以兼容旧数据，当前物理不使用。
- 链式模式下 `Rot` 与 `RC` 均生效：非 RC 骨骼用 Rot 做平滑旋转，
  RC 骨骼优先瞄准。
- 内部类名/程序集名/插件目录沿用 `ThighPhysicsController`，仅为了兼容安装路径。
- 角色参数记忆仅限本次游戏会话；跨重启请保存卡片或使用预设 XML。
