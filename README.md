<h1>Simple Screen Manager for Unity (aka SS)</h1>

<h2>Who is this for?</h2>

* Developers looking for a **very simple Screen Manager/Navigator for Unity** with enough features to build any type of game, even mid-core or more complex projects.
* Those who want to **learn the basics in 10 minutes**.
* Those who want to **master advanced features in 30 minutes**.

<h2>Concept</h2>

* Regardless of fullscreen page or modal/modeless window, everything is considered a **Screen**.
* At any given time, **only one Screen is visible** to optimize performance. If a Screen is shown on top of another, the underlying Screen will be temporarily hidden, and it will reappear when the top Screen is closed.
* A Screen is a freeform prefab that does **not require any specific scripts**, allowing it to be used as a child object anywhere.
* A **Scene** is freeform and contains any object, including its own UI canvas. The Scene’s canvas will not be hidden when a Screen is displayed on top.

<p align="center">
  <img width="200px" src="/learn/unity/ss/demo.gif?raw=true" alt="Demo">
  <p align="center">In this demo, even though they share the same prefab, the Store can be displayed as a modal window or as the content of the Store Tab.</p>
</p>

<h2>Basic Usage</h2>

<h3>1. Screen Settings</h3>
Set this to change the main canvas scaler and the game window size

```cs
From Menu: SS / Screen Settings / Input Screen Width & Height / Save
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/screen-settings.gif?raw=true" alt="Demo">
</p>

<h3>2. Create a screen</h3>

```cs
From Menu: SS / Screen Generator / Input Screen Name / Generate
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/screen-generator.gif?raw=true" alt="Demo">
</p>

<h3>3. Drag screen prefab to Resources/Screens folder</h3>In case of not using Addressables

<p align="center">
  <img width="500px" src="/learn/unity/ss/drag-screen.gif?raw=true" alt="Demo">
</p>

<h3>4. Add a screen on top with default animation</h3>

```cs
ScreenManager.Add<Screen1Controller>(screenName: "Screen1", onScreenLoad: (screen) => { });
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/add-screen.gif?raw=true" alt="Demo">
</p>

<h3>5. Close a screen</h3>

```cs
ScreenManager.Close();
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/close-screen.gif?raw=true" alt="Demo">
</p>

<h3>6. Load a scene with automatic fade</h3>

```cs
ScreenManager.Load<Scene1Controller>(sceneName: "Scene1", onSceneLoaded: (scene1) => { });
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/load-scene.gif?raw=true" alt="Demo">
</p>

<h2>Advance Usage</h2>

<h3>1. Addressables</h3>
If you want to use Addressables, you do not need to drag screen prefabs to Resources/Screens

<h4>1.1. Install Addressables package: </h4>

```cs
From Menu: Window / Package Manager / Unity Registry / Addressables / Install
```

<h4>1.2. Add this Scripting Define Symbol: ADDRESSABLE</h4>

```cs
From Menu: Edit / Project Settings / Player / Other Settings / Scripting Define Symbols / + / ADDRESSABLE / Apply
```

<p align="center">
  <img width="500px" src="/learn/unity/ss/advance/addressable-symbol.png?raw=true" alt="Demo">
</p>

<h4>1.3. Add screen prefab to addressables groups  </h4>

Make sure the addressable name is the same as the screen name, not a path to the prefab.

<p align="center">
  <img width="500px" src="learn/unity/ss/advance/addressable-groups.gif?raw=true" alt="Demo">
</p>