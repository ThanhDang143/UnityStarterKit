# SSSystem implemented

This module implements SSSystem and creates the basic folder structure for a Unity project.

## Include
- SSSystem 3.0
- UIAdaptation
- Odin Inspector and Serializer 3.3.1.4
- Singleton

## How to use
- Start with scene named SceneManager.
- Initialize game settings and data in script SceneManager
- Enable UIAdaptation (auto add UIScaler, auto setup UISaveZone) with symbol `AUTO_UIADAPTATION`. Fix it in function `OnEnableFS()` inside `SSController.cs`

### Shortcut
- Shortcut to `SceneManager` is `alt + 1` or `option + 1` 
- Shortcut to `ScreenHome` is `alt + 2` or `option + 2` 
- Shortcut to `ScreenHome` is `alt + 2` or `option + 2` 
- Can change in `SSShortcut.cs`

## Folder structure
- Folder `_Assets` to save images, sounds, fonts...
- Folder `PluginPackages` to save plugin user add to project. Different from package tracking or ads.