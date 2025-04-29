# Unity Starter Kit for ThanhDV... and anyone else brave enough to give it a try!
## Include
- Utilities 
```
https://github.com/ThanhDang143/UnityStarterKit.git?path=/Assets/CustomPackages/Utilities
```
- SSManager
```
https://github.com/ThanhDang143/UnityStarterKit.git?path=/Assets/CustomPackages/SSManager
```

## Changelog v0.0.11
### SimpleScreenManager (SSManager)
- Import SSManager

## Changelog v0.0.10
### Utilities
- Add DebugExtension, EventDispatcher
- Fix Singleton
- Package can add via Git URL

## Changelog v0.0.9
- Update Singleton for exception case, MonoBehaviour dont destroy and can destroy.

* Developers looking for a **very simple Screen Manager / Navigator for Unity** with enough features to build any type of game, even mid-core or more complex projects.
* Those who want to **learn the basics in 10 minutes**.
* Those who want to **master advanced features in 30 minutes**.

## Changelog v0.0.6
- Remove the installation function through Git URL.

* Regardless of fullscreen page or modal/modeless window, they are all considered **Screen**.
* At any given time, **only one Screen is visible** to optimize performance. If a Screen is shown on top of another, the underlying Screen will be temporarily hidden, and it will reappear when the top Screen is closed.
* A Screen is a freeform prefab that does **not require any specific scripts**, allowing it to be used as a child object anywhere.
* A **Scene** is freeform and contains any object, including its own UI canvas. The Scene’s canvas will not be hidden when a Screen is displayed on top.
* The package size is only 38 KB and does **not depend** on any external libraries.

<p align="center">
  <img width="200px" src="/learn/unity/ss/demo.gif?raw=true" alt="Demo">
  <p align="center">In this demo, even though they share the same prefab, the Store can be displayed as a modal window or as the content of the Store Tab.</p>
</p>

<h2>Basic Usage</h2>

<h3>1. Screen Settings</h3>
Set this to change the main canvas scaler and the game window size

## Changelog v0.0.1
### Release:
- SSSystem Implemented v0.0.1
- UIAdaptation v0.0.1
- Singleton v0.0.1