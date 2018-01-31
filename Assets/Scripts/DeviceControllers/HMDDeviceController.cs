﻿using NormandErwan.MasterThesis.Experiment.Experiment.States;
using NormandErwan.MasterThesis.Experiment.Experiment.Task;
using NormandErwan.MasterThesis.Experiment.Experiment.Variables;
using NormandErwan.MasterThesis.Experiment.Inputs;
using NormandErwan.MasterThesis.Experiment.UI.HUD;
using UnityEngine;

namespace NormandErwan.MasterThesis.Experiment.DeviceControllers
{
  public class HMDDeviceController : DeviceController
  {
    // Constants

    protected float maxSelectableDistance = 0f;

    // Editor fields

    [SerializeField]
    private TaskGridMasks taskGridMasks;

    [SerializeField]
    private HMDDeviceHUD hmdDeviceHUD;

    [Header("Cursors")]
    [SerializeField]
    private LeapFingerCursorsInput leapFingerCursorsInput;

    [SerializeField]
    private Renderer rightLeapHand;

    [SerializeField]
    private Renderer leftLeapHand;

    // Properties

    public override CursorsInput CursorsInput { get { return leapFingerCursorsInput; } }

    // Methods

    protected override void Start()
    {
      base.Start();

      leapFingerCursorsInput.Configure(maxSelectableDistance);
      leapFingerCursorsInput.enabled = false;

      // TODO: remove, for debug testing only
      //ParticipantIsRightHanded = true;
      //StartCoroutine(StartTaskDebug());
    }

    // DeviceController methods

    public override void ActivateTask()
    {
      base.ActivateTask();

      leapFingerCursorsInput.enabled = true;
      foreach (var cursor in leapFingerCursorsInput.Cursors)
      {
        cursor.Value.SetActive(cursor.Value.IsIndex 
          && technique.CurrentCondition.useLeapInput
          && (ParticipantIsRightHanded == cursor.Value.IsRightHanded));
      }

      hmdDeviceHUD.ShowContent(false);
    }

    protected override void StateController_CurrentStateUpdated(State currentState)
    {
      base.StateController_CurrentStateUpdated(currentState);

      hmdDeviceHUD.ShowContent(true);
      hmdDeviceHUD.UpdateInstructionsProgress(StateController);
    }

    protected override void TaskGrid_Configured()
    {
      base.TaskGrid_Configured();
      taskGridMasks.Configure(!technique.CurrentCondition.showTaskGridOnlyOnMobileDevice);
    }

    protected override void TaskGrid_Completed()
    {
      base.TaskGrid_Completed();
      taskGridMasks.Hide();
    }
  }
}