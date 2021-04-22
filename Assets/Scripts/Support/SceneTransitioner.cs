using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitioner
{
    #region Singleton

    private static SceneTransitioner _singleton;

    public static SceneTransitioner GetInstance()
    {
        if (_singleton == null)
        {
            _singleton = new SceneTransitioner();
        }

        return _singleton;
    }

    #endregion

    public void GoToScene(int indexScene)
    {
        SceneManager.LoadSceneAsync(indexScene, LoadSceneMode.Single);
    }

    public IEnumerator GoToSceneAfterNSeconds(int indexScene, float nSeconds)
    {
        yield return Waiter.Active(nSeconds);
        GoToScene(indexScene);
    }
}