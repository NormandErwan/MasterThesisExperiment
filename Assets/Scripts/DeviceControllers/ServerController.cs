﻿using NormandErwan.MasterThesis.Experiment.Experiment.States;
using NormandErwan.MasterThesis.Experiment.Loggers;
using NormandErwan.MasterThesis.Experiment.UI.HUD;
using UnityEngine;

namespace NormandErwan.MasterThesis.Experiment.DeviceControllers
{
  public class ServerController : DeviceController
  {
    // Editor fields

    [SerializeField]
    private ServerHUD serverHUD;

    // MonoBehaviour methods

    protected override void Start()
    {
      base.Start();
      ParticipantLogger.DeviceControllerName = "server";

      serverHUD.BeginExperimentButtonPressed += ServerHUD_BeginExperimentButtonPressed;
      serverHUD.NextStateButtonPressed += ServerHUD_NextStateButtonPressed;
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      serverHUD.BeginExperimentButtonPressed -= ServerHUD_BeginExperimentButtonPressed;
      serverHUD.NextStateButtonPressed -= ServerHUD_NextStateButtonPressed;
    }

    // DeviceController methods

    protected override void StateController_CurrentStateUpdated(State currentState)
    {
      base.StateController_CurrentStateUpdated(currentState);
      serverHUD.UpdateInstructionsProgress(StateController);
    }

    protected virtual void ServerHUD_BeginExperimentButtonPressed()
    {
      serverHUD.DeactivateExperimentConfiguration();

      // Configure (sync) the experiment
      ParticipantId = serverHUD.ParticipantId;
      ConditionsOrdering = serverHUD.ConditionsOrdering;
      ParticipantIsRightHanded = serverHUD.ParticipantIsRightHanded;
      OnConfigureExperimentSync();

      // Set the ordering in conditions (sync)
      if (serverHUD.ConditionsOrdering == 1)
      {
        var mainIndVar = StateController.independentVariables[0];
        mainIndVar.NextCondition();
      }
      else if (serverHUD.ConditionsOrdering == 2)
      {
        var mainIndVar = StateController.independentVariables[0];
        mainIndVar.NextCondition();
        mainIndVar.NextCondition();
      }

      // Begin (sync) the experiment
      StateController.BeginExperiment();
    }

    protected virtual void ServerHUD_NextStateButtonPressed()
    {
      StateController.NextState();
    }
  }
}