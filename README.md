<h1>Simple Screen Manager for Unity (aka SS)</h1>

<h2>Who is this for?</h2>

* Developers looking for a **very simple Screen Manager for Unity** with enough features to build any type of game, even mid-core or more complex projects.
* Those who want to **learn the basics in 1 minute**.
* Those who want to **master advanced features in 10 minutes**.

<h2>Basic Usage</h2>

* Create a screen:
```cs
From Menu: SS / Screen Generator / Input Screen Name / Generate
```
* Add a screen on top with default animation
```cs
ScreenManager.Add<Screen1>("Screen1", onScreenLoad: (screen) => { });
```
* Close a screen
```cs
ScreenManager.Close();
```
* Load a scene with automatic fade
```cs
ScreenManager.Load<Scene1>("Scene1", onSceneLoaded: (scene1) => { });
```

<h2>Advance Usage</h2>