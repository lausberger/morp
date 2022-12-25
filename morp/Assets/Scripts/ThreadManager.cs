using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
  private static readonly List<Action> executeOnMainThread = new List<Action>();
  private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
  private static bool actionToExecuteOnMainThread = false;

  private void Update()
  {
    UpdateMain();
  }

  public static void ExecuteOnMainThread(Action _action)
  {
    if (_action == null)
    {
      Debug.Log("ThreadManager: No action to execute on main thread");
      return;
    }

    lock (executeOnMainThread)
    {
      executeOnMainThread.Add(_action);
      actionToExecuteOnMainThread = true;
    }
  }

  // call this only from the main thread
  public static void UpdateMain()
  {
    if (!actionToExecuteOnMainThread)
    {
      return;
    }
    
    executeCopiedOnMainThread.Clear();
    lock (executeOnMainThread)
    {
      executeCopiedOnMainThread.AddRange(executeOnMainThread);
      executeOnMainThread.Clear();
      actionToExecuteOnMainThread = false;
    }

    for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
    {
      executeCopiedOnMainThread[i]();
    }
  }
}