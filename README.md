<h1>Simple Screen Manager for Unity (aka SS)</h1>

<h2>Who is this for?</h2>

* Developers looking for a **very simple Screen Manager for Unity** with enough features to build any type of game, even mid-core or more complex projects.
* Those who want to **learn the basics in 5 minutes**.
* Those who want to **master advanced features in 15 minutes**.

<h2>Basic Usage</h2>

* Screen Settings (set this to change the main canvas scaler and the game window size):
```cs
From Menu: SS / Screen Settings / Input Screen Width & Height / Save
```

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/screen-settings.gif" alt="Demo">
</p>

* Create a screen:
```cs
From Menu: SS / Screen Generator / Input Screen Name / Generate
```

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/screen-generator.gif" alt="Demo">
</p>

* Drag screen prefab to Resources/Screens folder:

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/drag-screen.gif" alt="Demo">
</p>

* Add a screen on top with default animation
```cs
ScreenManager.Add<Screen1Controller>(screenName: "Screen1", onScreenLoad: (screen) => { });
```

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/add-screen.gif" alt="Demo">
</p>

* Close a screen
```cs
ScreenManager.Close();
```

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/close-screen.gif" alt="Demo">
</p>

* Load a scene with automatic fade
```cs
ScreenManager.Load<Scene1>(sceneName: "Scene1", onSceneLoaded: (scene1) => { });
```

<p align="center">
  <img width="50%" src="https://zenga.com.vn/learn/unity/ss/load-scene.gif" alt="Demo">
</p>

<h2>Advance Usage</h2>