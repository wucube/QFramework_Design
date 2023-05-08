using System.Collections;
using System.Collections.Generic;
using CounterApp;
using UnityEngine;
using QFramework;

public class SubCountCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<ICounterModel>().Count.Value--;
    }
}
