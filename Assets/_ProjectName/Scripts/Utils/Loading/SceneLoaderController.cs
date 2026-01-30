using MEC;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoaderController : UIControllerBase
{
    public Action UnloadSceneComplete;
    public Action SwitchScenesCompleted;

    private CoroutineHandle sceneCoroutine;

    public override void Initialize(params object[] parameters)
    {
        
    }
    public void SwitchScenes(int currentScene, int nextScene)
    {
        ShowAllView();

        views.Initialize();

        sceneCoroutine = Timing.RunCoroutine(ChangeScene(currentScene, nextScene));
    }

    private IEnumerator<float> ChangeScene(int currentScene, int newScene)
    {
        if (SceneManager.GetSceneByBuildIndex(currentScene).isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);

            while (!unloadOperation.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            yield return Timing.WaitForOneFrame;
        }

        SwitchScenesCompleted?.Invoke();

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newScene));
        Timing.KillCoroutines(sceneCoroutine);

        HideAllView();
    }

    public void LoadScene(int scene)
    {
        ShowAllView();

        views.Initialize();

        sceneCoroutine = Timing.RunCoroutine(LoadSceneOperation(scene));
    }

    private IEnumerator<float> LoadSceneOperation(int scene)
    {
        HideAllView();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            yield return Timing.WaitForOneFrame;
        }

        SwitchScenesCompleted?.Invoke();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene));
        Timing.KillCoroutines(sceneCoroutine);
    }

    public void UnloadScene(int scene)
    {
        ShowAllView();

        views.Initialize();

        sceneCoroutine = Timing.RunCoroutine(UnloadSceneOperation(scene));
    }

    private IEnumerator<float> UnloadSceneOperation(int scene)
    {
        if (SceneManager.GetSceneAt(scene).isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(scene);

            while (!unloadOperation.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
        }

        UnloadSceneComplete?.Invoke();

        Timing.KillCoroutines(sceneCoroutine);

        HideAllView();
    }

    public override void Conclude()
    {

    }
}
