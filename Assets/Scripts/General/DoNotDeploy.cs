using UnityEngine;

public class DoNotDeploy : MonoBehaviour
{
    void Awake()
    {
        if (Debug.isDebugBuild) //Check if this is a development build.
        {
            IDebug[] debugItems = GetComponents<IDebug>();
            foreach (IDebug script in debugItems) //If this GO is holding debug scripts, enable them.
            {
                script.ToggleActive();
            }
        }
        else Destroy(gameObject); //Destroy if this is a deployment build.
    }
}
