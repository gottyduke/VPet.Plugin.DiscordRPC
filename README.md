# VPet.Plugin.DiscordRPC
 为 [VPet](https://github.com/LorisYounger/VPet) 启用 discord rich presence.


## 示例
![slacking](static/assets/slacking.png)
![idling](static/assets/idling.png)
> Supports English locale.


## 动画版权声明与授权

在github中 [桌宠动画文件](https://github.com/LorisYounger/VPet/tree/main/VPet-Simulator.Windows/mod/0000_core/pet/vup) 动画版权归 [虚拟主播模拟器制作组](https://www.exlb.net/VUP-Simulator)所有, 本插件使用个别差分图均非商业用途, 所有版权归如上归属.


## 需求
- [Visual Studio Community 2022](https://visualstudio.microsoft.com/)
  - 带有.Net Framework/CSharp的桌面开发


## 构建
本插件移除了原Demo项目推荐的nuget包, 因为其与上流[VPet_Simulator.Core](https://github.com/LorisYounger/VPet/tree/main/VPet-Simulator.Core)相比存在更新脱节, 构建时需要手动指定依赖到VPet目录下`VPet-Simulator.Core.dll`, `VPet-Simulator.Windows.Interface.dll`实例.


## 许可

[MIT](LICENSE)


## 鸣谢

- [虚拟桌宠模拟器](https://github.com/LorisYounger/VPet)


---


# VPet.Plugin.DiscordRPC
 Enables discord rich presence for [VPet](https://github.com/LorisYounger/VPet) .


## Animation copyright notice and authorization terms

The copyright of the [pet animation files](./VPet-Simulator.Windows/mod/0000_core/pet/vup) provided in the source code belongs to [the VUP-Simulator team](https://www.exlb.net/VUP-Simulator). This plugin uses sliced sprites that inherit and sustain the above license.


## Requirement
- [Visual Studio Community 2022](https://visualstudio.microsoft.com/)
  - Desktop application development with .Net framework/CSharp.


## Building
This plugin has removed the nuget reference of [VPet_Simulator.Core](https://github.com/LorisYounger/VPet/tree/main/VPet-Simulator.Core) because it's severely lacking behind on updates. You need to manually add reference to `VPet-Simulator.Core.dll`, `VPet-Simulator.Windows.Interface.dll` in VPet instance path.


## LICENSE

[MIT](LICENSE)


## Credits

- [VPet Simulator](https://github.com/LorisYounger/VPet)

